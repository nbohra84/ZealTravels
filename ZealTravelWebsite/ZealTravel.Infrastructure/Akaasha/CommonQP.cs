using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RtfPipe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Models;




namespace ZealTravel.Infrastructure.Akaasha
{

    class CommonQP
    {
        ICredential _credential;

        public CommonQP(ICredential credential)
        {
            _credential = credential;
        }

        private static string GetBaseUrl()
        {
            var baseUrl = ConfigurationHelper.GetSetting("AkasaAirline:BaseURL");
            baseUrl = string.IsNullOrEmpty(baseUrl) ? "https://t-extprt-reyalrb.qp.akasaair.com" : baseUrl;
            return baseUrl;
        }
        
        public static async Task<string> GetResponseQpAsync(string SearchID, string jsonRequestData, string Method, string Token)
        {
            return await GetResponseQpAsync(SearchID, jsonRequestData,Method,Token,"","");
        }
        public static async Task<string> GetResponseQpAsync(string SearchID, string jsonRequestData, string Method, string Token, string _HttpVerb)
        {
            return await GetResponseQpAsync(SearchID, jsonRequestData, Method, Token, "", _HttpVerb);
        }

        public static async Task<string> GetResponseQpAsync(string SearchID, string jsonRequestData, string Method, string Token,string _Key,string _HttpVerb)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "";

                    if (Method.Equals("token", StringComparison.OrdinalIgnoreCase))
                    {
                        apiUrl = GetBaseUrl() + "/api/nsk/v2/token";
                    }
                    else if (Method.Equals("Search", StringComparison.OrdinalIgnoreCase))
                    {
                        //2. Search
                        apiUrl = GetBaseUrl() +"/api/nsk/v4/availability/search/simple";
                        //apiUrl =GetBaseUrl() + "/api/v2/graph/Estimate";  // old Url
                    }
                    else if (Method.Equals("TripSell", StringComparison.OrdinalIgnoreCase))
                    {
                        //3. TripSell
                        apiUrl = GetBaseUrl() + "/api/nsk/v4/trip/sell";
                        // apiUrl =GetBaseUrl() + "/api/nsk/v4/trip/sell";
                    }
                    else if (Method.Equals("Quote", StringComparison.OrdinalIgnoreCase))
                    {
                        //4. Quote
                        apiUrl = GetBaseUrl() + "/api/nsk/v2/bookings/quote";
                    }
                    //5.SSRAVAILABILITY
                    //6. Seatmaps
                    else if (Method.Equals("Contacts", StringComparison.OrdinalIgnoreCase))
                    {
                        //7. Contacts
                        apiUrl = GetBaseUrl() + "/api/nsk/v1/booking/contacts";
                    }

                    else if (Method.Equals("Passengers", StringComparison.OrdinalIgnoreCase))
                    {
                        //8. Passengers
                        apiUrl = GetBaseUrl() + "/api/nsk/v3/booking/passengers/"+ _Key;
                    }
                    else if (Method.Equals("Payments", StringComparison.OrdinalIgnoreCase))
                    {
                        //10.Payments
                        apiUrl = GetBaseUrl() + "/api/nsk/v2/booking/payments";
                    }
                    else if (Method.Equals("Bookings", StringComparison.OrdinalIgnoreCase))
                    {
                        //11.Bookings
                        apiUrl = GetBaseUrl() + "/api/nsk/v3/booking";
                    }
                    else if (Method.Equals("GetBookingState", StringComparison.OrdinalIgnoreCase))
                    {
                        //12. GetBookingState
                        apiUrl = GetBaseUrl() + "/api/nsk/v1/booking"; // + Token;  // + "&param2=null";
                       // apiUrl = GetBaseUrl() + "/api/nsk/v2/booking" + Token;  // + "&param2=null";
                    }

                    string accessToken = Token;

                    // Create a JSON string or object to send in the request body
                    //string jsonData = "{\"key1\": \"value1\", \"key2\": \"value2\"}";

                    // Set the content type and data for the request
                    
                    HttpContent content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");

                    // Set the Authorization header
                    HttpResponseMessage response;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    if (_HttpVerb.Equals("PUT",StringComparison.OrdinalIgnoreCase))
                    {
                        response = await client.PutAsync(apiUrl, content);
                    }
                    else if (_HttpVerb.Equals("GET", StringComparison.OrdinalIgnoreCase))
                    {
                        //content = new StringContent("");
                        // Set the Content-Type header if necessary
                        //content.Headers.Clear();
                        //content.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                        // Send a POST request with an empty body
                        //response = await client.PostAsync(apiUrl, content);
                        response = await client.GetAsync(apiUrl);
                    }
                    else {
                        response = await client.PostAsync(apiUrl, content);
                    }
                    
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the response as needed
                        responseJSON = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseJSON);
                    }
                    else
                    {
                        // Handle error cases
                        Console.WriteLine("API request failed with status code: " + response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQpAsync", jsonRequestData, responseXML, SearchID, Method + "-" + "" + "-" + ex.Message);
            }
            //DBCommon.Logger.dbLogg("", 0, "GetResponseQpAsync:"+ Method , Token, jsonRequestData, SearchID, responseJSON);
            return responseJSON;
        }

        #region OldCalls
        /*public static async Task<string> GetResponseQpAsyncContact(string SearchID, string jsonRequestData, string Method, string _Token)
        {
            
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                string apiUrl = "";


                //P7025891 //test
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                apiUrl = GetBaseUrl() + "/api/nsk/v1/booking/contacts";

                string accessToken = _Token;


                //string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequestData);

                //string jsonString = "{{\"credentials\": {{\"username\": \"QPMAA8752B_01\", \"password\": \"Akasa@2023\", \"domain\": \"EXT\"}}}}";


                byte[] data = Encoding.UTF8.GetBytes(jsonRequestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.Headers.Add("Authorization", "Bearer " + accessToken);

                //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
                //NetworkCredential cred = new NetworkCredential("Universal API/uAPI6776316127-4450f3c2", "9f&QrX2!G_"); //test
                //NetworkCredential cred = new NetworkCredential("Universal API/uAPI4997970939-5270ffc4", "n/8JX4i}5m"); //test
                //request.Credentials = cred;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = rd.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQp-Token", "", responseXML, SearchID, ex.Message);
            }

            return responseJSON;
        }//====== end
      

        public static async Task<string> GetResponseQpV1Async(string SearchID, string NetworkUserName, string NetworkPassword, string jsonRequestData,string _Token)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                string apiUrl = "";


                //P7025891 //test
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                apiUrl = GetBaseUrl() + "/api/nsk/v2/bookings/quote"; 

                string accessToken = _Token;


                //string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequestData);

                //string jsonString = "{{\"credentials\": {{\"username\": \"QPMAA8752B_01\", \"password\": \"Akasa@2023\", \"domain\": \"EXT\"}}}}";


                byte[] data = Encoding.UTF8.GetBytes(jsonRequestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.Headers.Add("Authorization", "Bearer " + accessToken);

                //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                //NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
                //NetworkCredential cred = new NetworkCredential("Universal API/uAPI6776316127-4450f3c2", "9f&QrX2!G_"); //test
                //NetworkCredential cred = new NetworkCredential("Universal API/uAPI4997970939-5270ffc4", "n/8JX4i}5m"); //test
                //request.Credentials = cred;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = rd.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQp-Token", "", responseXML, SearchID, ex.Message);
            }

            return responseJSON;
        }//====== end
        */
        #endregion




        public static async Task<string> GetResponseQpFareRuleAsync(string SearchID, string jsonRequestData, string Method, string Token,string _JournyKey,string _SegmantKey)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {


                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "";


                    if (Method.Equals("token", StringComparison.OrdinalIgnoreCase))
                    {
                        apiUrl = GetBaseUrl() + "/api/nsk/v2/token";
                        
                    }
                    else if (Method.Equals("Search", StringComparison.OrdinalIgnoreCase))
                    {
                        apiUrl = GetBaseUrl() + "/api/nsk/v4/availability/search/simple";
                        //apiUrl =GetBaseUrl() + "/api/v2/graph/Estimate";  // old Url

                    }
                    else if (Method.Equals("Quote", StringComparison.OrdinalIgnoreCase))
                    {
                        apiUrl = GetBaseUrl() + "/api/nsk/v1/fareRules/category50/journeys/" + _JournyKey + "/segments/" + _SegmantKey;
                        // apiUrl =GetBaseUrl() + "/api/nsk/v4/trip/sell";
                    }
                    else if (Method.Equals("FareRule", StringComparison.OrdinalIgnoreCase))
                    {
                        // Fare rule API as below.
                        apiUrl = GetBaseUrl() + "/api/nsk/v1/fareRules/category50/journeys/" +_JournyKey + "/segments/"+_SegmantKey;
                        // apiUrl =GetBaseUrl() + "/api/nsk/v4/trip/sell";
                    }




                    string accessToken = Token;

                    // Create a JSON string or object to send in the request body
                    //string jsonData = "{\"key1\": \"value1\", \"key2\": \"value2\"}";

                    // Set the content type and data for the request

                    HttpContent content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");

                    // Set the Authorization header

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the response as needed
                        responseJSON = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseJSON);
                    }
                    else
                    {
                        // Handle error cases
                        Console.WriteLine("API request failed with status code: " + response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQpAsync", jsonRequestData, responseXML, SearchID, Method + "-" + "" + "-" + ex.Message);
            }
            return responseJSON;
        }

        #region OldToken
        /* public static async Task<string> GetResponseQpTokenAsync_1(string SearchID, string NetworkUserName, string NetworkPassword, string Domain)
         {
             string responseXML = string.Empty;
             string responseJSON = string.Empty;

             try
             {
                 string apiUrl = "";



                 //P7025891 //test
                 ServicePointManager.Expect100Continue = true;
                 ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                 ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                 apiUrl = GetBaseUrl() + @"/api/nsk/v2/token";  
                 //apiUrl = @"https://tbnk-reyalrb.qp.akasaair.com/api/nsk/v2/token";


                 //NetworkPassword = "Demo@123"; // this test server

                 var credentials = new
                 {
                     username = NetworkUserName,
                     password = NetworkPassword,
                     domain = Domain
                 };




                 string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { credentials });

                 //string jsonString = "{{\"credentials\": {{\"username\": \"QPMAA8752B_01\", \"password\": \"Akasa@2023\", \"domain\": \"EXT\"}}}}";


                 byte[] data = Encoding.UTF8.GetBytes(jsonString);

                 HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                 request.Method = "POST";
                 request.ContentType = "text/xml";
                 request.Accept = "text/xml";
                 request.Headers.Add(@"SOAP:Action");
                 request.Headers.Add("Accept-Encoding", "gzip");
                 request.KeepAlive = true;

                 request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                 NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
                 //NetworkCredential cred = new NetworkCredential("Universal API/uAPI6776316127-4450f3c2", "9f&QrX2!G_"); //test
                 //NetworkCredential cred = new NetworkCredential("Universal API/uAPI4997970939-5270ffc4", "n/8JX4i}5m"); //test
                 request.Credentials = cred;

                 Stream dataStream = request.GetRequestStream();
                 dataStream.Write(data, 0, data.Length);
                 dataStream.Close();

                 using (WebResponse response =await request.GetResponseAsync())
                 {
                     using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                     {
                         responseJSON = rd.ReadToEnd();
                     }
                 }
             }
             catch (WebException ex)
             {
                 DBCommon.Logger.dbLogg("", 0, "GetResponseQp-Token", "", responseXML, SearchID, ex.Message);
             }

             return responseJSON;
         }//====== end
         */


        //=========== get Booing Status
        /*public static async Task<string> GetResponseBookingStateAsync(string SearchID, string _Token)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                string apiUrl = "";


                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                apiUrl = GetBaseUrl() + "/api/nsk/v1/booking"+_Token;

                string accessToken = _Token;


                var jsonObject = new
                {
                    data = (object)null
                };

                string jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);


                byte[] data = Encoding.UTF8.GetBytes(jsonRequestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.Headers.Add("Authorization", "Bearer " + accessToken);



                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = rd.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQp-Token", "", responseXML, SearchID, ex.Message);
            }

            return responseJSON;

        }
        */
        //===== end booking status

        //==== get token using HTTP content
        /* public static async Task<string> GetResponseQpTokenAsync(string SearchID, string NetworkUserName, string NetworkPassword, string Domain)
         {

             string responseJSON = string.Empty;
             string jsonRequestData = string.Empty;

             //if (__Token != "")
             //{
             //    //return "{{\"response\": {{\"token\":\"" + __Token + "\",\"ideal\":\"" + 10 + "\"}}}}";
             //    return  "{\"data\":{\"" + __Token +  ",\"idleTimeoutInMinutes\":15}}";
             //}

             try
             {
                 using (HttpClient client = new HttpClient())
                 {
                     string apiUrl = "";

                     apiUrl = GetBaseUrl() + @"/api/nsk/v2/token";
                     NetworkPassword = "Test@2023";   // for test server only
                     var credentials = new
                     {
                         username = NetworkUserName,
                         password = NetworkPassword,
                         domain = Domain
                     };
                    jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(new { credentials });

                     HttpContent content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");

                     // Set the Authorization header

                     //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                     HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                     if (response.IsSuccessStatusCode)
                     {
                         // Handle the response as needed
                         responseJSON = await response.Content.ReadAsStringAsync();
                         Console.WriteLine("Response: " + responseJSON);
                     }
                     else
                     {
                         // Handle error cases
                         Console.WriteLine("API request failed with status code: " + response.StatusCode);
                     }
                 }
             }
             catch (HttpRequestException ex)
             {
                 DBCommon.Logger.dbLogg("", 0, "GetResponseQpAsync", jsonRequestData, responseJSON, SearchID, ex.Message);
             }
             //__Token = responseJSON;
             return responseJSON;
         }
         */
        //======= end HTTP token taken
        #endregion


        //======= GetToken v0
        public static async Task<string> GetResponseQpTokenAsync(string SearchID, string NetworkUserName, string NetworkPassword, string Domain)
        {
            //======== Test serer only
           // return await GetResponseQpTokenTestSvrOnlyAsync(SearchID, NetworkUserName, NetworkPassword, Domain);
            //===== End

            string responseXML = string.Empty;
            string responseJSON = string.Empty;
            string jsonRequestData = "";

            try
            {
               
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                string apiUrl = GetBaseUrl() + "/api/nsk/v2/token";
                
                var credentials = new
                {
                    username = NetworkUserName,
                    password = NetworkPassword,
                    domain = Domain
                };

                jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(new { credentials });

                byte[] data = Encoding.UTF8.GetBytes(jsonRequestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
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

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = rd.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQp-Token", "", responseXML, SearchID, ex.Message);
            }
            //DBCommon.Logger.dbLogg("", 0, "GetResponseQpAsync:Token" , "", jsonRequestData, SearchID, responseJSON);
            return responseJSON;
        }
        //====== end
        //======== token for Test server only
        public static async Task<string> GetResponseQpTokenTestSvrOnlyAsync(string SearchID, string NetworkUserName, string NetworkPassword, string Domain)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;
            string jsonRequestData = "";
            try
            {
                // SSL/TLS secure channel
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "";

                    //apiUrl = GetBaseUrl() + "/api/nsk/v2/token";



                    //string accessToken = Token;

                    // Create a JSON string or object to send in the request body
                    //string jsonData = "{\"key1\": \"value1\", \"key2\": \"value2\"}";

                    apiUrl = GetBaseUrl() + "/api/nsk/v2/token";
                    NetworkUserName = "QPMAA8752B_01";
                    NetworkPassword = "Dec#2024";        // this is for only test
                    var credentials = new
                    {
                        username = NetworkUserName,
                        password = NetworkPassword,
                        domain = Domain
                    };

                    jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(new { credentials });

                    HttpContent content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");

                    // Set the Authorization header

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the response as needed
                        responseJSON = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseJSON);
                    }
                    else
                    {
                        // Handle error cases
                        Console.WriteLine("API request failed with status code: " + response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseQpAsync", jsonRequestData, responseXML, SearchID, "Token4TestSvr" + "-" + "" + "-" + ex.Message);
            }
            return responseJSON;
        }

        //====== end token Test server

        //================ start RetrievePNR
        public static string RetrievePNR(string SearchID, string CompanyID, int BookingRef, string Response, out string QpPNR, out string AirReservationLocatorCode)
        {
            string AirlinePNR = "";
            QpPNR = "";
            AirReservationLocatorCode = "";
            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(Response);
                XmlElement root = xmlflt.DocumentElement;
                if (root.HasChildNodes)
                {
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        string s12 = root.ChildNodes[i].InnerXml;

                        XmlDocument xmlflt1 = new XmlDocument();
                        xmlflt1.LoadXml(s12);
                        XmlElement root1 = xmlflt1.DocumentElement;
                        if (root1.HasChildNodes)
                        {
                            for (int j = 0; j < root1.ChildNodes.Count; j++)
                            {
                                string s123 = root1.ChildNodes[j].OuterXml;

                                XmlDocument xmlflt2 = new XmlDocument();
                                xmlflt2.LoadXml(s123);
                                XmlElement root2 = xmlflt2.DocumentElement;
                                if (root2.HasChildNodes)
                                {
                                    for (int k = 0; k < root2.ChildNodes.Count; k++)
                                    {
                                        string Nodes = root2.ChildNodes[k].OuterXml;
                                        if (Nodes.IndexOf("data:ocators") != -1)
                                        {
                                            DataSet dsAvailability = new DataSet();
                                            dsAvailability.ReadXml(new System.IO.StringReader(Nodes));

                                            if (dsAvailability != null && dsAvailability.Tables["AirReservation"] != null)
                                            {
                                                AirReservationLocatorCode = dsAvailability.Tables["AirReservation"].Rows[0]["LocatorCode"].ToString();
                                            }

                                            if (dsAvailability != null && dsAvailability.Tables.Count > 1)
                                            {
                                                if (dsAvailability.Tables["SupplierLocator"] != null)
                                                {
                                                    AirlinePNR = dsAvailability.Tables["SupplierLocator"].Rows[0]["SupplierLocatorCode"].ToString();
                                                }
                                            }
                                        }

                                        if (Nodes.IndexOf("universal:ProviderReservationInfo") != -1)
                                        {
                                            DataSet dsAvailability = new DataSet();
                                            dsAvailability.ReadXml(new System.IO.StringReader(Nodes));

                                            if (dsAvailability.Tables["ProviderReservationInfo"] != null)
                                            {
                                                QpPNR = dsAvailability.Tables["ProviderReservationInfo"].Rows[0]["LocatorCode"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "RetrievePNR", "air_Qp", Response, SearchID, ex.Message);
            }
            return AirlinePNR;
        }


        //============== end RetrievePNR

        public static string ConvertRtfToPlainText(string rtfText)
        {
            
            if (string.IsNullOrEmpty(rtfText))
                return string.Empty;

            // RtfPipe extracts plain text from RTF content
            return Rtf.ToHtml(rtfText);
            //return System.Text.RegularExpressions.Regex.Replace(result, "<.*?>", string.Empty); // Strip HTML tags
        }

        /*public static string ConvertRtfToPlainTextRgx(string rtfText)
        {
            string pattern = @"\\[a-z]+\d*[-\s]*\d*|[{}]|\\\w+";
            //string pattern = @"\\[a-z]+\d*|[{}]|\\[a-z]+\s*-\s*";
            return System.Text.RegularExpressions.Regex.Replace(rtfText, pattern, "");

            //string pattern = @"\\[^\\]+";
            //return System.Text.RegularExpressions.Regex.Replace(rtfContent, pattern, "");
        }*/

        public async Task<string> GetTokenAsync(string SupplierID, string SearchID)
        {
            string Response = "";


            string _LoginID = "QPMAA8752B_01";
            string _Password = "Dec#2024";
            var supplierID = string.IsNullOrEmpty(SupplierID) ? "QPMAA8752B" : SupplierID;

            var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDDetailLccAirline>("LCC", supplierID);
            if (dtCred != null && dtCred.Count > 0)
            {
                _LoginID = dtCred.FirstOrDefault()?.LoginID;//dr["LoginID"].ToString();
                _Password = dtCred.FirstOrDefault()?.Pwd;//dr["Pwd"].ToString();
            }


            var _TokenObj = await CommonQP.GetResponseQpTokenAsync(SearchID, _LoginID, _Password, "EXT");

            string xmlString = JsonConvert.DeserializeXmlNode(_TokenObj, "token").OuterXml;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            XmlNode root = xmlDoc.DocumentElement;
            Response = root.ChildNodes[0]["token"].InnerText;

            return Response;
        }
    }
}
