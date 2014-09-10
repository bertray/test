///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Toyota.Common.Utilities
{
    public class NetworkUtils
    {
        private NetworkUtils() { }

        public static Stream HttpPost(string url, IDictionary<string, string> parameters)
        {
            if (!string.IsNullOrEmpty(url))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (request != null)
                {
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";

                    if ((parameters != null) && (parameters.Count > 0))
                    {
                        StringBuilder paramBuilder = new StringBuilder();
                        foreach (string key in parameters.Keys)
                        {
                            paramBuilder.Append(key).Append('=').Append(parameters[key]).Append('&');
                        }
                        paramBuilder.Remove(paramBuilder.Length - 1, 1);

                        byte[] paramBytes = Encoding.ASCII.GetBytes(paramBuilder.ToString());
                        if ((paramBytes != null) && (paramBytes.Length > 0))
                        {
                            request.ContentLength = paramBytes.Length;
                            Stream requestStream = request.GetRequestStream();
                            requestStream.Write(paramBytes, 0, paramBytes.Length);
                            requestStream.Close();
                        }                        
                    }
                    else
                    {
                        request.ContentLength = 0;
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response != null)
                    {
                        response.GetResponseStream();
                    }
                }
            }
            return null;
        }
    }
}
