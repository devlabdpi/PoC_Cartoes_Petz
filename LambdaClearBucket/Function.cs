using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaClearBucket
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(JsonElement json, ILambdaContext context)
        {
            var response = new Response();
            response.TotemID = 0;

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
                response.TotemID = request.TotemID;


                var storageClient = new GoogleCloudStorage.StorageClient(request.TotemID);
                storageClient.ClearBucket();
                storageClient.Dispose();

                response.Sucesso = true;
                response.Descricao = "Repositório esvaziado com sucesso.";
            }
            catch (Exception e)
            {
                response.Sucesso = false;
                response.Descricao = e.Message;
            }

            return JsonSerializer.Serialize(response);
        }
    }
}
