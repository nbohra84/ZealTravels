using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;


/// <summary>
/// Summary description for GetAirCreatePnrRequest
/// </summary>
/// 
namespace ZealTravel.Infrastructure.Akaasha
{
    class GetAirCreatePnrRequest
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        private string SearchID;
        private string SupplierID;
        private string CompanyID;
        private Int32 BookingRef;
        public GetAirCreatePnrRequest(string SearchID,string SupplierID, string CompanyID, int BookingRef)
        {
            //this.NetworkUserName = NetworkUserName;
            //this.NetworkPassword = NetworkPassword;
            //this.TargetBranch = TargetBranch;
            //this.UserName = UserName;
            //this.Password = Password;

            this.SearchID = SearchID;
            this.SupplierID = SupplierID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //========= start trip sell request
        public string GetTripSellRequest(string CompanyID, DataTable dtSelect, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string Request = "";

            try
            {
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }

                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string StateName = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");


                string[] splitEmail = Email.Split('@');
                string kkEmail = splitEmail[0].ToString().Trim() + "//" + splitEmail[1].ToString().Trim();
                string SegmentRef = dtSelect.Rows[0]["SegmentRef"].ToString();
                string CarrierCode = dtSelect.Rows[0]["CarrierCode"].ToString();
                string Sector = dtSelect.Rows[0]["Sector"].ToString();
                string FltType = dtSelect.Rows[0]["FltType"].ToString();

                string JourneyKey = dtSelect.Rows[0]["JourneySellKey"].ToString();
                string FareAvailabilityKey = dtSelect.Rows[0]["BookingFareID"].ToString();

                string Idx = dtSelect.Rows[0]["API_AirlineID"].ToString(); 
                string[] splitIdx = Idx.Split('_');
                string Identifier = splitIdx[0];
                

                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());



                ArrayList ArayAdtFareInfo = new ArrayList();
                ArrayList ArayChdFareInfo = new ArrayList();
                ArrayList ArayInfFareInfo = new ArrayList();

                foreach (DataRow dr in dtSelect.Rows)
                {
                    ArayAdtFareInfo.Add(dr["AdtFareInfoRef"].ToString());
                    ArayChdFareInfo.Add(dr["ChdFareInfoRef"].ToString());
                    ArayInfFareInfo.Add(dr["InfFareInfoRef"].ToString());
                }

                ArayAdtFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayAdtFareInfo);
                ArayChdFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayChdFareInfo);
                ArayInfFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayInfFareInfo);

                ArrayList _types = new ArrayList();
                _types.Add(new { type = "ADT", count = Adt });
                if (Chd > 0)
                {
                    _types.Add(new { type = "CHD", count = Chd });
                }

                var tripSell__ = new
                {
                    keys = new[]
                {
                new
                {
                    journeyKey = JourneyKey,
                    fareAvailabilityKey = FareAvailabilityKey
                }},
                passengers = new
                {
                    types = _types
                },
                    currencyCode = "INR",
                    promotionCode = (string)null
                };

                // Serialize the JSON object to a string
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(tripSell__ );
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetTripSellRequest", "air_Qp", Request, SearchID, ex.Message);
            }
            return Request;
        }
        //=================== end TripSell

        //===== start Quote
        public string GetQuoteRequest(string CompanyID, DataTable dtSelect, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string Request = "";

            try
            {

                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }

                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string StateName = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");


                string[] splitEmail = Email.Split('@');
                string kkEmail = splitEmail[0].ToString().Trim() + "//" + splitEmail[1].ToString().Trim();
                string SegmentRef = dtSelect.Rows[0]["SegmentRef"].ToString();
                string CarrierCode = dtSelect.Rows[0]["CarrierCode"].ToString();
                string Sector = dtSelect.Rows[0]["Sector"].ToString();
                string FltType = dtSelect.Rows[0]["FltType"].ToString();

                string JourneyKey = dtSelect.Rows[0]["JourneySellKey"].ToString();
                string FareAvailabilityKey = dtSelect.Rows[0]["BookingFareID"].ToString();
                string Destination = dtSelect.Rows[0]["Destination"].ToString();
                string Origin = dtSelect.Rows[0]["Origin"].ToString();
                string DepartureDate = dtSelect.Rows[0]["DepartureDate"].ToString();

                string Idx = dtSelect.Rows[0]["API_AirlineID"].ToString();
                string[] splitIdx = Idx.Split('_');
                string Identifier = splitIdx[0];
            
                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());



                ArrayList ArayAdtFareInfo = new ArrayList();
                ArrayList ArayChdFareInfo = new ArrayList();
                ArrayList ArayInfFareInfo = new ArrayList();

                foreach (DataRow dr in dtSelect.Rows)
                {
                    ArayAdtFareInfo.Add(dr["AdtFareInfoRef"].ToString());
                    ArayChdFareInfo.Add(dr["ChdFareInfoRef"].ToString());
                    ArayInfFareInfo.Add(dr["InfFareInfoRef"].ToString());
                }

                ArayAdtFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayAdtFareInfo);
                ArayChdFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayChdFareInfo);
                ArayInfFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayInfFareInfo);


                //===== get the quoat request
                ArrayList _types = new ArrayList();
                _types.Add(new { type = "ADT", count = Adt });
                if (Chd > 0)
                {
                    _types.Add(new { type = "CHD", count = Chd });
                }
                //===========
                ArrayList _ssrs = new ArrayList();
                if (Inf > 0)
                {
                    _ssrs.Add(new
                    {
                        ssrCode = "INFT",
                        count = Inf,
                        designator = new
                        {
                            destination = Destination,
                            origin = Origin,
                            departureDate = DepartureDate
                        }
                    });
                }
                    


                var jsonObject = new
                {
                    ssrs = new[]
                {
                new
                {
                    market = new
                    {
                        identifier = new
                        {
                            identifier = Identifier,
                            carrierCode = CarrierCode
                        },
                        destination = Destination,
                        origin = Origin,
                        departureDate =DepartureDate
                    },
                    items = new[]
                    {
                            new
                            {
                                passengerType = "ADT",
                                ssrs = _ssrs
                            }
                        }
                    }
                },
                    keys = new[]
                {
                new
                {
                    journeyKey =JourneyKey,
                    fareAvailabilityKey = FareAvailabilityKey,
                    standbyPriorityCode = "",
                    inventoryControl = "HoldSpace"
                }
                },
                    passengers = new
                    {
                        types = _types,
                        residentCountry = "IN"
                    },
                    currencyCode = "INR"
                };
                //======================

                Request = Newtonsoft.Json.JsonConvert.SerializeObject( jsonObject );
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetQuoteRequest", "air_Qp", Request, SearchID, ex.Message);
            }
            return Request;
        }
        //======= end Quoate

        public string GetContactsRequest(string CompanyID, DataTable dtSelect, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string Request = "";

            try
            {


                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }

                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string StateName = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");


                string[] splitEmail = Email.Split('@');
                string kkEmail = splitEmail[0].ToString().Trim() + "//" + splitEmail[1].ToString().Trim();
                string SegmentRef = dtSelect.Rows[0]["SegmentRef"].ToString();
                string CarrierCode = dtSelect.Rows[0]["CarrierCode"].ToString();
                string Sector = dtSelect.Rows[0]["Sector"].ToString();
                string FltType = dtSelect.Rows[0]["FltType"].ToString();

                string JourneyKey = dtSelect.Rows[0]["JourneySellKey"].ToString();
                string FareAvailabilityKey = dtSelect.Rows[0]["BookingFareID"].ToString();

                string Idx = dtSelect.Rows[0]["API_AirlineID"].ToString();
                string[] splitIdx = Idx.Split('_');
                string Identifier = splitIdx[0];


                int passengerid = 1;
                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());



                ArrayList ArayAdtFareInfo = new ArrayList();
                ArrayList ArayChdFareInfo = new ArrayList();
                ArrayList ArayInfFareInfo = new ArrayList();

                foreach (DataRow dr in dtSelect.Rows)
                {
                    ArayAdtFareInfo.Add(dr["AdtFareInfoRef"].ToString());
                    ArayChdFareInfo.Add(dr["ChdFareInfoRef"].ToString());
                    ArayInfFareInfo.Add(dr["InfFareInfoRef"].ToString());
                }

                ArayAdtFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayAdtFareInfo);
                ArayChdFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayChdFareInfo);
                ArayInfFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayInfFareInfo);

      
                //======================
                StringBuilder _Contacts = new StringBuilder();
                _Contacts = new StringBuilder();
                _Contacts.Append("{");
                _Contacts.Append("\"contactTypeCode\": \"P\",");
                _Contacts.Append("\"phoneNumbers\": [");
                _Contacts.Append("{");
                _Contacts.Append("\"type\": \"Home\",");
                _Contacts.Append("\"number\": \"" + PassengerMobile + "\"");
                _Contacts.Append("},");
                _Contacts.Append("{");
                _Contacts.Append("\"type\": \"Other\",");
                _Contacts.Append("\"number\": \"" + PhoneNo + "\"");
                _Contacts.Append("}");
                _Contacts.Append("],");
                _Contacts.Append("\"cultureCode\": null,");
                _Contacts.Append("\"address\": {");
                _Contacts.Append("\"lineOne\": \"" + CompanyAddress + "\",");
                //_Contacts.Append("\"lineOne\":\"No 196,Govindappa Naicken\",");
                _Contacts.Append("\"lineTwo\": null,");
                _Contacts.Append("\"lineThree\": null,");
                _Contacts.Append("\"countryCode\": \"" + CountryCode + "\",");
                _Contacts.Append("\"provinceState\": \"" + StateCode + "\",");
                _Contacts.Append("\"city\": \"" + CityName + "\",");
                _Contacts.Append("\"postalCode\": \"" + PostalCode + "\"");
                _Contacts.Append("},");
                _Contacts.Append("\"emailAddress\": \"" + Email + "\",");
                _Contacts.Append("\"customerNumber\": null,");
                _Contacts.Append("\"sourceOrganization\": \"" + SupplierID + "\",");
                _Contacts.Append("\"distributionOption\": null,");
                _Contacts.Append("\"notificationPreference\": null,");
                _Contacts.Append("\"companyName\": \"" + CompanyName + "\",");
                _Contacts.Append("\"name\": {");
                _Contacts.Append("\"first\": \"Deepak M\",");
                _Contacts.Append("\"middle\": null,");
                _Contacts.Append("\"last\": \"Jain\",");
                _Contacts.Append("\"title\": \"Mr\",");
                _Contacts.Append("\"suffix\": null");
                _Contacts.Append("}");
                _Contacts.Append("}");


                Request = _Contacts.ToString();
               


            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetContactsRequest", "air_Qp", Request, SearchID, ex.Message);
            }
            return Request;
        }

        public string GetContactsRequest4GST(string CompanyID, DataTable dtSelect, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string Request = "";

            try
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetContactsRequest4GST", "air_Qp", Request, SearchID, Request);

                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }
                else
                {
                    return "";  //if dont have any GST details 
                }

                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string StateName = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");

                //DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetContactsRequest", "air_Qp", "GST Mail : " + GSTCompanyEmail + "  GST No : " + , SearchID, "");
                /*
                string[] splitEmail = Email.Split('@');
                string kkEmail = splitEmail[0].ToString().Trim() + "//" + splitEmail[1].ToString().Trim();
                string SegmentRef = dtSelect.Rows[0]["SegmentRef"].ToString();
                string CarrierCode = dtSelect.Rows[0]["CarrierCode"].ToString();
                string Sector = dtSelect.Rows[0]["Sector"].ToString();
                string FltType = dtSelect.Rows[0]["FltType"].ToString();

                string JourneyKey = dtSelect.Rows[0]["JourneySellKey"].ToString();
                string FareAvailabilityKey = dtSelect.Rows[0]["BookingFareID"].ToString();

                string Idx = dtSelect.Rows[0]["API_AirlineID"].ToString();
                string[] splitIdx = Idx.Split('_');
                string Identifier = splitIdx[0];

                
                int passengerid = 1;
                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());


               
                ArrayList ArayAdtFareInfo = new ArrayList();
                ArrayList ArayChdFareInfo = new ArrayList();
                ArrayList ArayInfFareInfo = new ArrayList();

                foreach (DataRow dr in dtSelect.Rows)
                {
                    ArayAdtFareInfo.Add(dr["AdtFareInfoRef"].ToString());
                    ArayChdFareInfo.Add(dr["ChdFareInfoRef"].ToString());
                    ArayInfFareInfo.Add(dr["InfFareInfoRef"].ToString());
                }

                ArayAdtFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayAdtFareInfo);
                ArayChdFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayChdFareInfo);
                ArayInfFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayInfFareInfo);
                */

                //======================
                StringBuilder _Contacts = new StringBuilder();
                _Contacts = new StringBuilder();
                _Contacts.Append("{");
                _Contacts.Append("\"contactTypeCode\":\"G\",");
                _Contacts.Append("\"phoneNumbers\": [");
                _Contacts.Append("{");
                _Contacts.Append("\"type\":\"Other\",");
                _Contacts.Append("\"number\": \"" + PassengerMobile + "\"");
                _Contacts.Append("}");
                //_Contacts.Append(",{");
                //_Contacts.Append("\"type\": \"Other\",");
                //_Contacts.Append("\"number\": \"" + PhoneNo + "\"");
                //_Contacts.Append("}");
                _Contacts.Append("],");
                _Contacts.Append("\"cultureCode\": null,");
                _Contacts.Append("\"address\": {");
                _Contacts.Append("\"lineOne\": null,");
                //_Contacts.Append("\"lineOne\":\"No 196,Govindappa Naicken\",");
                _Contacts.Append("\"lineTwo\": null,");
                _Contacts.Append("\"lineThree\": null,");
                _Contacts.Append("\"countryCode\": \"" + CountryCode + "\",");
                _Contacts.Append("\"provinceState\": \"" + StateCode + "\",");
                _Contacts.Append("\"city\": \"" + CityName + "\",");
                _Contacts.Append("\"postalCode\": \"" + PostalCode + "\"");
                _Contacts.Append("},");
                _Contacts.Append("\"emailAddress\": \"" + GSTCompanyEmail + "\",");  // "{GSTemailID}",
                _Contacts.Append("\"customerNumber\": \"" + GSTNumber + "\" ,");  // "{GSTNumber}",  
                _Contacts.Append("\"sourceOrganization\":null,");
                _Contacts.Append("\"distributionOption\":\"None\",");
                _Contacts.Append("\"notificationPreference\":\"None\",");
                _Contacts.Append("\"companyName\": \"" + CompanyName + "\",");
                _Contacts.Append("\"name\": {");
                _Contacts.Append("\"first\": \"Deepak M\",");
                _Contacts.Append("\"middle\": null,");
                _Contacts.Append("\"last\": \"Jain\",");
                _Contacts.Append("\"title\": \"Mr\",");
                _Contacts.Append("\"suffix\": null");
                _Contacts.Append("}");
                _Contacts.Append("}");


                Request = _Contacts.ToString();

                DBCommon.Logger.dbLogg(CompanyID, 0, "GetContactsRequest4GST", "air_Qp", Request, SearchID, Request);

            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetContactsRequest4GST", "air_Qp", Request, SearchID, ex.Message);
            }
            return Request;
        }

        public Dictionary<string,string> GetPassengersRequest(string CompanyID, DataTable dtSelect, DataTable dtPassenger,  DataTable dtGstInfo)
        {
            //string Request = "";
            Dictionary<string, string> _PassengerRequestList = new Dictionary<string, string>();
            try
            {
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }




                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();

                int passengerid = 1;


                StringBuilder _Passengers = new StringBuilder();
                int iSSR = 1;
                DataRow[] dtAdults = dtPassenger.Select("PaxType='" + "ADT" + "'");
                string[] AdtBTR = dtSelect.Rows[0]["AdtBTR"].ToString().Split('?');
                for (int i = 0; i < dtAdults.CopyToDataTable().Rows.Count; i++)
                {
                    DataRow dr = dtAdults.CopyToDataTable().Rows[i];
                    string Gender = "MALE";
                    if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                    {
                        Gender = "FEMALE";
                    }
                    _Passengers = new StringBuilder();
                    _Passengers.Append("{");
                    _Passengers.Append("\"name\": {");
                    _Passengers.Append("\"first\": \""+ dr["First_Name"].ToString().Trim()  + "\",");
                    _Passengers.Append("\"middle\": null,");
                    _Passengers.Append("\"last\": \""+ dr["Last_Name"].ToString().Trim() + "\",");
                    _Passengers.Append("\"title\": \""+ dr["Title"].ToString().Trim() + "\",");
                    _Passengers.Append("\"suffix\": null");
                    _Passengers.Append("},");
                    _Passengers.Append("\"info\": {");
                    _Passengers.Append("\"nationality\": \"" + (dr["Nationality"].ToString() == "" ? "IN" : dr["Nationality"].ToString()) + "\",");
                    _Passengers.Append("\"residentCountry\": \"" + (dr["PpCountry"].ToString() == "" ? "IN" : dr["PpCountry"].ToString()) + "\",");
                    _Passengers.Append("\"gender\": \""+ Gender + "\",");
                    _Passengers.Append("\"dateOfBirth\": \"" + dr["DOB"].ToString() + "\"");
                    _Passengers.Append("}");
                    _Passengers.Append("}");

                    _PassengerRequestList.Add("ADT|"+ passengerid, _Passengers.ToString());

                    passengerid++;
                }

                DataRow[] dtInfants = dtPassenger.Select("PaxType='" + "INF" + "'");
                if (dtInfants.Length > 0)
                {
                    string[] InfBTR = dtSelect.Rows[0]["InfBTR"].ToString().Split('?');
                    for (int i = 0; i < dtInfants.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtInfants.CopyToDataTable().Rows[i];
                        DateTime dtInfant = Convert.ToDateTime(dr["DOB"].ToString());
                        int Age = DateTime.Now.Year - dtInfant.Year;

                        string Gender = "MALE";
                        if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                        {
                            Gender = "FEMALE";
                        }
                        _Passengers = new StringBuilder();
                        _Passengers.Append("{");
                        _Passengers.Append("\"nationality\": \"" + (dr["Nationality"].ToString() == "" ? "IN" : dr["Nationality"].ToString()) + "\",");
                        _Passengers.Append("\"residentCountry\": \"" + (dr["PpCountry"].ToString() == "" ? "IN" : dr["PpCountry"].ToString()) + "\",");
                        _Passengers.Append("\"gender\": \"" + Gender + "\",");
                        _Passengers.Append("\"dateOfBirth\": \"" + dr["DOB"].ToString() + "\",");
                        _Passengers.Append("\"name\": {");
                        _Passengers.Append("\"first\": \"" + dr["First_Name"].ToString().Trim() + "\",");
                        _Passengers.Append("\"middle\": null,");
                        _Passengers.Append("\"last\": \"" + dr["Last_Name"].ToString().Trim() + "\",");
                        _Passengers.Append("\"title\": \"" + dr["Title"].ToString().Trim() + "\",");
                        _Passengers.Append("\"suffix\": null");
                        _Passengers.Append("}");
                        _Passengers.Append("}");

                        _PassengerRequestList.Add("INF|" + passengerid, _Passengers.ToString());


                        passengerid++;
                    }
                }

                DataRow[] dtChilds = dtPassenger.Select("PaxType='" + "CHD" + "'");
                if (dtChilds.Length > 0)
                {
                    string[] ChdBTR = dtSelect.Rows[0]["ChdBTR"].ToString().Split('?');
                    for (int i = 0; i < dtChilds.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtChilds.CopyToDataTable().Rows[i];
                        DateTime dtChild = Convert.ToDateTime(dr["DOB"].ToString());
                        int Age = DateTime.Now.Year - dtChild.Year;

                        string Gender = "MALE";
                        if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                        {
                            Gender = "FEMALE";
                        }
                        _Passengers = new StringBuilder();
                        _Passengers.Append("{");
                        _Passengers.Append("\"name\": {");
                        _Passengers.Append("\"first\": \"" + dr["First_Name"].ToString().Trim() + "\",");
                        _Passengers.Append("\"middle\": null,");
                        _Passengers.Append("\"last\": \"" + dr["Last_Name"].ToString().Trim() + "\",");
                        _Passengers.Append("\"title\": \"" + dr["Title"].ToString().Trim() + "\",");
                        _Passengers.Append("\"suffix\": null");
                        _Passengers.Append("},");
                        _Passengers.Append("\"info\": {");
                        _Passengers.Append("\"nationality\": \"" + (dr["Nationality"].ToString()==""?"IN": dr["Nationality"].ToString()) + "\",");
                        _Passengers.Append("\"residentCountry\": \"" + (dr["PpCountry"].ToString()==""?"IN": dr["PpCountry"].ToString()) + "\",");
                        _Passengers.Append("\"gender\": \"" + Gender + "\",");
                        _Passengers.Append("\"dateOfBirth\": \"" + dr["DOB"].ToString() + "\"");
                        _Passengers.Append("}");
                        _Passengers.Append("}");

                        _PassengerRequestList.Add("CHD|" + passengerid, _Passengers.ToString());


                        passengerid++;
                    }
                }

             
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetPassengerRequest", "air_Qp", "", SearchID, ex.Message);
            }
            return _PassengerRequestList;
        }
        public Dictionary<string, string> GetAssignSeatRequest(string CompanyID, DataTable dtSelect, DataTable dtPassenger)
        {
            //string Request = "";
            Dictionary<string, string> _AssignSeatRequestList = new Dictionary<string, string>();
            try
            {


                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();




                int iSSR = 1;
                DataRow[] dtAdults = dtPassenger.Select("PaxType='" + "ADT" + "'");
                string[] AdtBTR = dtSelect.Rows[0]["AdtBTR"].ToString().Split('?');
                for (int i = 0; i < dtAdults.CopyToDataTable().Rows.Count; i++)
                {
                    DataRow dr = dtAdults.CopyToDataTable().Rows[i];
                    string Gender = "MALE";
                    if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                    {
                        Gender = "FEMALE";
                    }

                    //=========== create _AssignSeat request
                    /*StringBuilder _AssignSeat = new StringBuilder();
                    _AssignSeat.Append("{");

                    _AssignSeat.Append("\"journeyKey\": \""+ dr["JourneySellKey"].ToString() + "\",");
                    _AssignSeat.Append("\"collectedCurrencyCode\": \"INR\"");

                    _AssignSeat.Append("}");
                    _AssignSeatRequestList.Add("ADT", _AssignSeat.ToString());
                    */
                    //======= end _AssignSeat request
                    //passengerid++;
                }

                DataRow[] dtInfants = dtPassenger.Select("PaxType='" + "INF" + "'");
                if (dtInfants.Length > 0)
                {
                    string[] InfBTR = dtSelect.Rows[0]["InfBTR"].ToString().Split('?');
                    for (int i = 0; i < dtInfants.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtInfants.CopyToDataTable().Rows[i];
                        DateTime dtInfant = Convert.ToDateTime(dr["DOB"].ToString());
                        int Age = DateTime.Now.Year - dtInfant.Year;

                        string Gender = "MALE";
                        if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                        {
                            Gender = "FEMALE";
                        }


                        //=========== create _AssignSeat request
                        /*StringBuilder _AssignSeat = new StringBuilder();
                        _AssignSeat.Append("{");

                        _AssignSeat.Append("\"journeyKey\": \""+ dr["JourneySellKey"].ToString() + "\",");
                        _AssignSeat.Append("\"collectedCurrencyCode\": \"INR\"");

                        _AssignSeat.Append("}");
                        _AssignSeatRequestList.Add("INF", _AssignSeat.ToString());
                        */
                        //======= end _AssignSeat request

                        //passengerid++;
                    }
                }

                DataRow[] dtChilds = dtPassenger.Select("PaxType='" + "CHD" + "'");
                if (dtChilds.Length > 0)
                {
                    string[] ChdBTR = dtSelect.Rows[0]["ChdBTR"].ToString().Split('?');
                    for (int i = 0; i < dtChilds.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtChilds.CopyToDataTable().Rows[i];
                        DateTime dtChild = Convert.ToDateTime(dr["DOB"].ToString());
                        int Age = DateTime.Now.Year - dtChild.Year;

                        string Gender = "MALE";
                        if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                        {
                            Gender = "FEMALE";
                        }


                        //=========== create _AssignSeat request
                        /*StringBuilder _AssignSeat = new StringBuilder();
                        _AssignSeat.Append("{");

                        _AssignSeat.Append("\"journeyKey\": \""+ dr["JourneySellKey"].ToString() + "\",");
                        _AssignSeat.Append("\"collectedCurrencyCode\": \"INR\"");

                        _AssignSeat.Append("}");
                        _AssignSeatRequestList.Add("CHD", _AssignSeat.ToString());
                        */
                        //======= end _AssignSeat request

                        //passengerid++;
                    }
                }


            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetAssignSeatRequest", "air_Qp", "", SearchID, ex.Message);
            }
            return _AssignSeatRequestList;
        }

        public string GetPaymentsRequest(string CompanyID, DataTable dtSelect, DataTable dtPassenger, DataTable dtGstInfo)
        {
            string Request = "";
            
            try
            {
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }

                //=========== get total amount
                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());
                int TotalBasePrice = Convert.ToInt32(dtSelect.Rows[0]["TotalBasic"].ToString());
                int TotalTaxes = Convert.ToInt32(dtSelect.Rows[0]["TotalTax"].ToString());

                int ddiscount = decimal.ToInt32(Convert.ToDecimal(dtSelect.Rows[0]["Adt_Import"].ToString()));
                int Totalddiscount = (Adt + Chd) * decimal.ToInt32(Convert.ToDecimal(dtSelect.Rows[0]["Adt_Import"].ToString()));
                int TotalBasePriceWithDiscount = (-1 * Totalddiscount) + TotalBasePrice;
                int TotalPrice = TotalBasePrice + TotalTaxes;


                //TotalPrice = decimal.ToInt32(_TotalJournyAmt);
                //============= end get total amoutn
                //string SupplierCode = dtSelect.Rows[0]["SupplierCode"].ToString();

                StringBuilder jsonBuilder = new StringBuilder();

                jsonBuilder.Append("{");
                jsonBuilder.Append("\"paymentMethodCode\": \"AG\",");
                jsonBuilder.Append("\"amount\":"+ TotalPrice + ",");
                jsonBuilder.Append("\"paymentFields\": {");
                jsonBuilder.Append("\"ACCTNO\": \"" + SupplierID + "\",");
                jsonBuilder.Append("\"AMT\":" + TotalPrice );
                jsonBuilder.Append("},");
                jsonBuilder.Append("\"currencyCode\": \"INR\",");
                jsonBuilder.Append("\"installments\": 1");
                jsonBuilder.Append("}");

                Request = jsonBuilder.ToString();


            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetPaymentsRequest", "air_Qp", "", SearchID, ex.Message);
            }
            return Request;
        }

        public string GetBookingsRequest(string CompanyID)
        {
            string Request = "";

            try
            {
               /* var jsonObject = new
                {
                    receivedBy = (object)null,
                    restrictionOverride = false,
                    hold = (object)null,
                    notifyContacts = false,
                    comments = (object)null,
                    contactTypesToNotify = (object)null
                };*/
                
               var jsonObject = new
                {
                    data = (object)null
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);

                Request = jsonString;

            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingsRequest", "air_Qp", "", SearchID, ex.Message);
            }
            return Request;
        }

        public string GetBookingStateRequest(string CompanyID)
        {
            string Request = "";

            try
            {
                var jsonObject = new
                {
                    data = (object)null
                };

                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);

                Request = jsonString;

            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingStateRequest", "air_Qp", "", SearchID, ex.Message);
            }
            return Request;
        }


        private int CalculateAge(string dob)
        {
            DateTime dateOfBirth = Convert.ToDateTime(dob);
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
        public string GetBookRequest_LCC(string CompanyID, DataTable dtSelect, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string Request = "";
            try
            {

                //BookingFareID AdtFareRuleKey
                //API_AirlineID ChdFareRuleKey
                //API_BookingFareID InfFareRuleKey

                bool IsSSRApplied = false;
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    if (!dr["PaxType"].ToString().Equals("INF"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            IsSSRApplied = true;
                            break;
                        }
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            IsSSRApplied = true;
                            break;
                        }
                        if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                        {
                            IsSSRApplied = true;
                            break;
                        }
                        if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                        {
                            IsSSRApplied = true;
                            break;
                        }
                    }
                }

                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                }

                string PassengerMobile = dtPassenger.Rows[0]["MobileNo"].ToString();
                string PassengerEmail = dtPassenger.Rows[0]["Email"].ToString();

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string StateName = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");

                string CarrierCode = dtSelect.Rows[0]["CarrierCode"].ToString();
                string Sector = dtSelect.Rows[0]["Sector"].ToString();
                string FltType = dtSelect.Rows[0]["FltType"].ToString();

                int passengerid = 1;
                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());

                ArrayList ArayAdtFareInfo = new ArrayList();
                ArrayList ArayChdFareInfo = new ArrayList();
                ArrayList ArayInfFareInfo = new ArrayList();

                foreach (DataRow dr in dtSelect.Rows)
                {
                    ArayAdtFareInfo.Add(dr["AdtFareInfoRef"].ToString());
                    ArayChdFareInfo.Add(dr["ChdFareInfoRef"].ToString());
                    ArayInfFareInfo.Add(dr["InfFareInfoRef"].ToString());
                }

                ArayAdtFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayAdtFareInfo);
                ArayChdFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayChdFareInfo);
                ArayInfFareInfo = DBCommon.CommonFunction.RemoveDuplicates(ArayInfFareInfo);

                Request += @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
                Request += @"<soap:Body>";

                Request += @"<univ:AirCreateReservationReq RetainReservation=""Both"" AuthorizedBy=""ZEALTRAVELS"" xmlns:air=""http://www.travelport.com/schema/air_v51_0"" xmlns:univ=""http://www.travelport.com/schema/universal_v51_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" TargetBranch=" + "\"" + TargetBranch + "\"" + "  TraceId=" + "\"" + SearchID + "\"" + " >";
                Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";

                int iSSR = 1;
                DataRow[] dtAdults = dtPassenger.Select("PaxType='" + "ADT" + "'");
                string[] AdtBTR = dtSelect.Rows[0]["AdtBTR"].ToString().Split('?');
                for (int i = 0; i < dtAdults.CopyToDataTable().Rows.Count; i++)
                {
                    DataRow dr = dtAdults.CopyToDataTable().Rows[i];
                    string Gender = "M";
                    if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                    {
                        Gender = "F";
                    }

                    //if (i.Equals(0))
                    //{
                    //Request += @"<com:BookingTraveler Key=" + "\"" + passengerid.ToString() + "\"" + " TravelerType=" + "\"ADT\"" + " Gender=" + "\"" + Gender + "\"" + ">";
                    Request += @"<com:BookingTraveler Key=" + "\"" + AdtBTR[i].ToString() + "\"" + " TravelerType=" + "\"ADT\"" + " Gender=" + "\"" + Gender + "\"" + ">";
                    Request += @"<com:BookingTravelerName Prefix =" + "\"" + dr["Title"].ToString().Trim() + "\"" + " Last = " + "\"" + dr["Last_Name"].ToString().Trim() + "\"" + " First = " + "\"" + dr["First_Name"].ToString().Trim() + "\"" + " />";

                    //Request += @"<com:DeliveryInfo>";
                    //Request += @"<com:ShippingAddress>";
                    //Request += @"<com:AddressName>" + CompanyName + "</com:AddressName>";
                    //Request += @"<com:Street>" + CompanyAddress + "</com:Street>";
                    //Request += @"<com:City>" + CityName + "</com:City>";
                    //Request += @"<com:State>" + StateName + "</com:State>";
                    //Request += @"<com:PostalCode>" + PostalCode + "</com:PostalCode>";
                    //Request += @"<com:Country>" + CountryCode + "</com:Country>";
                    //Request += @"</com:ShippingAddress>";
                    //Request += @"</com:DeliveryInfo>";

                    Request += @"<com:PhoneNumber Number=" + "\"" + PassengerMobile + "\"" + "/>";

                    //Request += @"<com:PhoneNumber Location=" + "\"" + "DEL" + "\"" + "  CountryCode=" + "\"" + "91" + "\"" + " AreaCode=" + "\"" + "999" + "\"" + " Number=" + "\"" + PassengerMobile + "\"" + "/>";   

                    if (GSTNumber.Length > 0)
                    {
                        Request += @"<com:Email Type=""P"" EmailID = " + "\"" + Email + "\"" + "/>";

                        string[] split = GSTCompanyEmail.Split('@');

                        string First = split[0].ToString().Trim().Replace("_", "..").Trim().Replace("-", "./").Trim();
                        string Second = split[1].ToString().Trim().Replace("_", "..").Trim().Replace("-", "./").Trim();

                        //Request += @"<com:SSR Type=""GSTA"" Status=""NN"" Key=" + "\"" + iSSR + "\"" + " FreeText=" + "\"" + "IND/" + GSTCompanyAddress + "\"" + " Carrier=" + "\"" + CarrierCode + "\"" + "/>";
                        //iSSR++;
                        Request += @"<com:SSR Type=""GSTN"" Status=""NN"" Key=" + "\"" + iSSR + "\"" + "  FreeText=" + "\"" + "/IND/" + GSTNumber + "/" + GSTCompanyName + "\"" + " Carrier=" + "\"" + CarrierCode + "\"" + "/>";
                        iSSR++;
                        //Request += @"<com:SSR Type=""GSTP"" Status=""NN"" Key=" + "\"" + iSSR + "\"" + "  FreeText=" + "\"" + "IND/" + GSTCompanyContactNumber + "\"" + " Carrier=" + "\"" + CarrierCode + "\"" + "/>";
                        //iSSR++;
                        Request += @"<com:SSR Type=""GSTE"" Status=""NN"" Key=" + "\"" + iSSR + "\"" + "  FreeText=" + "\"" + "/IND/" + First + "//" + Second + "\"" + " Carrier=" + "\"" + CarrierCode + "\"" + "/>";
                        iSSR++;
                    }
                    else
                    {
                        Request += @"<com:Email EmailID = " + "\"" + Email + "\"" + "/>";
                    }

                    //if (dr["MealCode_O"].ToString().Trim().Length > 2)
                    //{
                    //    Request += @"<com:SSR Key=" + "\"" + iSSR + "\"" + " Status=" + "\"" + "NN" + "\"" + " Type =" + "\"" + dr["MealCode_O"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + JSK1 + "\"" + "  Carrier = " + "\"" + CarrierCode + "\"" + "/>";
                    //    iSSR++;
                    //}
                    //if (dr["MealCode_I"].ToString().Trim().Length > 2)
                    //{
                    //    Request += @"<com:SSR Key=" + "\"" + iSSR + "\"" + " Status=" + "\"" + "NN" + "\"" + " Type =" + "\"" + dr["MealCode_I"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + JSK2 + "\"" + "  Carrier = " + "\"" + CarrierCode + "\"" + "/>";
                    //    iSSR++;
                    //}

                    //if (dr["FFN"].ToString().Trim().Length > 0)
                    //{
                    //    string Code = "";
                    //    string Number = "";
                    //    GetBookFunctions.GetFFN(CompanyID, BookingRef, dr["FFN"].ToString().Trim(), FltType, out Code, out Number);
                    //    Request += @"<com:LoyaltyCard SupplierCode=" + "\"" + Code + "\"" + " CardNumber=" + "\"" + Number + "\"" + "/>";
                    //}

                    if (Sector.Equals("I"))
                    {
                        //p-ppcountry-ppno-birth country-dob-gender-ppe-lname-fname-title
                        //"P / GB / S12345678 / IN / 12JUL90 / M / 23OCT15 / SINGH / ASHK / MR"

                        DateTime dDOB = Convert.ToDateTime(dr["DOB"].ToString());
                        DateTime dPPExpirayDate = Convert.ToDateTime(dr["PPExpirayDate"].ToString());
                        string ppdetail = "P" + "/" + dr["PpCountry"].ToString() + "/" + dr["PpNumber"].ToString() + "/" + dr["Nationality"].ToString() + "/" + dDOB.ToString("ddMMMyy").ToUpper() + "/" + Gender + "/" + dPPExpirayDate.ToString("ddMMMyy").ToUpper() + "/" + dr["First_Name"].ToString() + "/" + dr["Last_Name"].ToString();
                        //Request += @"<com:SSR Key=""1A"" Type=""DOCS"" Status=""HK"" FreeText=" + "\"" + ppdetail + "\"" + "/>";
                        Request += @"<com:SSR Type=""DOCS""  Key=" + "\"" + iSSR + "\"" + " FreeText=" + "\"" + ppdetail + "\"" + "/>";
                        iSSR++;
                    }

                    Request += @"<com:Address>";
                    Request += @"<com:AddressName>" + CompanyName + "</com:AddressName>";
                    Request += @"<com:Street>" + CompanyAddress + "</com:Street>";
                    Request += @"<com:City>" + CityName + "</com:City>";
                    Request += @"<com:State>" + StateName + "</com:State>";
                    Request += @"<com:PostalCode>" + PostalCode + "</com:PostalCode>";
                    Request += @"<com:Country>" + CountryCode + "</com:Country>";
                    Request += @"</com:Address>";
                    Request += @"</com:BookingTraveler>";
                    //}
                    //else
                    //{
                    //    Request += @"<com:BookingTraveler Key=" + "\"" + AdtBTR[i].ToString() + "\"" + " TravelerType=" + "\"ADT\"" + " Gender=" + "\"" + Gender + "\"" + ">";
                    //    Request += @"<com:BookingTravelerName Prefix=" + "\"" + dr["Title"].ToString().Trim() + "\"" + " Last=" + "\"" + dr["Last_Name"].ToString().Trim() + "\"" + " First=" + "\"" + dr["First_Name"].ToString().Trim() + "\"" + "/>";
                    //    Request += @"<com:PhoneNumber Number=" + "\"" + PassengerMobile + "\"" + "/>";
                    //    Request += @"<com:Email EmailID = " + "\"" + Email + "\"" + "/>";

                    //    //if (dr["MealCode_O"].ToString().Trim().Length > 2)
                    //    //{
                    //    //    Request += @"<com:SSR Key=" + "\"" + iSSR + "\"" + " Status=" + "\"" + "NN" + "\"" + " Type =" + "\"" + dr["MealCode_O"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + JSK1 + "\"" + "  Carrier = " + "\"" + CarrierCode + "\"" + "/>";
                    //    //    iSSR++;
                    //    //}
                    //    //if (dr["MealCode_I"].ToString().Trim().Length > 2)
                    //    //{
                    //    //    Request += @"<com:SSR Key=" + "\"" + iSSR + "\"" + " Status=" + "\"" + "NN" + "\"" + " Type =" + "\"" + dr["MealCode_I"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + JSK2 + "\"" + "  Carrier = " + "\"" + CarrierCode + "\"" + "/>";
                    //    //    iSSR++;
                    //    //}

                    //    if (Sector.Equals("I"))
                    //    {
                    //        //p-ppcountry-ppno-birth country-dob-gender-ppe-lname-fname-title
                    //        //"P / GB / S12345678 / IN / 12JUL90 / M / 23OCT15 / SINGH / ASHK / MR"

                    //        DateTime dDOB = Convert.ToDateTime(dr["DOB"].ToString());
                    //        DateTime dPPExpirayDate = Convert.ToDateTime(dr["PPExpirayDate"].ToString());
                    //        string ppdetail = "P" + "/" + dr["PpCountry"].ToString() + "/" + dr["PpNumber"].ToString() + "/" + dr["Nationality"].ToString() + "/" + dDOB.ToString("ddMMMyy").ToUpper() + "/" + Gender + "/" + dPPExpirayDate.ToString("ddMMMyy").ToUpper() + "/" + dr["First_Name"].ToString() + "/" + dr["Last_Name"].ToString();
                    //        //Request += @"<com:SSR Key=""1A"" Type=""DOCS"" Status=""HK"" FreeText=" + "\"" + ppdetail + "\"" + "/>";
                    //        Request += @"<com:SSR Type=""DOCS""  Key=" + "\"" + iSSR + "\"" + " FreeText=" + "\"" + ppdetail + "\"" + "/>";
                    //        iSSR++;
                    //    }
                    //    Request += @"</com:BookingTraveler> ";
                    //}
                    passengerid++;
                }

                DataRow[] dtChilds = dtPassenger.Select("PaxType='" + "CHD" + "'");
                if (dtChilds.Length > 0)
                {
                    string[] ChdBTR = dtSelect.Rows[0]["ChdBTR"].ToString().Split('?');
                    for (int i = 0; i < dtChilds.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtChilds.CopyToDataTable().Rows[i];
                        DateTime dtChild = Convert.ToDateTime(dr["DOB"].ToString());
                        int Age = DateTime.Now.Year - dtChild.Year;

                        string Gender = "M";
                        if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                        {
                            Gender = "F";
                        }

                        Request += @"<com:BookingTraveler Key=" + "\"" + ChdBTR[i].ToString() + "\"" + " TravelerType=" + "\"CHD\"" + " Gender=" + "\"" + Gender + "\"" + " Age=" + "\"" + Age + "\"" + " DOB=" + "\"" + dtChild.ToString("yyyy-MM-dd") + "\"" + ">";
                        Request += @"<com:BookingTravelerName Prefix=" + "\"" + dr["Title"].ToString().Trim() + "\"" + " Last=" + "\"" + dr["Last_Name"].ToString().Trim() + "\"" + " First=" + "\"" + dr["First_Name"].ToString().Trim() + "\"" + "/>";

                        //Request += @"<com:DeliveryInfo>";
                        //Request += @"<com:ShippingAddress>";
                        //Request += @"<com:AddressName>" + CompanyName + "</com:AddressName>";
                        //Request += @"<com:Street>" + CompanyAddress + "</com:Street>";
                        //Request += @"<com:City>" + CityName + "</com:City>";
                        //Request += @"<com:State>" + StateName + "</com:State>";
                        //Request += @"<com:PostalCode>" + PostalCode + "</com:PostalCode>";
                        //Request += @"<com:Country>" + CountryCode + "</com:Country>";
                        //Request += @"</com:ShippingAddress>";
                        //Request += @"</com:DeliveryInfo>";


                        Request += @"<com:PhoneNumber Number=" + "\"" + PassengerMobile + "\"" + "/>";
                        Request += @"<com:Email EmailID = " + "\"" + Email + "\"" + "/>";

                        if (Sector.Equals("I"))
                        {
                            //p - ppcountry - ppno - birth country - dob - gender - ppe - lname - fname - title
                            //"P / GB / S12345678 / IN / 12JUL90 / M / 23OCT15 / SINGH / ASHK / MR"

                            DateTime dDOB = Convert.ToDateTime(dr["DOB"].ToString());
                            DateTime dPPExpirayDate = Convert.ToDateTime(dr["PPExpirayDate"].ToString());
                            string ppdetail = "P" + "/" + dr["PpCountry"].ToString() + "/" + dr["PpNumber"].ToString() + "/" + dr["Nationality"].ToString() + "/" + dDOB.ToString("ddMMMyy").ToUpper() + "/" + Gender + "/" + dPPExpirayDate.ToString("ddMMMyy").ToUpper() + "/" + dr["First_Name"].ToString() + "/" + dr["Last_Name"].ToString();
                            //Request += @"<com:SSR Key=""1A"" Type=""DOCS"" Status=""HK"" FreeText=" + "\"" + ppdetail + "\"" + "/>";
                            Request += @"<com:SSR  Type=""DOCS"" Key=" + "\"" + iSSR + "\"" + " FreeText=" + "\"" + ppdetail + "\"" + "/>";
                            iSSR++;
                        }



                        //if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        //{
                        //    Request += @"<com:SSR Key=" + "\"" + iSSR + "\"" + " Status=" + "\"" + "NN" + "\"" + " Type =" + "\"" + dr["MealCode_O"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + JSK1 + "\"" + "  Carrier = " + "\"" + CarrierCode + "\"" + "/>";
                        //    iSSR++;
                        //}
                        //if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        //{
                        //    Request += @"<com:SSR Key=" + "\"" + iSSR + "\"" + " Status=" + "\"" + "NN" + "\"" + " Type =" + "\"" + dr["MealCode_I"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + JSK2 + "\"" + "  Carrier = " + "\"" + CarrierCode + "\"" + "/>";
                        //    iSSR++;
                        //}

                        string sss = "P-C" + Age.ToString();

                        Request += @"<com:NameRemark>";
                        Request += @"<com:RemarkData>" + sss + "</com:RemarkData>";
                        Request += @"</com:NameRemark>";

                        Request += @"<com:Address>";
                        Request += @"<com:AddressName>" + CompanyName + "</com:AddressName>";
                        Request += @"<com:Street>" + CompanyAddress + "</com:Street>";
                        Request += @"<com:City>" + CityName + "</com:City>";
                        Request += @"<com:State>" + StateName + "</com:State>";
                        Request += @"<com:PostalCode>" + PostalCode + "</com:PostalCode>";
                        Request += @"<com:Country>" + CountryCode + "</com:Country>";
                        Request += @"</com:Address>";




                        Request += @"</com:BookingTraveler>";

                        passengerid++;
                    }
                }

                DataRow[] dtInfants = dtPassenger.Select("PaxType='" + "INF" + "'");
                if (dtInfants.Length > 0)
                {
                    string[] InfBTR = dtSelect.Rows[0]["InfBTR"].ToString().Split('?');
                    for (int i = 0; i < dtInfants.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtInfants.CopyToDataTable().Rows[i];
                        DateTime dtInfant = Convert.ToDateTime(dr["DOB"].ToString());
                        int Age = DateTime.Now.Year - dtInfant.Year;

                        string Gender = "M";
                        if (dr["Title"].ToString().Trim().Equals("MRS") || dr["Title"].ToString().Trim().Equals("MS") || dr["Title"].ToString().Trim().Equals("MISS"))
                        {
                            Gender = "F";
                        }

                        Request += @"<com:BookingTraveler Key=" + "\"" + InfBTR[i].ToString() + "\"" + " TravelerType =" + "\"INF\"" + " Gender=" + "\"" + Gender + "\"" + " Age=" + "\"" + Age + "\"" + " DOB=" + "\"" + dtInfant.ToString("yyyy-MM-dd") + "\"" + ">";
                        Request += @"<com:BookingTravelerName Prefix=" + "\"" + dr["Title"].ToString().Trim() + "\"" + " Last=" + "\"" + dr["Last_Name"].ToString().Trim() + "\"" + " First=" + "\"" + dr["First_Name"].ToString().Trim() + "\"" + "/>";

                        //Request += @"<com:DeliveryInfo>";
                        //Request += @"<com:ShippingAddress>";
                        //Request += @"<com:AddressName>" + CompanyName + "</com:AddressName>";
                        //Request += @"<com:Street>" + CompanyAddress + "</com:Street>";
                        //Request += @"<com:City>" + CityName + "</com:City>";
                        //Request += @"<com:State>" + StateName + "</com:State>";
                        //Request += @"<com:PostalCode>" + PostalCode + "</com:PostalCode>";
                        //Request += @"<com:Country>" + CountryCode + "</com:Country>";
                        //Request += @"</com:ShippingAddress>";
                        //Request += @"</com:DeliveryInfo>";

                        Request += @"<com:PhoneNumber Number=" + "\"" + PassengerMobile + "\"" + "/>";
                        Request += @"<com:Email EmailID=" + "\"" + Email + "\"" + "/>";

                        if (Sector.Equals("I"))
                        {
                            //p-ppcountry-ppno-birth country-dob-gender-ppe-lname-fname-title
                            //"P / GB / S12345678 / IN / 12JUL90 / M / 23OCT15 / SINGH / ASHK / MR"

                            DateTime dDOB = Convert.ToDateTime(dr["DOB"].ToString());
                            DateTime dPPExpirayDate = Convert.ToDateTime(dr["PPExpirayDate"].ToString());
                            string ppdetail = "P" + "/" + dr["PpCountry"].ToString() + "/" + dr["PpNumber"].ToString() + "/" + dr["Nationality"].ToString() + "/" + dDOB.ToString("ddMMMyy").ToUpper() + "/" + Gender + "I" + "/" + dPPExpirayDate.ToString("ddMMMyy").ToUpper() + "/" + dr["First_Name"].ToString() + "/" + dr["Last_Name"].ToString();
                            //Request += @"<com:SSR Key=""1A"" Type=""DOCS"" Status=""HK"" FreeText=" + "\"" + ppdetail + "\"" + "/>";
                            Request += @"<com:SSR Type=""DOCS"" Key=" + "\"" + iSSR + "\"" + " FreeText=" + "\"" + ppdetail + "\"" + "/>";
                            iSSR++;
                        }

                        Request += @"<com:NameRemark>";
                        Request += @"<com:RemarkData>" + dtInfant.ToString("ddMMMyy").ToUpper() + "</com:RemarkData>";
                        Request += @"</com:NameRemark>";

                        Request += @"<com:Address>";
                        Request += @"<com:AddressName>" + CompanyName + "</com:AddressName>";
                        Request += @"<com:Street>" + CompanyAddress + "</com:Street>";
                        Request += @"<com:City>" + CityName + "</com:City>";
                        Request += @"<com:State>" + StateName + "</com:State>";
                        Request += @"<com:PostalCode>" + PostalCode + "</com:PostalCode>";
                        Request += @"<com:Country>" + CountryCode + "</com:Country>";
                        Request += @"</com:Address>";


                        Request += @"</com:BookingTraveler>";
                        passengerid++;
                    }
                }



                //string text = "CTCM" + dtAdults.CopyToDataTable().Rows[0]["MobileNo"].ToString();
                //if (dtSelect.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                //{
                //    Request += @"<com:OSI Key=""1"" Carrier=" + "\"" + dtSelect.Rows[0]["CarrierCode"].ToString() + "\"" + " Text=" + "\"" + text + "\"" + " ProviderCode=" + "\"ACH\"" + "/>";
                //}
                //else
                //{
                //    Request += @"<com:OSI Key=""1"" Carrier=" + "\"" + dtSelect.Rows[0]["CarrierCode"].ToString() + "\"" + " Text=" + "\"" + text + "\"" + " ProviderCode=" + "\"1G\"" + "/>";
                //}

                DataRow drMobile = dtAdults.CopyToDataTable().Rows[0];
                string Frst1 = "91";
                string Frst2 = drMobile["MobileNo"].ToString();
                if (drMobile["MobileNo"].ToString().Trim().Length > 11)
                {
                    Frst2 = drMobile["MobileNo"].ToString().Substring(2, 12);
                }


                Request += @"<com:ContinuityCheckOverride Key=""0 "">Yes</com:ContinuityCheckOverride>";


                Request += @"<com:AgencyContactInfo> ";
                Request += @"<com:PhoneNumber Type=""Agency"" Location=""DEL"" CountryCode=""0091"" Text=" + "\"" + CompanyName + "\"" + " AreaCode=" + "\"" + Frst1 + "\"" + "  Number=" + "\"" + Frst2 + "\"" + " />";
                Request += @"</com:AgencyContactInfo>";

                Request += @"<com:EmailNotification Recipients = ""All""/>";

                Request += @"<com:FormOfPayment Key = ""1"" Type = ""AgencyPayment"">";
                Request += @"<com:AgencyPayment AgencyBillingIdentifier=" + "\"" + UserName + "\"" + " AgencyBillingPassword=" + "\"" + Password + "\"" + "/>";
                Request += @"</com:FormOfPayment>";



                //=========================================================================

                int TotalBasePrice = Convert.ToInt32(dtSelect.Rows[0]["TotalBasic"].ToString());
                int TotalTaxes = Convert.ToInt32(dtSelect.Rows[0]["TotalTax"].ToString());

                int ddiscount = decimal.ToInt32(Convert.ToDecimal(dtSelect.Rows[0]["Adt_Import"].ToString()));
                int Totalddiscount = (Adt + Chd) * decimal.ToInt32(Convert.ToDecimal(dtSelect.Rows[0]["Adt_Import"].ToString()));
                int TotalBasePriceWithDiscount = (-1 * Totalddiscount) + TotalBasePrice;
                int TotalPrice = TotalBasePrice + TotalTaxes;

                Request += @"<air:AirPricingSolution Key=" + "\"" + dtSelect.Rows[0]["AirPricePointKey"].ToString() + "\"" + " TotalPrice=" + "\"" + "INR" + TotalPrice + ".00" + "\"" + " ApproximateTotalPrice=" + "\"" + "INR" + TotalPrice + ".00" + "\"" + " ApproximateBasePrice=" + "\"" + "INR" + TotalBasePriceWithDiscount + ".00" + "\"" + " BasePrice=" + "\"" + "INR" + TotalBasePriceWithDiscount + ".00" + "\"" + " ApproximateTaxes=" + "\"" + "INR" + TotalTaxes + ".00" + "\"" + " Taxes=" + "\"" + "INR" + TotalTaxes + ".00" + "\"" + " Fees=" + "\"" + "INR" + Totalddiscount + ".00" + "\"" + ">";
                int k1 = 0;
                int k2 = 0;
                foreach (DataRow dr in dtSelect.Rows)//airSegemnt
                {
                    string ProviderCode = dr["ProviderCode"].ToString();
                    string TravelTime = dr["TravelTime"].ToString();
                    string OptionalServicesIndicator = dr["OptionalServicesIndicator"].ToString();
                    string AvailabilitySource = dr["AvailabilitySource"].ToString();
                    string AvailabilityDisplayType = dr["AvailabilityDisplayType"].ToString();
                    string Group = dr["Group"].ToString();
                    string FlightTime = dr["FlightTime"].ToString();
                    string Distance = dr["Distance"].ToString();

                    string APISRequirementsRef = dr["TempData1"].ToString();
                    string Status = dr["TempData2"].ToString();
                    string ChangeOfPlane = dr["TempData3"].ToString();
                    string Equipment = dr["EquipmentType"].ToString();


                    string CodeshareInfoOperatingCarrier = dr["CodeshareInfoOperatingCarrier"].ToString();
                    string CodeshareInfoOperatingFlightNumber = dr["CodeshareInfoOperatingFlightNumber"].ToString();
                    string CodeshareInfo = dr["CodeshareInfo"].ToString();

                    Request += @"<air:AirSegment Key=" + "\"" + dr["SegmentRef"].ToString() + "\"" + "";
                    if (OptionalServicesIndicator.Length > 0)
                    {
                        Request += @" OptionalServicesIndicator =" + "\"" + OptionalServicesIndicator + "\"" + "";
                    }
                    if (AvailabilityDisplayType.Length > 0)
                    {
                        Request += @" AvailabilityDisplayType =" + "\"" + AvailabilityDisplayType + "\"" + "";
                    }

                    Request += @" Group =" + "\"" + Group + "\"" + "";
                    Request += @" Carrier =" + "\"" + dr["CarrierCode"].ToString() + "\"" + "";
                    Request += @" SupplierCode =" + "\"" + dr["CarrierCode"].ToString() + "\"" + "";
                    Request += @" FlightNumber =" + "\"" + dr["FlightNumber"].ToString() + "\"" + "";
                    Request += @" Origin =" + "\"" + dr["DepartureStation"].ToString() + "\"" + "";
                    Request += @" Destination =" + "\"" + dr["ArrivalStation"].ToString() + "\"" + "";
                    Request += @" DepartureTime =" + "\"" + dr["DepDate"].ToString() + "\"" + "";
                    Request += @" ArrivalTime =" + "\"" + dr["ArrDate"].ToString() + "\"" + "";
                    Request += @" HostTokenRef =" + "\"" + dr["AdtHTR"].ToString() + "\"" + "";

                    if (APISRequirementsRef.Length > 0)
                    {
                        Request += @" APISRequirementsRef =" + "\"" + APISRequirementsRef + "\"" + "";
                    }
                    if (Status.Length > 0)
                    {
                        Request += @" Status =" + "\"" + Status + "\"" + "";
                    }
                    if (ChangeOfPlane.Length > 0)
                    {
                        Request += @" ChangeOfPlane =" + "\"" + ChangeOfPlane + "\"" + "";
                    }
                    if (Equipment.Length > 0)
                    {
                        Request += @" Equipment =" + "\"" + Equipment + "\"" + "";
                    }

                    if (FlightTime.Length > 0)
                    {
                        Request += @" FlightTime =" + "\"" + FlightTime + "\"" + "";
                    }
                    if (TravelTime.Length > 0)
                    {
                        Request += @" TravelTime =" + "\"" + TravelTime + "\"" + "";
                    }
                    if (Distance.Length > 0)
                    {
                        Request += @" Distance =" + "\"" + Distance + "\"" + "";
                    }

                    Request += @" ProviderCode =" + "\"" + dr["ProviderCode"].ToString() + "\"" + "";
                    Request += @" ClassOfService =" + "\"" + dr["ClassOfService"].ToString() + "\"" + "";
                    if (AvailabilitySource.Length > 0)
                    {
                        Request += @" AvailabilitySource =" + "\"" + AvailabilitySource + "\"" + "";
                    }

                    Request += @" >";

                    if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length > 0 && CodeshareInfo.Length > 0)
                    {
                        Request += @" <air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + " OperatingFlightNumber=" + "\"" + CodeshareInfoOperatingFlightNumber + "\"" + ">" + CodeshareInfo + "</air:CodeshareInfo>";
                    }
                    else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length > 0 && CodeshareInfo.Length.Equals(0))
                    {
                        Request += @" <air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + " OperatingFlightNumber=" + "\"" + CodeshareInfoOperatingFlightNumber + "\"" + "></air:CodeshareInfo>";
                    }
                    else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length.Equals(0) && CodeshareInfo.Length.Equals(0))
                    {
                        Request += @" <air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + "</air:CodeshareInfo>";
                    }
                    else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length.Equals(0) && CodeshareInfo.Length > 0)
                    {
                        Request += @" <air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + ">" + CodeshareInfo + "</air:CodeshareInfo>";
                    }

                    if (dr["FltType"].ToString().Equals("O"))
                    {
                        if (k1.Equals(0))
                        {
                            k1++;
                            if (dr["Connection_O"].ToString().Length > 0)
                            {
                                string[] Conn = dr["Connection_O"].ToString().Split('?');
                                for (int i = 0; i < Conn.Length; i++)
                                {
                                    Request += @"<air:Connection SegmentIndex=" + "\"" + Conn[i].ToString() + "\"" + "/>";
                                }
                            }
                            else
                            {
                                Request += @"<air:Connection/>";
                            }
                        }
                    }
                    else
                    {
                        if (k2.Equals(0))
                        {
                            k2++;
                            if (dr["Connection_I"].ToString().Length > 0)
                            {
                                string[] Conn = dr["Connection_I"].ToString().Split('?');
                                for (int i = 0; i < Conn.Length; i++)
                                {
                                    Request += @"<air:Connection SegmentIndex=" + "\"" + Conn[i].ToString() + "\"" + "/>";
                                }
                            }
                            else
                            {
                                Request += @"<air:Connection/>";
                            }
                        }
                    }

                    Request += @" </air:AirSegment>";
                }

                passengerid = 1;

                string Refundable = "true";
                if (!dtSelect.Rows[0]["RefundType"].ToString().Trim().Equals("N"))
                {
                    Refundable = "false";
                }

                int AdtBasePrice = Convert.ToInt32(dtSelect.Rows[0]["AdtTotalBasic"].ToString());
                int AdtBasePriceWithDiscount = (-1 * ddiscount) + AdtBasePrice;
                int AdtTotalTaxes = Convert.ToInt32(dtSelect.Rows[0]["AdtTotalTax"].ToString());
                int AdtTotalPrice = AdtBasePrice + AdtTotalTaxes;
                string ETicketability = dtSelect.Rows[0]["ETicketability"].ToString().Trim();
                string LatestTicketingTime = dtSelect.Rows[0]["LatestTicketingTime"].ToString().Trim();

                Request += @"<air:AirPricingInfo  Key=" + "\"" + dtSelect.Rows[0]["AdtAirPricingInfoKey"].ToString() + "\"" + "  TotalPrice=" + "\"" + "INR" + AdtTotalPrice + ".00" + "\"" + " ApproximateTotalPrice=" + "\"" + "INR" + AdtTotalPrice + ".00" + "\"" + " ApproximateBasePrice=" + "\"" + "INR" + AdtBasePrice + ".00" + "\"" + " BasePrice=" + "\"" + "INR" + AdtBasePrice + ".00" + "\"" + " ApproximateTaxes=" + "\"" + "INR" + AdtTotalTaxes + ".00" + "\"" + " Taxes=" + "\"" + "INR" + AdtTotalTaxes + ".00" + "\"" + "";
                //if (LatestTicketingTime.Length > 0)
                //{
                //    Request += @"LatestTicketingTime=" + "\"" + Convert.ToDateTime(dtSelect.Rows[0]["LatestTicketingTime"].ToString().Trim()).ToString("yyyy-MM-dd") + "\"" + "";
                //}
                if (dtSelect.Rows[0]["PricingMethod"].ToString().Trim().Length > 0)
                {
                    Request += @" PricingMethod =" + "\"" + dtSelect.Rows[0]["PricingMethod"].ToString().Trim() + "\"" + "";
                }
                if (Refundable.Length > 0)
                {
                    Request += @" Refundable =" + "\"" + Refundable + "\"" + "";
                }
                if (ETicketability.Length > 0)
                {
                    Request += @" ETicketability =" + "\"" + ETicketability + "\"" + "";
                }
                if (dtSelect.Rows[0]["PlatingCarrier"].ToString().Trim().Length > 0)
                {
                    Request += @" PlatingCarrier =" + "\"" + dtSelect.Rows[0]["PlatingCarrier"].ToString().Trim() + "\"" + "";
                }
                if (dtSelect.Rows[0]["ProviderCode"].ToString().Trim().Length > 0)
                {
                    Request += @" ProviderCode =" + "\"" + dtSelect.Rows[0]["ProviderCode"].ToString().Trim() + "\"" + "";
                }
                Request += @">";

                for (int m = 0; m < ArayAdtFareInfo.Count; m++)
                {
                    string FareRuleKey_Text = "";
                    string[] SplitAdtFareRuleKey = dtSelect.Rows[0]["BookingFareID"].ToString().Trim().Split('?');//AdtFareRuleKey
                    for (int p = 0; p < SplitAdtFareRuleKey.Length; p++)
                    {
                        string[] splitFareKey = SplitAdtFareRuleKey[p].ToString().Trim().Split('@');
                        if (splitFareKey[0].ToString().Trim().Equals(ArayAdtFareInfo[m].ToString()))
                        {
                            FareRuleKey_Text = splitFareKey[1].ToString().Trim();
                            break;
                        }
                    }

                    DataRow[] drFareInfoRef = dtSelect.Select("AdtFareInfoRef='" + ArayAdtFareInfo[m].ToString() + "'");
                    string[] Split = drFareInfoRef.CopyToDataTable().Rows[0]["AdtFareInfoRef_data"].ToString().Split('?');
                    string Origin = "";
                    string Destination = "";
                    string EffectiveDate = "";
                    string DepartureDate = "";
                    string Amount = "";
                    string NegotiatedFare = "";
                    string NotValidBefore = "";
                    string NotValidAfter = "";
                    string TaxAmount = "";
                    string FareBasis = "";

                    string PromotionalFare = "";
                    string FareFamily = "";
                    string SupplierCode = "";

                    for (int i = 0; i < Split.Length; i++)
                    {
                        if (Split[i].ToString().IndexOf("Origin") != -1)
                        {
                            Origin = Split[i].ToString().Replace("Origin:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("Destination") != -1)
                        {
                            Destination = Split[i].ToString().Replace("Destination:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("EffectiveDate") != -1)
                        {
                            EffectiveDate = Split[i].ToString().Replace("EffectiveDate:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("DepartureDate") != -1)
                        {
                            DepartureDate = Split[i].ToString().Replace("DepartureDate:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("TaxAmount") != -1)
                        {
                            TaxAmount = Split[i].ToString().Replace("TaxAmount:", "").Trim().Replace("INR", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("Amount") != -1 && Split[i].ToString().IndexOf("TaxAmount") == -1)
                        {
                            Amount = Split[i].ToString().Replace("Amount:", "").Trim().Replace("INR", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("NegotiatedFare") != -1)
                        {
                            NegotiatedFare = Split[i].ToString().Replace("NegotiatedFare:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("NotValidBefore") != -1)
                        {
                            NotValidBefore = Split[i].ToString().Replace("NotValidBefore:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("NotValidAfter") != -1)
                        {
                            NotValidAfter = Split[i].ToString().Replace("NotValidAfter:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("PromotionalFare") != -1)
                        {
                            PromotionalFare = Split[i].ToString().Replace("PromotionalFare:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("FareFamily") != -1)
                        {
                            FareFamily = Split[i].ToString().Replace("FareFamily:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("SupplierCode") != -1)
                        {
                            SupplierCode = Split[i].ToString().Replace("SupplierCode:", "").Trim();
                        }
                        else if (Split[i].ToString().IndexOf("FareBasis") != -1)
                        {
                            FareBasis = Split[i].ToString().Replace("FareBasis:", "").Trim();
                        }

                    }


                    Request += @"<air:FareInfo PassengerTypeCode=""ADT"" Key=" + "\"" + ArayAdtFareInfo[m].ToString() + "\"" + " Amount=" + "\"" + "INR" + TotalBasePrice + ".00" + "\"" + " TaxAmount=" + "\"" + "INR" + TotalTaxes + ".00" + "\"" + " Origin=" + "\"" + Origin + "\"" + " Destination=" + "\"" + Destination + "\"" + "";
                    if (EffectiveDate.Length > 0)
                    {
                        Request += @" EffectiveDate =" + "\"" + EffectiveDate + "\"" + "";
                    }
                    if (DepartureDate.Length > 0)
                    {
                        Request += @" DepartureDate =" + "\"" + DepartureDate + "\"" + "";
                    }
                    if (NegotiatedFare.Length > 0)
                    {
                        Request += @" NegotiatedFare =" + "\"" + NegotiatedFare + "\"" + "";
                    }
                    if (NotValidBefore.Length > 0)
                    {
                        Request += @" NotValidBefore =" + "\"" + NotValidBefore + "\"" + "";
                    }
                    if (NotValidAfter.Length > 0)
                    {
                        Request += @" NotValidAfter =" + "\"" + NotValidAfter + "\"" + "";
                    }

                    if (PromotionalFare.Length > 0)
                    {
                        Request += @" PromotionalFare =" + "\"" + PromotionalFare + "\"" + "";
                    }
                    if (FareFamily.Length > 0)
                    {
                        Request += @" FareFamily =" + "\"" + FareFamily + "\"" + "";
                    }
                    if (SupplierCode.Length > 0)
                    {
                        Request += @" SupplierCode =" + "\"" + SupplierCode + "\"" + "";
                    }

                    if (FareBasis.Length > 0)
                    {
                        Request += @" FareBasis =" + "\"" + FareBasis + "\"" + "";
                    }

                    Request += @">";

                    Request += @"<air:FareRuleKey FareInfoRef=" + "\"" + ArayAdtFareInfo[m].ToString() + "\"" + " ProviderCode=" + "\"" + dtSelect.Rows[0]["ProviderCode"].ToString().Trim() + "\"" + " >";
                    Request += FareRuleKey_Text;
                    Request += @" </air:FareRuleKey>";

                    Request += @"</air:FareInfo>";
                }

                foreach (DataRow dr in dtSelect.Rows)//airSegemnt
                {
                    Request += @"<air:BookingInfo HostTokenRef=" + "\"" + dr["AdtHTR"].ToString().Trim() + "\"" + " BookingCode=" + "\"" + dr["ClassOfService"].ToString().Trim() + "\"" + " FareInfoRef=" + "\"" + dr["AdtFareInfoRef"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + dr["SegmentRef"].ToString().Trim() + "\"" + "/>";
                }

                for (int j = 0; j < Adt; j++)
                {
                    Request += @"<air:PassengerType Code=""ADT"" BookingTravelerRef=" + "\"" + AdtBTR[j].ToString() + "\"" + "/>";
                    passengerid++;
                }

                Request += @"<air:AirPricingModifiers FaresIndicator=""PublicFaresOnly"">";
                Request += @"<air:PromoCodes>";
                Request += @"<air:PromoCode Code =""CUITYCS"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                Request += @"</air:PromoCodes>";
                Request += @"</air:AirPricingModifiers>";

                Request += @"</air:AirPricingInfo>";

                if (Inf > 0)
                {
                    int InfBasePrice = Convert.ToInt32(dtSelect.Rows[0]["InfTotalBasic"].ToString());
                    int InfTotalTaxes = Convert.ToInt32(dtSelect.Rows[0]["InfTotalTax"].ToString());
                    int InfTotalPrice = InfBasePrice + InfTotalTaxes;

                    Request += @"<air:AirPricingInfo IncludesVAT=""false""  Key=" + "\"" + dtSelect.Rows[0]["InfAirPricingInfoKey"].ToString() + "\"" + "  TotalPrice=" + "\"" + "INR" + InfTotalPrice + ".00" + "\"" + " ApproximateTotalPrice=" + "\"" + "INR" + InfTotalPrice + ".00" + "\"" + " ApproximateBasePrice=" + "\"" + "INR" + InfBasePrice + ".00" + "\"" + " BasePrice=" + "\"" + "INR" + InfBasePrice + ".00" + "\"" + " ApproximateTaxes=" + "\"" + "INR" + InfTotalTaxes + ".00" + "\"" + " Taxes=" + "\"" + "INR" + InfTotalTaxes + ".00" + "\"" + " PricingMethod=" + "\"" + dtSelect.Rows[0]["PricingMethod"].ToString().Trim() + "\"" + "";
                    if (dtSelect.Rows[0]["PlatingCarrier"].ToString().Trim().Length > 0)
                    {
                        Request += @" PlatingCarrier =" + "\"" + dtSelect.Rows[0]["PlatingCarrier"].ToString().Trim() + "\"" + "";
                    }
                    Request += @" ProviderCode =" + "\"" + dtSelect.Rows[0]["ProviderCode"].ToString().Trim() + "\"" + ">";

                    for (int m = 0; m < ArayInfFareInfo.Count; m++)
                    {
                        string FareRuleKey_Text = "";
                        string[] SplitAdtFareRuleKey = dtSelect.Rows[0]["API_BookingFareID"].ToString().Trim().Split('?');//AdtFareRuleKey
                        for (int p = 0; p < SplitAdtFareRuleKey.Length; p++)
                        {
                            string[] splitFareKey = SplitAdtFareRuleKey[p].ToString().Trim().Split('@');
                            if (splitFareKey[0].ToString().Trim().Equals(ArayInfFareInfo[m].ToString()))
                            {
                                FareRuleKey_Text = splitFareKey[1].ToString().Trim();
                                break;
                            }
                        }

                        DataRow[] drFareInfoRef = dtSelect.Select("InfFareInfoRef='" + ArayInfFareInfo[m].ToString() + "'");
                        string[] Split = drFareInfoRef.CopyToDataTable().Rows[0]["InfFareInfoRef_data"].ToString().Split('?');
                        string Origin = "";
                        string Destination = "";
                        string EffectiveDate = "";
                        string DepartureDate = "";
                        string Amount = "";
                        string NegotiatedFare = "";
                        string NotValidBefore = "";
                        string NotValidAfter = "";
                        string TaxAmount = "";
                        string FareBasis = "";

                        string PromotionalFare = "";
                        string FareFamily = "";
                        string SupplierCode = "";

                        for (int i = 0; i < Split.Length; i++)
                        {
                            if (Split[i].ToString().IndexOf("Origin") != -1)
                            {
                                Origin = Split[i].ToString().Replace("Origin:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("Destination") != -1)
                            {
                                Destination = Split[i].ToString().Replace("Destination:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("EffectiveDate") != -1)
                            {
                                EffectiveDate = Split[i].ToString().Replace("EffectiveDate:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("DepartureDate") != -1)
                            {
                                DepartureDate = Split[i].ToString().Replace("DepartureDate:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("TaxAmount") != -1)
                            {
                                TaxAmount = Split[i].ToString().Replace("TaxAmount:", "").Trim().Replace("INR", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("Amount") != -1 && Split[i].ToString().IndexOf("TaxAmount") == -1)
                            {
                                Amount = Split[i].ToString().Replace("Amount:", "").Trim().Replace("INR", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("NegotiatedFare") != -1)
                            {
                                NegotiatedFare = Split[i].ToString().Replace("NegotiatedFare:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("NotValidBefore") != -1)
                            {
                                NotValidBefore = Split[i].ToString().Replace("NotValidBefore:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("NotValidAfter") != -1)
                            {
                                NotValidAfter = Split[i].ToString().Replace("NotValidAfter:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("PromotionalFare") != -1)
                            {
                                PromotionalFare = Split[i].ToString().Replace("PromotionalFare:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("FareFamily") != -1)
                            {
                                FareFamily = Split[i].ToString().Replace("FareFamily:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("SupplierCode") != -1)
                            {
                                SupplierCode = Split[i].ToString().Replace("SupplierCode:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("FareBasis") != -1)
                            {
                                FareBasis = Split[i].ToString().Replace("FareBasis:", "").Trim();
                            }
                        }

                        Request += @"<air:FareInfo PassengerTypeCode=""INF"" Key=" + "\"" + ArayInfFareInfo[m].ToString() + "\"" + " Amount=" + "\"" + "INR" + TotalBasePrice + ".00" + "\"" + " TaxAmount=" + "\"" + "INR" + TotalTaxes + ".00" + "\"" + " Origin=" + "\"" + Origin + "\"" + " Destination=" + "\"" + Destination + "\"" + "";
                        if (EffectiveDate.Length > 0)
                        {
                            Request += @" EffectiveDate =" + "\"" + EffectiveDate + "\"" + "";
                        }
                        if (DepartureDate.Length > 0)
                        {
                            Request += @" DepartureDate =" + "\"" + DepartureDate + "\"" + "";
                        }
                        if (NegotiatedFare.Length > 0)
                        {
                            Request += @" NegotiatedFare =" + "\"" + NegotiatedFare + "\"" + "";
                        }
                        if (NotValidBefore.Length > 0)
                        {
                            Request += @" NotValidBefore =" + "\"" + NotValidBefore + "\"" + "";
                        }
                        if (NotValidAfter.Length > 0)
                        {
                            Request += @" NotValidAfter =" + "\"" + NotValidAfter + "\"" + "";
                        }

                        if (PromotionalFare.Length > 0)
                        {
                            Request += @" PromotionalFare =" + "\"" + PromotionalFare + "\"" + "";
                        }
                        if (FareFamily.Length > 0)
                        {
                            Request += @" FareFamily =" + "\"" + FareFamily + "\"" + "";
                        }
                        if (SupplierCode.Length > 0)
                        {
                            Request += @" SupplierCode =" + "\"" + SupplierCode + "\"" + "";
                        }


                        if (FareBasis.Length > 0)
                        {
                            Request += @" FareBasis =" + "\"" + FareBasis + "\"" + ">";
                        }

                        Request += @"<air:FareRuleKey FareInfoRef=" + "\"" + ArayInfFareInfo[m].ToString() + "\"" + " ProviderCode=" + "\"" + dtSelect.Rows[0]["ProviderCode"].ToString().Trim() + "\"" + " >";
                        Request += FareRuleKey_Text;
                        Request += @"</air:FareRuleKey>";

                        Request += @"</air:FareInfo>";
                    }

                    foreach (DataRow dr in dtSelect.Rows)//airSegemnt
                    {
                        Request += @"<air:BookingInfo HostTokenRef=" + "\"" + dr["InfHTR"].ToString().Trim() + "\"" + " BookingCode=" + "\"" + dr["ClassOfService"].ToString().Trim() + "\"" + " FareInfoRef=" + "\"" + dr["InfFareInfoRef"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + dr["SegmentRef"].ToString().Trim() + "\"" + "/>";
                    }

                    string[] InfBTR = dtSelect.Rows[0]["InfBTR"].ToString().Split('?');
                    for (int i = 0; i < dtInfants.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtInfants.CopyToDataTable().Rows[i];
                        Request += @"<air:PassengerType Code=""INF"" Age=" + "\"" + CalculateAge(dr["DOB"].ToString()) + "\"" + " BookingTravelerRef=" + "\"" + InfBTR[i].ToString() + "\"" + "/>";
                        passengerid++;
                    }

                    Request += @"<air:AirPricingModifiers FaresIndicator=""PublicFaresOnly"">";
                    Request += @"<air:PromoCodes>";
                    Request += @"<air:PromoCode Code =""CUITYCS"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                    Request += @"</air:PromoCodes>";
                    Request += @"</air:AirPricingModifiers>";

                    Request += @"</air:AirPricingInfo>";
                }

                if (Chd > 0)
                {
                    int ChdBasePrice = Convert.ToInt32(dtSelect.Rows[0]["ChdTotalBasic"].ToString());
                    int ChdBasePriceWithDiscount = (-1 * ddiscount) + ChdBasePrice;
                    int ChdTotalTaxes = Convert.ToInt32(dtSelect.Rows[0]["ChdTotalTax"].ToString());
                    int ChdTotalPrice = ChdBasePrice + ChdTotalTaxes;

                    Request += @"<air:AirPricingInfo IncludesVAT=""false""  Key=" + "\"" + dtSelect.Rows[0]["ChdAirPricingInfoKey"].ToString() + "\"" + "  TotalPrice=" + "\"" + "INR" + ChdTotalPrice + ".00" + "\"" + " ApproximateTotalPrice=" + "\"" + "INR" + ChdTotalPrice + ".00" + "\"" + " ApproximateBasePrice=" + "\"" + "INR" + ChdBasePrice + ".00" + "\"" + " BasePrice=" + "\"" + "INR" + ChdBasePrice + ".00" + "\"" + " ApproximateTaxes=" + "\"" + "INR" + ChdTotalTaxes + ".00" + "\"" + " Taxes=" + "\"" + "INR" + ChdTotalTaxes + ".00" + "\"" + " PricingMethod=" + "\"" + dtSelect.Rows[0]["PricingMethod"].ToString().Trim() + "\"" + "";
                    if (dtSelect.Rows[0]["PlatingCarrier"].ToString().Trim().Length > 0)
                    {
                        Request += @" PlatingCarrier =" + "\"" + dtSelect.Rows[0]["PlatingCarrier"].ToString().Trim() + "\"" + "";
                    }
                    Request += @" ProviderCode =" + "\"" + dtSelect.Rows[0]["ProviderCode"].ToString().Trim() + "\"" + ">";

                    for (int m = 0; m < ArayChdFareInfo.Count; m++)
                    {
                        string FareRuleKey_Text = "";
                        string[] SplitAdtFareRuleKey = dtSelect.Rows[0]["API_AirlineID"].ToString().Trim().Split('?');//AdtFareRuleKey
                        for (int p = 0; p < SplitAdtFareRuleKey.Length; p++)
                        {
                            string[] splitFareKey = SplitAdtFareRuleKey[p].ToString().Trim().Split('@');
                            if (splitFareKey[0].ToString().Trim().Equals(ArayChdFareInfo[m].ToString()))
                            {
                                FareRuleKey_Text = splitFareKey[1].ToString().Trim();
                                break;
                            }
                        }

                        DataRow[] drFareInfoRef = dtSelect.Select("ChdFareInfoRef='" + ArayChdFareInfo[m].ToString() + "'");
                        string[] Split = drFareInfoRef.CopyToDataTable().Rows[0]["ChdFareInfoRef_data"].ToString().Split('?');
                        string Origin = "";
                        string Destination = "";
                        string EffectiveDate = "";
                        string DepartureDate = "";
                        string Amount = "";
                        string NegotiatedFare = "";
                        string NotValidBefore = "";
                        string NotValidAfter = "";
                        string TaxAmount = "";
                        string FareBasis = "";

                        string PromotionalFare = "";
                        string FareFamily = "";
                        string SupplierCode = "";

                        for (int i = 0; i < Split.Length; i++)
                        {
                            if (Split[i].ToString().IndexOf("Origin") != -1)
                            {
                                Origin = Split[i].ToString().Replace("Origin:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("Destination") != -1)
                            {
                                Destination = Split[i].ToString().Replace("Destination:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("EffectiveDate") != -1)
                            {
                                EffectiveDate = Split[i].ToString().Replace("EffectiveDate:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("DepartureDate") != -1)
                            {
                                DepartureDate = Split[i].ToString().Replace("DepartureDate:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("TaxAmount") != -1)
                            {
                                TaxAmount = Split[i].ToString().Replace("TaxAmount:", "").Trim().Replace("INR", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("Amount") != -1 && Split[i].ToString().IndexOf("TaxAmount") == -1)
                            {
                                Amount = Split[i].ToString().Replace("Amount:", "").Trim().Replace("INR", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("NegotiatedFare") != -1)
                            {
                                NegotiatedFare = Split[i].ToString().Replace("NegotiatedFare:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("NotValidBefore") != -1)
                            {
                                NotValidBefore = Split[i].ToString().Replace("NotValidBefore:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("NotValidAfter") != -1)
                            {
                                NotValidAfter = Split[i].ToString().Replace("NotValidAfter:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("PromotionalFare") != -1)
                            {
                                PromotionalFare = Split[i].ToString().Replace("PromotionalFare:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("FareFamily") != -1)
                            {
                                FareFamily = Split[i].ToString().Replace("FareFamily:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("SupplierCode") != -1)
                            {
                                SupplierCode = Split[i].ToString().Replace("SupplierCode:", "").Trim();
                            }
                            else if (Split[i].ToString().IndexOf("FareBasis") != -1)
                            {
                                FareBasis = Split[i].ToString().Replace("FareBasis:", "").Trim();
                            }
                        }

                        Request += @"<air:FareInfo PassengerTypeCode=""CHD"" Key=" + "\"" + ArayChdFareInfo[m].ToString() + "\"" + " Amount=" + "\"" + "INR" + TotalBasePrice + ".00" + "\"" + " TaxAmount=" + "\"" + "INR" + TotalTaxes + ".00" + "\"" + " Origin=" + "\"" + Origin + "\"" + " Destination=" + "\"" + Destination + "\"" + "";
                        if (EffectiveDate.Length > 0)
                        {
                            Request += @" EffectiveDate =" + "\"" + EffectiveDate + "\"" + "";
                        }
                        if (DepartureDate.Length > 0)
                        {
                            Request += @" DepartureDate =" + "\"" + DepartureDate + "\"" + "";
                        }
                        if (NegotiatedFare.Length > 0)
                        {
                            Request += @" NegotiatedFare =" + "\"" + NegotiatedFare + "\"" + "";
                        }
                        if (NotValidBefore.Length > 0)
                        {
                            Request += @" NotValidBefore =" + "\"" + NotValidBefore + "\"" + "";
                        }
                        if (NotValidAfter.Length > 0)
                        {
                            Request += @" NotValidAfter =" + "\"" + NotValidAfter + "\"" + "";
                        }

                        if (PromotionalFare.Length > 0)
                        {
                            Request += @" PromotionalFare =" + "\"" + PromotionalFare + "\"" + "";
                        }
                        if (FareFamily.Length > 0)
                        {
                            Request += @" FareFamily =" + "\"" + FareFamily + "\"" + "";
                        }
                        if (SupplierCode.Length > 0)
                        {
                            Request += @" SupplierCode =" + "\"" + SupplierCode + "\"" + "";
                        }

                        if (FareBasis.Length > 0)
                        {
                            Request += @" FareBasis =" + "\"" + FareBasis + "\"" + ">";
                        }

                        Request += @"<air:FareRuleKey FareInfoRef=" + "\"" + ArayChdFareInfo[m].ToString() + "\"" + " ProviderCode=" + "\"" + dtSelect.Rows[0]["ProviderCode"].ToString().Trim() + "\"" + ">";
                        Request += FareRuleKey_Text;
                        Request += @"</air:FareRuleKey>";

                        Request += @"</air:FareInfo>";
                    }

                    foreach (DataRow dr in dtSelect.Rows)//airSegemnt
                    {
                        Request += @"<air:BookingInfo HostTokenRef=" + "\"" + dr["ChdHTR"].ToString().Trim() + "\"" + " BookingCode=" + "\"" + dr["ClassOfService"].ToString().Trim() + "\"" + " FareInfoRef=" + "\"" + dr["ChdFareInfoRef"].ToString().Trim() + "\"" + " SegmentRef=" + "\"" + dr["SegmentRef"].ToString().Trim() + "\"" + "/>";
                    }

                    string[] ChdBTR = dtSelect.Rows[0]["ChdBTR"].ToString().Split('?');
                    for (int i = 0; i < dtChilds.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtChilds.CopyToDataTable().Rows[i];
                        Request += @"<air:PassengerType Code=""CHD"" Age=" + "\"" + CalculateAge(dr["DOB"].ToString()) + "\"" + " BookingTravelerRef=" + "\"" + ChdBTR[i].ToString() + "\"" + "/>";
                        passengerid++;
                    }

                    Request += @"<air:AirPricingModifiers FaresIndicator=""PublicFaresOnly"">";
                    Request += @"<air:PromoCodes>";
                    Request += @"<air:PromoCode Code =""CUITYCS"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                    Request += @"</air:PromoCodes>";
                    Request += @"</air:AirPricingModifiers>";

                    Request += @"</air:AirPricingInfo>";
                }

                string HostTokenRef = dtSelect.Rows[0]["HostTokenRef"].ToString().Trim();
                string[] SplitHostTokenRef = HostTokenRef.Split('?');
                for (int d = 0; d < SplitHostTokenRef.Length; d++)
                {
                    if (SplitHostTokenRef[d].ToString().Trim().Length > 0)
                    {
                        string[] SplitKey = SplitHostTokenRef[d].ToString().Trim().Split('@');



                        Request += @"<com:HostToken  Key=" + "\"" + SplitKey[0].ToString().Trim() + "\"" + ">" + SplitKey[1].ToString().Trim() + "</com:HostToken>";
                    }
                }

                Request += @"</air:AirPricingSolution>";

                //=========================================================================

                Request += @"<com:ActionStatus Type=""ACTIVE"" ProviderCode=""ACH"" TicketDate =""T*""/>";

                Request += @"</univ:AirCreateReservationReq>";
                Request += @"</soap:Body>";
                Request += @"</soap:Envelope>";
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookRequest", "air_uapi", Request, SearchID, ex.Message);
            }
            return Request;
        }
    }
}