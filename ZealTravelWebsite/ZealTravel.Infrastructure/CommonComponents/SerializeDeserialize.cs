using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;

namespace ZealTravel.Infrastructure.CommonComponents
{
    public class SerializeDeserialize
    {
        private StringBuilder stringBuilderLog = new StringBuilder();

        private static string logServiceName = ConfigurationManager.AppSettings["LogServiceName"];

        private static string logModuleName = "CommonComponents";

        private static string logFileName = "SerializeDeserialize";

        public static string SerializeInXmlString(object transformObject, bool isRemoveNameSpaces)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xmlSerializer = new XmlSerializer(transformObject.GetType());
                xmlSerializer.Serialize(memoryStream, transformObject);
                memoryStream.Position = 0L;
                xmlDocument.Load(memoryStream);
                if (isRemoveNameSpaces)
                {
                    xmlDocument = RemoveXmlns(xmlDocument);
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in SerializeInXmlString" + ex.StackTrace, "");
            }

            return xmlDocument.OuterXml;
        }

        public static string SerializeInJsonString(object transformObject, bool isRemoveNameSpaces)
        {
            string result = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(transformObject, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include
                });
            }
            catch (Exception ex)
            {
               // Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in SerializeInJsonString" + ex.StackTrace, "");
            }

            return result;
        }

        public static string SerializeInJsonString(object transformObject, string rootObject)
        {
            string result = string.Empty;
            try
            {
                dynamic val = null;
                switch (rootObject)
                {
                    case "SearchRQ":
                        val = new
                        {
                            SearchRQ = transformObject
                        };
                        break;
                    case "BookingXML":
                        val = new
                        {
                            BookingXML = transformObject
                        };
                        break;
                    case "SearchResult":
                        val = new
                        {
                            SearchResult = transformObject
                        };
                        break;
                    case "Flight":
                        val = new
                        {
                            Flight = transformObject
                        };
                        break;
                    case "FlightRQ":
                        val = new
                        {
                            FlightRQ = transformObject
                        };
                        break;
                    case "FlightRS":
                        val = new
                        {
                            FlightRS = transformObject
                        };
                        break;
                    case "PaxTicketInfo":
                        val = new
                        {
                            PaxTicketInfo = transformObject
                        };
                        break;
                    case "AirportList":
                        val = new
                        {
                            AirportList = transformObject
                        };
                        break;
                    case "AirlineList":
                        val = new
                        {
                            AirlineList = transformObject
                        };
                        break;
                    case "UserRS":
                        val = new
                        {
                            UserRS = transformObject
                        };
                        break;
                    case "UserRQ":
                        val = new
                        {
                            UserRQ = transformObject
                        };
                        break;
                    case "BookingEnquiryRequest":
                        val = new
                        {
                            BookingEnquiryRequest = transformObject
                        };
                        break;
                    case "BookingEnquiryResponse":
                        val = new
                        {
                            BookingEnquiryResponse = transformObject
                        };
                        break;
                    case "BookingDetailRequest":
                        val = new
                        {
                            BookingDetailRequest = transformObject
                        };
                        break;
                    case "BookingDetailResponse":
                        val = new
                        {
                            BookingDetailResponse = transformObject
                        };
                        break;
                    case "CancellationRequest":
                        val = new
                        {
                            CancellationRequest = transformObject
                        };
                        break;
                    case "CountryDataList":
                        val = new
                        {
                            CountryDataList = transformObject
                        };
                        break;
                    case "Booking":
                        val = new
                        {
                            Booking = transformObject
                        };
                        break;
                    case "MailSend":
                        val = new
                        {
                            MailSend = transformObject
                        };
                        break;
                    case "LicenceRequest":
                        val = new
                        {
                            LicenceRequest = transformObject
                        };
                        break;
                    case "LicenceResponse":
                        val = new
                        {
                            LicenceResponse = transformObject
                        };
                        break;
                    case "EncryptionKeyResponse":
                        val = new
                        {
                            EncryptionKeyResponse = transformObject
                        };
                        break;
                    case "AvailabilityResponse":
                        val = new
                        {
                            AvailabilityResponse = transformObject
                        };
                        break;
                    case "CancellationPolicy":
                        val = new
                        {
                            CancellationPolicy = transformObject
                        };
                        break;
                    case "Hotel":
                        val = new
                        {
                            Hotel = transformObject
                        };
                        break;
                    case "Room":
                        val = new
                        {
                            Room = transformObject
                        };
                        break;
                    case "CountryResponse":
                        val = new
                        {
                            CountryResponse = transformObject
                        };
                        break;
                    case "CityResponse":
                        val = new
                        {
                            CityResponse = transformObject
                        };
                        break;
                    case "CityRequest":
                        val = new
                        {
                            CityRequest = transformObject
                        };
                        break;
                    case "Response":
                        val = new
                        {
                            Response = transformObject
                        };
                        break;
                    case "HotelFeatureResponse":
                        val = new
                        {
                            HotelFeatureResponse = transformObject
                        };
                        break;
                    case "CancellationResponse":
                        val = new
                        {
                            CancellationResponse = transformObject
                        };
                        break;
                    case "Request":
                        val = new
                        {
                            Request = transformObject
                        };
                        break;
                    case "IssueVoucher":
                        val = new
                        {
                            IssueVoucher = transformObject
                        };
                        break;
                    case "CommonRQ":
                        val = new
                        {
                            CommonRQ = transformObject
                        };
                        break;
                    case "CommonRS":
                        val = new
                        {
                            CommonRS = transformObject
                        };
                        break;
                    case "AgencyBookingSettingsRQ":
                        val = new
                        {
                            AgencyBookingSettingsRQ = transformObject
                        };
                        break;
                    case "AgencyBookingSettingsRS":
                        val = new
                        {
                            AgencyBookingSettingsRS = transformObject
                        };
                        break;
                    case "CartBookingObjectList":
                        val = new
                        {
                            CartBookingObjectList = transformObject
                        };
                        break;
                    case "CartBookingObject":
                        val = new
                        {
                            CartBookingObject = transformObject
                        };
                        break;
                    case "CancelBookingRefund":
                        val = new
                        {
                            CancelBookingRefund = transformObject
                        };
                        break;
                    case "Cruise":
                        val = new
                        {
                            Cruise = transformObject
                        };
                        break;
                    case "MenuList":
                        val = new
                        {
                            MenuList = transformObject
                        };
                        break;
                    case "IssueTicketRequest":
                        val = new
                        {
                            IssueTicketRequest = transformObject
                        };
                        break;
                    case "IssueTicketResponse":
                        val = new
                        {
                            IssueTicketResponse = transformObject
                        };
                        break;
                    case "EnquiryRequest":
                        val = new
                        {
                            EnquiryRequest = transformObject
                        };
                        break;
                    case "EnquiryResponse":
                        val = new
                        {
                            EnquiryResponse = transformObject
                        };
                        break;
                    case "SeatMapRS":
                        val = new
                        {
                            SeatMapRS = transformObject
                        };
                        break;
                    case "HotelRequest":
                        val = new
                        {
                            HotelRequest = transformObject
                        };
                        break;
                    case "SearchRequest":
                        val = new
                        {
                            SearchRequest = transformObject
                        };
                        break;
                    case "Insurance":
                        val = new
                        {
                            Insurance = transformObject
                        };
                        break;
                    case "PassengerDetails":
                        val = new
                        {
                            PassengerDetails = transformObject
                        };
                        break;
                    case "FaresRulesReply":
                        val = new
                        {
                            FaresRulesReply = transformObject
                        };
                        break;
                    case "GetTravelCategoryResponse":
                        val = new
                        {
                            GetTravelCategoryResponse = transformObject
                        };
                        break;
                    case "GetInsurancePlanResponse":
                        val = new
                        {
                            GetInsurancePlanResponse = transformObject
                        };
                        break;
                    case "GetPlanRidersResponse":
                        val = new
                        {
                            GetPlanRidersResponse = transformObject
                        };
                        break;
                    case "GetPlanDocumentsResponse":
                        val = new
                        {
                            GetPlanDocumentsResponse = transformObject
                        };
                        break;
                }

                result = JsonConvert.SerializeObject(val, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include
                });
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in SerializeInJsonString" + ex.StackTrace, "");
            }

            return result;
        }

        public static string SerializeInJsonString(object transformObject)
        {
            string result = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(transformObject, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in SerializeInJsonString" + ex.StackTrace, "");
            }

            return result;
        }

        public static object DeserializeFromJsonString(string jsonString)
        {
            object result = null;
            try
            {
                result = JsonConvert.DeserializeObject(jsonString, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in DeserializeFromJsonString" + ex.StackTrace, "");
            }

            return result;
        }

        public static object DeserializeFromXmlString(string xml, Type tp)
        {
            object result = null;
            try
            {
                if (xml.Contains("UTF-16"))
                {
                    xml = xml.Replace("UTF-16", "UTF-8");
                }

                xml = xml.Replace("<ChildAge></ChildAge>", "").Replace("<ChildAge/>", "").Replace("<ChildAge />", "");
                Stream stream = StringToStream(xml);
                XmlSerializer xmlSerializer = new XmlSerializer(tp);
                result = xmlSerializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, string.Concat("Exception in DeserializeFromXmlString", ex.InnerException, ex.StackTrace), "");
            }

            return result;
        }

        public static object Deserialize(XmlDocument xmlDocument, Type tp)
        {
            object result = null;
            try
            {
                Stream stream = StringToStream(xmlDocument.OuterXml);
                XmlSerializer xmlSerializer = new XmlSerializer(tp);
                result = xmlSerializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in Deserialize" + ex.StackTrace, "");
            }

            return result;
        }

        public HttpResponseMessage GetResponse(object value, bool requestInJson, string rootObject)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            string text = "";
            try
            {
                if (requestInJson)
                {
                    dynamic val = null;
                    switch (rootObject)
                    {
                        case "SearchResult":
                            val = new
                            {
                                SearchResult = value
                            };
                            break;
                        case "BookingXML":
                            val = new
                            {
                                BookingXML = value
                            };
                            break;
                        case "BookingResponse":
                            val = new
                            {
                                BookingResponse = value
                            };
                            break;
                        case "CancellationResponse":
                            val = new
                            {
                                CancellationResponse = value
                            };
                            break;
                        case "AirportList":
                            val = new
                            {
                                AirportList = value
                            };
                            break;
                        case "AirlineList":
                            val = new
                            {
                                AirlineList = value
                            };
                            break;
                        case "SSRList":
                            val = new
                            {
                                SSRList = value
                            };
                            break;
                        case "FlightRS":
                            val = new
                            {
                                FlightRS = value
                            };
                            break;
                        case "UserRS":
                            val = new
                            {
                                UserRS = value
                            };
                            break;
                        case "BookingEnquiryResponse":
                            val = new
                            {
                                BookingEnquiryResponse = value
                            };
                            break;
                        case "BookingDetailResponse":
                            val = new
                            {
                                BookingDetailResponse = value
                            };
                            break;
                        case "CountryDataList":
                            val = new
                            {
                                CountryDataList = value
                            };
                            break;
                        case "MailSend":
                            val = new
                            {
                                MailSend = value
                            };
                            break;
                        case "LicenceResponse":
                            val = new
                            {
                                LicenceResponse = value
                            };
                            break;
                        case "EncryptionKeyResponse":
                            val = new
                            {
                                EncryptionKeyResponse = value
                            };
                            break;
                        case "HotelFeatureResponse":
                            val = new
                            {
                                HotelFeatureResponse = value
                            };
                            break;
                        case "CountryResponse":
                            val = new
                            {
                                CountryResponse = value
                            };
                            break;
                        case "CityResponse":
                            val = new
                            {
                                CityResponse = value
                            };
                            break;
                        case "Response":
                            val = new
                            {
                                Response = value
                            };
                            break;
                        case "BookingRequest":
                            val = new
                            {
                                BookingRequest = value
                            };
                            break;
                        case "ProgramDataList":
                            val = new
                            {
                                ProgramDataList = value
                            };
                            break;
                        case "FeaturedDetails":
                            val = new
                            {
                                FeaturedDetails = value
                            };
                            break;
                        case "CommonRS":
                            val = new
                            {
                                CommonRS = value
                            };
                            break;
                        case "CommonRQ":
                            val = new
                            {
                                CommonRQ = value
                            };
                            break;
                        case "SeatMapRS":
                            val = new
                            {
                                SeatMapRS = value
                            };
                            break;
                        case "AgencyBookingSettingsRQ":
                            val = new
                            {
                                AgencyBookingSettingsRQ = value
                            };
                            break;
                        case "AgencyBookingSettingsRS":
                            val = new
                            {
                                AgencyBookingSettingsRS = value
                            };
                            break;
                        case "CartBookingObjectList":
                            val = new
                            {
                                CartBookingObjectList = value
                            };
                            break;
                        case "MenuItem":
                            val = value;
                            break;
                        case "LoyaltyLedger":
                            val = value;
                            break;
                        case "CompanyPasswordSettig":
                            val = value;
                            break;
                        case "IssueTicketRequest":
                            val = new
                            {
                                IssueTicketRequest = value
                            };
                            break;
                        case "IssueTicketResponse":
                            val = new
                            {
                                IssueTicketResponse = value
                            };
                            break;
                        case "EnquiryRequest":
                            val = new
                            {
                                EnquiryRequest = value
                            };
                            break;
                        case "EnquiryResponse":
                            val = new
                            {
                                EnquiryResponse = value
                            };
                            break;
                        case "StateDataList":
                            val = new
                            {
                                StateDataList = value
                            };
                            break;
                        case "StaticDataContent":
                            val = new
                            {
                                StaticDataContent = value
                            };
                            break;
                        case "CompanySystemSetting":
                            val = new
                            {
                                CompanySystemSetting = value
                            };
                            break;
                        case "TripAdvisorResponse":
                            val = new
                            {
                                TripAdvisorResponse = value
                            };
                            break;
                        case "DistanceResponse":
                            val = new
                            {
                                DistanceResponse = value
                            };
                            break;
                        case "SearchBooking":
                            val = new
                            {
                                SearchBooking = value
                            };
                            break;
                        case "PassportDetail":
                            val = new
                            {
                                PassportDetail = value
                            };
                            break;
                        case "Admin":
                            val = new
                            {
                                Admin = value
                            };
                            break;
                        case "DestinationResponse":
                            val = new
                            {
                                DestinationResponse = value
                            };
                            break;
                        case "RoomFeatureResponse":
                            val = new
                            {
                                RoomFeatureResponse = value
                            };
                            break;
                        case "TripPurposeListResponse":
                            val = new
                            {
                                TripPurposeListResponse = value
                            };
                            break;
                        case "ZoneListResponse":
                            val = new
                            {
                                ZoneListResponse = value
                            };
                            break;
                        case "CountryListResponse":
                            val = new
                            {
                                CountryListResponse = value
                            };
                            break;
                        case "StatesListResponse":
                            val = new
                            {
                                StatesListResponse = value
                            };
                            break;
                        case "GetBookingStatusResponse":
                            val = new
                            {
                                GetBookingStatusResponse = value
                            };
                            break;
                        case "GetTravelCategoryResponse":
                            val = new
                            {
                                GetTravelCategoryResponse = value
                            };
                            break;
                        case "GetInsurancePlanResponse":
                            val = new
                            {
                                GetInsurancePlanResponse = value
                            };
                            break;
                        case "GetPlanRidersResponse":
                            val = new
                            {
                                GetPlanRidersResponse = value
                            };
                            break;
                        case "GetPlanDocumentsResponse":
                            val = new
                            {
                                GetPlanDocumentsResponse = value
                            };
                            break;
                    }

                    text = JsonConvert.SerializeObject(val, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include
                    });
                    httpResponseMessage.Content = new StringContent(text, Encoding.UTF8, "application/json");
                }
                else
                {
                    text = SerializeInXmlString(value, isRemoveNameSpaces: true);
                    httpResponseMessage.Content = new StringContent(text, Encoding.UTF8, "application/xml");
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in GetResponse" + ex.StackTrace, "");
            }

            return httpResponseMessage;
        }

        public HttpResponseMessage GetResponseAsHtml(string value)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                httpResponseMessage.Content = new StringContent(value, Encoding.UTF8, "application/html");
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in GetResponse" + ex.StackTrace, "");
            }

            return httpResponseMessage;
        }

        public static T DeserializeObject<T>(string request, bool requestInJson)
        {
            object obj = ((!requestInJson) ? DeserializeFromXmlString(request, typeof(T)) : DeserializeFromXmlString(JsonConvert.DeserializeXmlNode(request, "").InnerXml, typeof(T)));
            return (T)obj;
        }

        public static T DeserializeObjectNew<T>(string request, bool requestInJson)
        {
            if (requestInJson)
            {
                JObject jObject = JObject.Parse(request);
                return jObject[jObject.First.Path].ToObject<T>();
            }

            object obj = DeserializeFromXmlString(request, typeof(T));
            return (T)obj;
        }

        public static T Deserialize<T>(string xml)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using TextReader textReader = new StringReader(xml);
                return (T)xmlSerializer.Deserialize(textReader);
            }
            catch
            {
                throw;
            }
        }

        public static T DeserializeJSON<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                throw;
            }
        }

        public static Stream StringToStream(string str)
        {
            MemoryStream memoryStream = null;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                memoryStream = new MemoryStream(bytes);
            }
            catch (Exception ex)
            {
                //Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, "Exception in StringToStream" + ex.StackTrace, "");
            }
            finally
            {
                memoryStream.Position = 0L;
            }

            return memoryStream;
        }

        private static XmlDocument RemoveXmlns(XmlDocument doc)
        {
            XDocument xDocument;
            using (XmlNodeReader reader = new XmlNodeReader(doc))
            {
                xDocument = XDocument.Load(reader);
            }

            (from x in xDocument.Root.Descendants().Attributes()
             where x.IsNamespaceDeclaration
             select x).Remove();
            (from x in xDocument.Root.Attributes()
             where x.IsNamespaceDeclaration
             select x).Remove();
            foreach (XElement item in xDocument.Descendants())
            {
                item.Name = item.Name.LocalName;
            }

            XmlDocument xmlDocument = new XmlDocument();
            using (XmlReader reader2 = xDocument.CreateReader())
            {
                xmlDocument.Load(reader2);
            }

            return xmlDocument;
        }

        public static string SerializeObjectXmlThenJson<T>(T obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                TextWriter textWriter = new clsTextWriter(stringBuilder);
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(textWriter, obj, xmlSerializerNamespaces);
                textWriter.Close();
                textWriter.Dispose();
            }
            catch
            {
                throw;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringBuilder.ToString());
            xmlDocument.RemoveChild(xmlDocument.FirstChild);
            return JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented, omitRootObject: false);
        }

        public static string SerializeObjectXmlThenJson<T>(T obj, bool includenull)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                TextWriter textWriter = new clsTextWriter(stringBuilder);
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(textWriter, obj, xmlSerializerNamespaces);
                textWriter.Close();
                textWriter.Dispose();
            }
            catch
            {
                throw;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringBuilder.ToString());
            xmlDocument.RemoveChild(xmlDocument.FirstChild);
            return JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented, includenull);
        }

        public static string ConvertJsonToXML(string json)
        {
            return JsonConvert.DeserializeXmlNode(json).InnerXml.ToString();
        }
    }
}
