using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZealTravel.Domain.Interfaces.TBO;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetApiCommonFunctions: IApiCommonFunctions
    {
        //public string errorMessage;

        public string ApiBookingid;
        public string InvoiceNumber;
        public string AirlinePNR;
        public string PNR;
        public bool IsTimeChanged;
        public bool IsPriceChanged;
        public bool IsChangeItinery;
        public ArrayList ArayTicketNumber;

        public string errorMessage { get ; set ; }

        public void GetPnrTicketFromResponse(int BookingRef, string FltType, string Response)
        {
            PNR = string.Empty;
            AirlinePNR = string.Empty;
            IsChangeItinery = false;
            IsTimeChanged = false;
            IsPriceChanged = false;
            ApiBookingid = string.Empty;
            InvoiceNumber = string.Empty;

            try
            {
                XmlDocument XMLResponse = new XmlDocument();
                XMLResponse.LoadXml(Response);

                PNR = XMLResponse.SelectSingleNode("Response/Response/PNR").InnerText;
                ApiBookingid = XMLResponse.SelectSingleNode("Response/Response/BookingId").InnerText;

                if (XMLResponse.SelectSingleNode("Response/Response/FlightItinerary") != null && XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/Segments") != null &&
                    XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/Segments/AirlinePNR") != null)
                {
                    AirlinePNR = XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/Segments/AirlinePNR").InnerText;
                }

                if (AirlinePNR == null || AirlinePNR.Trim().Equals(string.Empty))
                {
                    AirlinePNR = PNR;
                }

                if (XMLResponse.SelectSingleNode("Response/Response/FlightItinerary") != null && XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/InvoiceNo") != null)
                {
                    InvoiceNumber = XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/InvoiceNo").InnerText;
                }

                if (XMLResponse.SelectSingleNode("Response/Response/IsTimeChanged") != null)
                {
                    IsTimeChanged = Convert.ToBoolean(XMLResponse.SelectSingleNode("Response/Response/IsTimeChanged").InnerText);
                }

                if (XMLResponse.SelectSingleNode("Response/Response/IsPriceChanged") != null)
                {
                    IsPriceChanged = Convert.ToBoolean(XMLResponse.SelectSingleNode("Response/Response/IsPriceChanged").InnerText);
                }

                if (IsTimeChanged || IsPriceChanged)
                {
                    IsChangeItinery = true;
                }

                try
                {
                    ArayTicketNumber = new ArrayList();
                    if (XMLResponse.SelectSingleNode("Response/Response/FlightItinerary") != null && XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/Passenger") != null &&
                       XMLResponse.SelectSingleNode("Response/Response/FlightItinerary/Passenger/Ticket") != null)
                    {
                        XmlNodeList nodeTKT = XMLResponse.SelectNodes("Response/Response/FlightItinerary/Passenger/Ticket"); // You can also use XPath here
                        foreach (XmlNode node in nodeTKT)
                        {
                            ArayTicketNumber.Add(node["TicketNumber"].InnerText.Trim().ToUpper());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.errorMessage = ex.Message;
                }
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg("", BookingRef, "GetPnrTicketFromResponse", "air_tbo-GetApiCommonFunctions", Response, "", errorMessage);
            }
        }

        public ArrayList GetTicketNumberFromResponse(string Response)
        {
            ArrayList ArayList = new ArrayList();

            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(Response);

                string FareBreakdown = "<Passengers>";
                XmlNodeList nodesFareBreakdown = xmldoc.SelectNodes("Response/Response/FlightItinerary/Passenger"); // You can also use XPath here
                foreach (XmlNode node in nodesFareBreakdown)
                {
                    FareBreakdown += node.OuterXml;
                }
                FareBreakdown += "</Passengers>";

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(FareBreakdown);

                XmlNodeList nodeTKT = xml.SelectNodes("Passengers/Passenger"); // You can also use XPath here
                foreach (XmlNode node in nodeTKT)
                {
                    XmlNodeList css = node.ChildNodes;
                    foreach (XmlNode nodeI in css)
                    {
                        XmlDocument xmlPassengers = new XmlDocument();
                        xmlPassengers.LoadXml(nodeI.InnerXml);

                        string TicketNumber = xmlPassengers.SelectSingleNode("Passengers/ValidatingAirline").InnerText;
                        TicketNumber += xmlPassengers.SelectSingleNode("Passengers/TicketNumber").InnerText;

                        ArayList.Add(TicketNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg("", 0, "GetTicketNumberFromResponse", "air_tbo-GetServices", Response, "", errorMessage);
            }

            return ArayList;
        }

        //-----------------------------------------------------------------------------------------------------------
        public static bool IsDirectTicketing(string CarrierCode)
        {
            if (GetCommonFunctions.IsLCC(CarrierCode).Equals(true) || CarrierCode.Equals("G9") || CarrierCode.Equals("FZ") || CarrierCode.Equals("ZO") || CarrierCode.Equals("2T") || CarrierCode.Equals("AK") || CarrierCode.Equals("FD") || CarrierCode.Equals("D7") || CarrierCode.Equals("QZ") || CarrierCode.Equals("XJ") || CarrierCode.Equals("XT") || CarrierCode.Equals("Z2") || CarrierCode.Equals("I5") || CarrierCode.Equals("IX") || CarrierCode.Equals("TR") || CarrierCode.Equals("XW"))
            {
                return true;
            }
            return false;
        }
        //-----------------------------------------------------------------------------------------------------------
        public static void Set_Journey_Duration_TimeDetail(DataTable dtBound)
        {
            SetJourneyTime(dtBound);
            foreach (DataRow dr in dtBound.Rows)
            {
                dr["JourneyTimeDesc"] = GetDateTimeFormatter.ConvertTmDesc(dr["JourneyTime"].ToString());

                if (Convert.ToInt32(dr["Duration"].ToString()) > 0)
                {
                    dr["DurationDesc"] = GetDateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                }
                else
                {
                    dr["Duration"] = GetDateTimeFormatter.GetDuration(dr["DepDate"].ToString(), dr["ArrDate"].ToString());
                    dr["DurationDesc"] = GetDateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                }

                dr["DepartureDate"] = GetDateTimeFormatter.DateFormat(dr["DepDate"].ToString());
                dr["ArrivalDate"] = GetDateTimeFormatter.DateFormat(dr["ArrDate"].ToString());
                dr["DepartureTime"] = GetDateTimeFormatter.TimeFormat(dr["DepTime"].ToString());
                dr["ArrivalTime"] = GetDateTimeFormatter.TimeFormat(dr["ArrTime"].ToString());
            }
            dtBound.AcceptChanges();
        }
        private static void SetJourneyTime(DataTable dtBound)
        {
            foreach (DataRow dr in dtBound.Rows)
            {
                if (Convert.ToInt32(dr["JourneyTime"].ToString()).Equals(0))
                {
                    SetJourneyTime(dr["RefID"].ToString(), dr["FltType"].ToString(), dtBound);
                }
            }
        }
        private static void SetJourneyTime(string RefID, string FltType, DataTable dtBound)
        {
            int iJourneyTime = 0;
            DataRow[] rows = dtBound.Select("RefID ='" + RefID + "' And FltType='" + FltType + "'");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    if (Convert.ToInt32(row["JourneyTime"].ToString()) > 0)
                    {
                        iJourneyTime = Convert.ToInt32(row["JourneyTime"].ToString());
                        break;
                    }
                }
                if (iJourneyTime > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        if (Convert.ToInt32(row["JourneyTime"].ToString()).Equals(0))
                        {
                            row["JourneyTime"] = iJourneyTime;
                        }
                        dtBound.AcceptChanges();
                        row.SetModified();
                    }
                }
                else
                {
                    iJourneyTime = GetDateTimeFormatter.GetDuration(rows.CopyToDataTable().Rows[0]["DepDate"].ToString(), rows.CopyToDataTable().Rows[rows.CopyToDataTable().Rows.Count - 1]["ArrDate"].ToString());
                    if (iJourneyTime > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            if (Convert.ToInt32(row["JourneyTime"].ToString()).Equals(0))
                            {
                                row["JourneyTime"] = iJourneyTime;
                            }
                            dtBound.AcceptChanges();
                            row.SetModified();
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        public DataTable SetFltType(DataTable dtBound)
        {
            foreach (DataRow dr in dtBound.Rows)
            {
                dr["FltType"] = "I";
                dr["RefID"] = Convert.ToInt32(dr["RefID"].ToString()) + 10000;
            }
            dtBound.AcceptChanges();

            return dtBound;
        }
        public static string GetCabinClass(string Cabin)
        {
            if (Cabin.Equals("Y")) // 1 for All 2 for Economy 3 for PremiumEconomy 4 for Business 5 for PremiumBusiness 6 for First)
            {
                return "1";
            }
            else if (Cabin.Equals("C"))
            {
                return "4";
            }
            else if (Cabin.Equals("A"))
            {
                return "3";
            }
            else if (Cabin.Equals("B"))
            {
                return "5";
            }
            return "0";
        }
        public static string GetApiRequestDateFormat(string Date)
        {
            string dd = Date.Substring(6, 2);
            string mm = Date.Substring(4, 2);
            string yy = Date.Substring(0, 4);
            return yy + "-" + mm + "-" + dd;
        }
        public string GetApiHttpResponseRoot(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Request, string ServiceUrl, string MethodName)
        {
            string ResponseXML = string.Empty;
            string ResponseJSON = string.Empty;

            try
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol =
  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    byte[] data = Encoding.UTF8.GetBytes(Request);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceUrl);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    request.Headers.Add(@"SOAP:Action");
                    request.Headers.Add("Accept-Encoding", "gzip");
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            ResponseJSON = rd.ReadToEnd();
                            JObject _JObject = JObject.Parse(ResponseJSON);
                            ResponseXML = JsonConvert.DeserializeXmlNode(ResponseJSON, "root").InnerXml;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    errorMessage = webEx.Message;
                    //DBCommon.Logger.dbLogg(Companyid,BookingRef, MethodName, "air_tbo-GetApiResponseRoot", Supplierid, Searchid, webEx.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, BookingRef, MethodName, "air_tbo-GetApiResponseRoot", Supplierid, Searchid, errorMessage);
            }

            //string id = "0.";
            //WriteFileToTxt(Request, id + MethodName + "_RQ_", false);
            //WriteFileToTxt(ResponseJSON, id + MethodName + "_RS_", false);
            //WriteFileToTxt(ResponseXML, id + MethodName + "_RS_", true);

            //DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, "AVAILABILITY", Request, Supplierid, Searchid);
            //DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, "AVAILABILITY", ResponseJSON, Supplierid, Searchid);
            //DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, "AVAILABILITY", ResponseXML, Supplierid, Searchid);

            return ResponseXML;
        }
        public string GetApiHttpResponse(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Request, string ServiceUrl, string MethodName)
        {
            string ResponseXML = string.Empty;
            string ResponseJSON = string.Empty;

            try
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    byte[] data = Encoding.UTF8.GetBytes(Request);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceUrl);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    request.Headers.Add(@"SOAP:Action");
                    request.Headers.Add("Accept-Encoding", "gzip");
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            ResponseJSON = rd.ReadToEnd();
                            JObject _JObject = JObject.Parse(ResponseJSON);
                            ResponseXML = JsonConvert.DeserializeXmlNode(ResponseJSON).InnerXml;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    errorMessage = webEx.Message;
                    //DBCommon.Logger.dbLogg(Companyid, BookingRef, MethodName, "air_tbo-GetApiHttpResponse", Supplierid, Searchid, webEx.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, BookingRef, MethodName, "air_tbo-GetApiHttpResponse", Supplierid, Searchid, errorMessage);
            }

            string Location = "";
            string id = "0.";
            if (MethodName.ToUpper().Equals("SEARCH"))
            {
                Location = "AVAILABILITY";
                id = "1.";
            }
            else if (MethodName.ToUpper().Equals("FareRule".ToUpper()))
            {
                Location = "FARE";
                id = "2.";
            }
            else if (MethodName.ToUpper().Equals("FareQuote".ToUpper()))
            {
                Location = "FARE";
                id = "3.";
            }
            else if (MethodName.ToUpper().Equals("SSR".ToUpper()))
            {
                Location = "SSR";
                id = "4.";
            }
            else if (MethodName.ToUpper().Equals("Book".ToUpper()))
            {
                Location = "PNR";
                id = "5.";
            }
            else if (MethodName.ToUpper().Equals("Ticket".ToUpper()))
            {
                Location = "TKT";
                id = "6.";
            }
            else if (MethodName.ToUpper().Equals("GetBookingDetails".ToUpper()))
            {
                Location = "STORE";
                id = "7.";
            }

            //WriteFileToTxt(Request, id + MethodName + "_RQ_", false);
            //WriteFileToTxt(ResponseJSON, id + MethodName + "_RS_", false);
            //WriteFileToTxt(ResponseXML, id + MethodName + "_RS_", true);

            if (Location.Equals("TKT") || Location.Equals("PNR"))
            {
               // DBCommon.Logger.dbLoggAPI(Searchid, Companyid, BookingRef, "", Supplierid, "PNR", Request, ResponseXML, "");
            }

           // DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, Location, Request, Supplierid, Searchid);
           // DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, Location, ResponseJSON, Supplierid, Searchid);
           // DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, Location, ResponseXML, Supplierid, Searchid);

            return ResponseXML;
        }
        public async Task<string> GetApiHttpResponseAsync(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Request, string ServiceUrl, string MethodName)
        {
            string ResponseXML = string.Empty;
            string ResponseJSON = string.Empty;

            try
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    byte[] data = Encoding.UTF8.GetBytes(Request);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceUrl);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    request.Headers.Add(@"SOAP:Action");
                    request.Headers.Add("Accept-Encoding", "gzip");
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();

                    using (WebResponse response =await request.GetResponseAsync())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            ResponseJSON = rd.ReadToEnd();
                            JObject _JObject = JObject.Parse(ResponseJSON);
                            ResponseXML = JsonConvert.DeserializeXmlNode(ResponseJSON).InnerXml;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    errorMessage = webEx.Message;
                    //DBCommon.Logger.dbLogg(Companyid, BookingRef, MethodName, "air_tbo-GetApiHttpResponse", Supplierid, Searchid, webEx.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, BookingRef, MethodName, "air_tbo-GetApiHttpResponse", Supplierid, Searchid, errorMessage);
            }

            string Location = "";
            string id = "0.";
            if (MethodName.ToUpper().Equals("SEARCH"))
            {
                Location = "AVAILABILITY";
                id = "1.";
            }
            else if (MethodName.ToUpper().Equals("FareRule".ToUpper()))
            {
                Location = "FARE";
                id = "2.";
            }
            else if (MethodName.ToUpper().Equals("FareQuote".ToUpper()))
            {
                Location = "FARE";
                id = "3.";
            }
            else if (MethodName.ToUpper().Equals("SSR".ToUpper()))
            {
                Location = "SSR";
                id = "4.";
            }
            else if (MethodName.ToUpper().Equals("Book".ToUpper()))
            {
                Location = "PNR";
                id = "5.";
            }
            else if (MethodName.ToUpper().Equals("Ticket".ToUpper()))
            {
                Location = "TKT";
                id = "6.";
            }
            else if (MethodName.ToUpper().Equals("GetBookingDetails".ToUpper()))
            {
                Location = "STORE";
                id = "7.";
            }

            //WriteFileToTxt(Request, id + MethodName + "_RQ_", false);
            //WriteFileToTxt(ResponseJSON, id + MethodName + "_RS_", false);
            //WriteFileToTxt(ResponseXML, id + MethodName + "_RS_", true);

            if (Location.Equals("TKT") || Location.Equals("PNR"))
            {
                //DBCommon.Logger.dbLoggAPI(Searchid, Companyid, BookingRef, "", Supplierid, "PNR", Request, ResponseXML, "");
            }

            //DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, Location, Request, Supplierid, Searchid);
            //DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, Location, ResponseJSON, Supplierid, Searchid);
            //DBCommon.Logger.WriteLogg(Companyid, BookingRef, MethodName, Location, ResponseXML, Supplierid, Searchid);

            return ResponseXML;
        }
        private string gettime()
        {
            return DateTime.Now.ToString("ddMMyyyyHHmm");
        }
        private static void WriteFileToTxt(string Response, string FileName, bool Isxml)
        {
            try
            {
                string logpath = @"E:\Cases\";
                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }

                string filepath = logpath + FileName + ".json";
                if (Isxml)
                {
                    filepath = logpath + FileName + ".xml";
                }

                FileInfo objFileInfo;
                FileStream fs;
                objFileInfo = new FileInfo(filepath);
                if (objFileInfo.Exists)
                {
                    System.IO.File.WriteAllText(filepath, "");
                    fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                }

                TextWriter m_streamWriter = new StreamWriter(fs);
                if (!string.IsNullOrEmpty(Response))
                {
                    m_streamWriter.WriteLine(Response);
                }
                m_streamWriter.Close();
                m_streamWriter.Dispose();
            }
            catch
            {

            }
        }
        //-----------------------------------------------------------------------------------------------------------
    }
}
