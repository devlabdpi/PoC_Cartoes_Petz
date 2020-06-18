using ClassLibrary;
using Google.Api.Gax.ResourceNames;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoogleCloudStorage
{
    public class StorageClient
    {
        private static readonly GoogleCredential CREDENTIALS = GoogleCredential.FromFile(Constants.CREDENTIALS_FILE);

        private readonly Google.Cloud.Storage.V1.StorageClient _client;
        private readonly string BucketName;
        private readonly long TotemId;

        public StorageClient(long totemId)
        {
            TotemId = totemId;
            BucketName = string.Format("{0}-{1:D6}",Constants.PROJECT_ID, TotemId);
            _client = Google.Cloud.Storage.V1.StorageClient.Create(CREDENTIALS);
        }

        public SimpleResponse SaveImage(byte[] bytes, string imageName)
        {
            try
            {
                var stream = new MemoryStream(bytes);
                var result = _client.UploadObject(BucketName, imageName, "image/jpeg", stream);

                return new SimpleResponse {
                    Success = true,
                    Description = result.MediaLink
                };
            }
            catch (Exception e)
            {
                return new SimpleResponse
                {
                    Success = false,
                    Description = e.Message
                };
            }
        }

        public List<ImageItem> GetImages()
        {
            var list = new List<ImageItem>();

            var items = _client.ListObjects(BucketName);

            foreach(var item in items)
            {
                list.Add(new ImageItem
                {
                    Numero = short.Parse(item.Name.Replace(".jpeg", "")),
                    MediaLink = item.MediaLink,
                    DataUpload = item.TimeCreated.GetValueOrDefault()
                });
            }

            return list;
        }

        public SimpleResponse ClearBucket()
        {
            try
            {
                var list = _client.ListObjects(BucketName);

                foreach (var item in list)
                {
                    _client.DeleteObject(item);
                }

                return new SimpleResponse
                {
                    Success = true,
                    Description = "Imagens do repositório foram apagadas."
                };
            }
            catch (Exception e)
            {
                return new SimpleResponse
                {
                    Success = false,
                    Description = e.Message
                };
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
