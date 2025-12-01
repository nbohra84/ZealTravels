using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using ZealTravel.Common;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Interfaces.UniversalAPI;

namespace ZealTravel.Infrastructure.UAPI
{
    public class UniversalApi :IUniversalApi
    {
         public  string GetRequestMC(ArrayList AirlineList, string Cabin, Int16 Adult, Int16 Child, Int16 Infant)
        {
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<Cabin>" + Cabin + "</Cabin>");

            ReqXml.Append("<AirSearch>");

            ReqXml.Append("<AirSrchInfo>");
            ReqXml.Append("<DepartureStation>" + "DEL" + " </DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + "BOM" + "</ArrivalStation>");
            ReqXml.Append("<StartDate>" + "20200110" + "</StartDate>");
            ReqXml.Append("</AirSrchInfo>");

            ReqXml.Append("<AirSrchInfo>");
            ReqXml.Append("<DepartureStation>" + "BOM" + " </DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + "AMD" + "</ArrivalStation>");
            ReqXml.Append("<StartDate>" + "20200112" + "</StartDate>");
            ReqXml.Append("</AirSrchInfo>");

            ReqXml.Append("<AirSrchInfo>");
            ReqXml.Append("<DepartureStation>" + "BLR" + " </DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + "DEL" + "</ArrivalStation>");
            ReqXml.Append("<StartDate>" + "20200115" + "</StartDate>");
            ReqXml.Append("</AirSrchInfo>");

            //ReqXml.Append("<AirSrchInfo>");
            //ReqXml.Append("<DepartureStation>" + "DEL" + " </DepartureStation>");
            //ReqXml.Append("<ArrivalStation>" + "BOM" + "</ArrivalStation>");
            //ReqXml.Append("<StartDate>" + "20191017" + "</StartDate>");
            //ReqXml.Append("</AirSrchInfo>");


            ReqXml.Append("</AirSearch>");

            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < AirlineList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + AirlineList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }
            ReqXml.Append("</AirVAry>");

            ReqXml.Append("<Adult>" + Adult + "</Adult>");
            ReqXml.Append("<Child>" + Child + "</Child>");
            ReqXml.Append("<Infant>" + Infant + "</Infant>");
            ReqXml.Append("</AvailabilityRequest>");
            return ReqXml.ToString();
        }
        public  string GetRequest(string DepartureStation, string ArrivalStation, string Cabin, ArrayList AirlineList, string BeginDate, string ArrivalDate, int Adult, int Child, int Infant)
        {
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<DepartureStation>" + DepartureStation + "</DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + ArrivalStation + "</ArrivalStation>");

            ReqXml.Append("<Cabin>" + Cabin + "</Cabin>");
            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < AirlineList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + AirlineList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }

            ReqXml.Append("</AirVAry>");
            ReqXml.Append("<StartDate>" + BeginDate + "</StartDate>");
            ReqXml.Append("<EndDate>" + ArrivalDate + "</EndDate>");
            ReqXml.Append("<Adult>" + Adult + "</Adult>");
            ReqXml.Append("<Child>" + Child + "</Child>");
            ReqXml.Append("<Infant>" + Infant + "</Infant>");
            ReqXml.Append("<SplFare>" + false + "</SplFare>");
            ReqXml.Append("</AvailabilityRequest>");
            return ReqXml.ToString();
        }

        public string GetResponseUapi(string NetworkUserName, string NetworkPassword, string SearchID, string requestData, string Method, string ServiceName)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                //P7025891 //test
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string url = ConfigurationHelper.GetSetting("GalileoAirline:BaseURL") + Method; //test

                byte[] data = Encoding.UTF8.GetBytes(requestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
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
                DBCommon.Logger.dbLogg("", 0, "GetResponseUapi", requestData, responseXML, SearchID, Method + "-" + ServiceName + "-" + ex.Message);
            }

            WriteFileToxml(ServiceName, requestData, ServiceName + "_RQ");
            WriteFileToxml(ServiceName, responseJSON, ServiceName + "RS");
            return responseJSON;
        }

        public async Task<string> GetResponseUapiAsync(string NetworkUserName, string NetworkPassword, string SearchID, string requestData, string Method, string ServiceName)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                //P7025891 //test
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string url = ConfigurationHelper.GetSetting("GalileoAirline:BaseURL") + Method;

                byte[] data = Encoding.UTF8.GetBytes(requestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
                request.Credentials = cred;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                //using (WebResponse response =await System.Threading.Tasks.Task.Run(()=> request.GetResponseAsync()))

                using (WebResponse response = await request.GetResponseAsync())
                {

                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = await rd.ReadToEndAsync();
                    }
                }
            }
            catch (WebException ex)
            {
                //DBCommon.Logger.dbLogg("", 0, "GetResponseUapi", requestData, responseXML, SearchID, Method + "-" + ServiceName + "-" + ex.Message);
            }

            WriteFileToxml(ServiceName, requestData, ServiceName + "_RQ");
            WriteFileToxml(ServiceName, responseJSON, ServiceName + "RS");
            return responseJSON;
        }

        private static void WriteFileToxml(string SearchID, string Response, string FileName)
        {
            try
            {
                string Path = @"C://uapiLog//" + FileName + ".xml";
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
                file.WriteLine(Response);
                file.Close();
            }
            catch (Exception ex)
            {

            }
        }
        /*private static void WriteFileToxmlAsync(string SearchID, string Response, string FileName)
        {
            try
            {
                string Path = @"C://uapiLog//" + FileName + ".xml";
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
                file.WriteLineAsync(Response);
                file.Close();
            }
            catch (Exception ex)
            {

            }
        }*/
    }
}
