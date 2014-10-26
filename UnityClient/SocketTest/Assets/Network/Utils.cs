using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class Utils
    {

        public static string Base64Encoding(string EncodingText)
        {
            System.Text.Encoding oEncoding = System.Text.Encoding.UTF8;
            byte[] arr = oEncoding.GetBytes(EncodingText);
            return System.Convert.ToBase64String(arr);
        }

        public static string Base64Decoding(string DecodingText)
        {
            System.Text.Encoding oEncoding = System.Text.Encoding.UTF8;
            byte[] arr = System.Convert.FromBase64String(DecodingText);
            return oEncoding.GetString(arr);
        }


    }