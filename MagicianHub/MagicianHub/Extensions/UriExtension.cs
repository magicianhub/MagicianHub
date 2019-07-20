using System;
using System.Collections.Generic;

namespace MagicianHub.Extensions
{
    public static class UriExtension
    {
        public static string[] GetPartAsArray(this Uri uri)
        {
            var uriString = uri.ToString();
            var uriParts = uriString.Remove(0, "magicianhub://".Length);
            uriParts = uriParts.Replace("/?", "?");
            return uriParts.Split('?')[0].Split('/');
        }

        /**
         * E.g we call it method with magicianhub://auth?login=test&pass=test
         * and it method return just dict with keys: login, pass and values: test, test.
         */
        public static Dictionary<string, string> GetPartParameters(this Uri uri)
        {
            var parameters = new Dictionary<string, string>();
            var uriString = uri.ToString();
            try
            {
                if (!uriString.Contains("?")) return parameters;
                var uriParameters = uriString.Split('?')[1].Split('&');
                foreach (var parameter in uriParameters)
                {
                    var keyAndValue = parameter.Split('=');
                    parameters.Add(keyAndValue[0], keyAndValue[1]);
                }
            }
            catch (IndexOutOfRangeException)
            {
                return parameters;
            }

            return parameters;
        }
    }
}
