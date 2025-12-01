
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ZealTravel.Common.Helpers;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetQueue
    {
        public string GetOwnPnr(string PNR)
        {
            string sRequest1 = GetUniversalRecordServiceRequest(PNR);
            return GetResponse(sRequest1, "UniversalRecordService");
        }
        public string GetOtherPnr(string PNR)
        {
            string sRequest11 = GetGdsQueueServiceRequest(PNR);
            return GetResponse(sRequest11, "GdsQueueService");
        }

        private string GetGdsQueueServiceRequest(string PNR)
        {
            string requestData = "";
            requestData += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
            requestData += @"<soapenv:Header/>";
            requestData += @"<soapenv:Body>";
            requestData += @"<gds:GdsEnterQueueReq TargetBranch=""P3469150"" PseudoCityCode=""5O39"" ProviderLocatorCode=""XXXXXX"" ProviderCode=""1G"" AuthorizedBy=""ZEALTRAVELS"" xmlns:gds=""http://www.travelport.com/schema/gdsQueue_v34_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"">";
            requestData += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";
            requestData += @"<com:QueueSelector Queue=""60""/>";
            requestData += @"</gds:GdsEnterQueueReq>";
            requestData += @"</soapenv:Body>";
            requestData += @"</soapenv:Envelope>";
            return requestData.Replace("XXXXXX", PNR);
        }
        private string GetUniversalRecordServiceRequest(string PNR)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">");
            sb.Append(@"<soapenv:Header/>");
            sb.Append(@"<soapenv:Body>");
            sb.Append(@"<univ:UniversalRecordRetrieveReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" xmlns:univ=""http://www.travelport.com/schema/universal_v51_0"" TargetBranch=""P3469150"">");
            sb.Append(@"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>");
            sb.Append(@"<univ:ProviderReservationInfo ProviderLocatorCode=""XXXXXX"" ProviderCode=""1G""/>");
            sb.Append(@"</univ:UniversalRecordRetrieveReq>");
            sb.Append(@"</soapenv:Body>");
            sb.Append(@"</soapenv:Envelope>");
            string requestData = sb.ToString();
            return requestData.Replace("XXXXXX", PNR);
        }
        private string GetResponse(string requestData, string Method)
        {
            string responseJSON = string.Empty;

            try
            {
                string url = ConfigurationHelper.GetSetting("GalileoAirline:BaseURL") + Method;
                byte[] data = Encoding.UTF8.GetBytes(requestData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");

                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                NetworkCredential cred = new NetworkCredential("Universal API/uAPI1511687902-c69284f5", "w=6JYo*57T");
                request.Credentials = cred;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = rd.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponse", requestData, responseJSON, Method, ex.Message);
            }
            return responseJSON;
        }
    }
}
