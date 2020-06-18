using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using ClassLibrary;
using Google.Cloud.Vision.V1;
using GoogleCloudStorage;
using GoogleCloudVision;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaAnalyzeImage
{
    public class Function
    {

        private static readonly VisionClient visionClient = new VisionClient();
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(JsonElement json, ILambdaContext context)
        {
            try
            {
                string bodyEncoded = json.GetProperty("body").ToString();
                string body = "";

                try
                {
                    body = Encoding.UTF8.GetString(Convert.FromBase64String(bodyEncoded));
                }
                catch (Exception)
                {
                    body = bodyEncoded;
                }

                var request = JsonSerializer.Deserialize<LambdaRequest>(body);

                byte[] bytes = Convert.FromBase64String(request.ImagemBase64);
                Image im = Image.FromBytes(bytes);

                var result = visionClient.AnalyzeImage(im, Constants.DEFAULT_MIN_CONFIDENCE);

                result.TotemID = request.TotemID;
                result.NumeroImagem = request.NumeroImagem;

                if (result.Sucesso)
                {
                    var storageClient = new StorageClient(request.TotemID);
                    var resp = storageClient.SaveImage(bytes,string.Format("{0}.jpeg",request.NumeroImagem));

                    if (!resp.Success)
                    {
                        result.Sucesso = false;
                        result.Descricao = "Não foi possível salvar a imagem no repositório." + resp.Description;
                    }
                    else
                    {
                        result.Sucesso = true;
                        result.MediaLink = resp.Description;
                        result.Descricao = "Imagem OK - salva no repositório.";
                    }

                    try
                    {
                        storageClient.Dispose();
                    }
                    catch (Exception) { }
                }

                return JsonSerializer.Serialize(result);
            }
            catch (Exception e)
            {
                return string.Format("JSON: {0} \n\n EXCEPTION: {1}", json.ToString(), e.ToString());
            }
        }
    }
}
