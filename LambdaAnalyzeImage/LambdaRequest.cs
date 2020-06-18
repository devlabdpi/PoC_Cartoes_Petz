using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaAnalyzeImage
{
    public class LambdaRequest
    {
        public virtual long TotemID { get; set; }
        public virtual int NumeroImagem { get; set; }
        public virtual string ImagemBase64 { get; set; }


        public LambdaRequest() { }
        public LambdaRequest(long totemId, int imageNumber, string imageBase64)
        {
            TotemID = totemId;
            NumeroImagem = imageNumber;
            ImagemBase64 = imageBase64;
        }
    }
}
