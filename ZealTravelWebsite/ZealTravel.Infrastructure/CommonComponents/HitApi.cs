using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Loggers;

namespace ZealTravel.Infrastructure.CommonComponents
{
    public class HitApi
    {
        public static string HitToApi(string apiPath, string request, string acceptVerb)
        {
            StreamReader streamReader = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiPath);
            httpWebRequest.Accept = acceptVerb;
            httpWebRequest.ContentType = acceptVerb;
            httpWebRequest.Method = "POST";
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            byte[] bytes = uTF8Encoding.GetBytes(request);
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.Timeout = 360000;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Flush();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpWebResponse.Headers.Get("Content-Encoding") == "gzip" || httpWebResponse.Headers.Get("Content-Encoding") == "deflate")
            {
                Stream stream = null;
                stream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                streamReader = new StreamReader(stream);
            }
            else
            {
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            }

            return streamReader.ReadToEnd();
        }

        public static string HitToApi(string apiPath, string request, string acceptVerb, ILogger logger)
        {
            StreamReader streamReader = null;
            try
            {
                //logger.AddToLog("Hit to hotel API start");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiPath);
                httpWebRequest.Accept = acceptVerb;
                httpWebRequest.ContentType = acceptVerb;
                httpWebRequest.Method = "POST";
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                byte[] bytes = uTF8Encoding.GetBytes(request);
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.Timeout = 360000;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.Headers.Get("Content-Encoding") == "gzip" || httpWebResponse.Headers.Get("Content-Encoding") == "deflate")
                {
                    Stream stream = null;
                    stream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                    streamReader = new StreamReader(stream);
                }
                else
                {
                    streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                }
            }
            catch (WebException ex)
            {
                WebResponse response = ex.Response;
                Stream stream2 = null;
                if (response.Headers.Get("Content-Encoding") == "gzip" || response.Headers.Get("Content-Encoding") == "deflate")
                {
                    stream2 = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream2 = response.GetResponseStream();
                }

                //logger.AddToLog("WebException" + ex.Message);
            }
            catch (Exception ex2)
            {
                //logger.AddToLog("Exception" + ex2.Message);
            }

            //logger.AddToLog("Hit to hotel API end");
            return streamReader.ReadToEnd();
        }

        public static string HitToApi(string apiPath, string acceptVerb)
        {
            StreamReader streamReader = null;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiPath);
            httpWebRequest.Accept = acceptVerb;
            httpWebRequest.ContentType = acceptVerb;
            httpWebRequest.Method = "GET";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpWebResponse.Headers.Get("Content-Encoding") == "gzip" || httpWebResponse.Headers.Get("Content-Encoding") == "deflate")
            {
                Stream stream = null;
                stream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                streamReader = new StreamReader(stream);
            }
            else
            {
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            }

            return streamReader.ReadToEnd();
        }

        public static string HitApiWIthCollection(object _object, string ApiUrl, string acceptVerb)
        {
            string result = string.Empty;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptVerb));
            HttpResponseMessage result2 = httpClient.PostAsJsonAsync(ApiUrl, _object).Result;
            using (HttpContent httpContent = result2.Content)
            {
                Task<string> task = httpContent.ReadAsStringAsync();
                result = task.Result;
            }

            return result;
        }

        public static string HitToApi(string apiPath, string request, string acceptVerb, Dictionary<string, string> HeaderKeys)
        {
            StreamReader streamReader = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiPath);
            httpWebRequest.Accept = acceptVerb;
            httpWebRequest.ContentType = acceptVerb;
            httpWebRequest.Method = "POST";
            if (HeaderKeys != null && HeaderKeys.Count() > 0)
            {
                foreach (KeyValuePair<string, string> HeaderKey in HeaderKeys)
                {
                    httpWebRequest.Headers.Add(HeaderKey.Key, HeaderKey.Value);
                }
            }

            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            byte[] bytes = uTF8Encoding.GetBytes(request);
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.Timeout = 360000;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Flush();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpWebResponse.Headers.Get("Content-Encoding") == "gzip" || httpWebResponse.Headers.Get("Content-Encoding") == "deflate")
            {
                Stream stream = null;
                stream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                streamReader = new StreamReader(stream);
            }
            else
            {
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            }

            return streamReader.ReadToEnd();
        }
    }
}
