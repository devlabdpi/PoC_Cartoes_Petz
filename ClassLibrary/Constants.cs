using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public class Constants
    {
        //General
        public static readonly string PROJECT_ID = "analiseimagempetz";
        public static readonly string CREDENTIALS_FILE = "testkey.json";

        //CLoud Vision
        public static readonly float DEFAULT_MIN_CONFIDENCE = 0.85f;
        
        //Cloud Storage
        public static readonly string IMAGE_OUTPUT_URL = "https://storage.cloud.google.com/{0}/{1}"; 


    }
}
