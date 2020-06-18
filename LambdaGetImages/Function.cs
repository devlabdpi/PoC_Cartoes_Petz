using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaGetImages
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

                var storageClient = new GoogleCloudStorage.StorageClient(request.TotemID);
                var items = storageClient.GetImages();
                storageClient.Dispose();
                return JsonSerializer.Serialize(items);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
