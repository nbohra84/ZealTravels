using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using ZealTravel.Infrastructure.Akaasa;
using CommonComponents;
using TIRequestResponse;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Infrastructure.AirCalculations;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Models;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.BookingManagement;
namespace ZealTravel.Infrastructure.Akaasha
{
    public class GetServices: IGetServices
    {
        private static string Toke_="";
        private static DateTime TimeOutOn_=DateTime.Now;
        private  string SupplierCode = "";
        IRR_Layer _rr_Layer;
        ITI_DBData _dbData;
        ITI_Search _search;
        IGetFare _getfare;
        IGetApiFlightFareRule _getapiFlightFareRule;
        ICredential _credential;
        IBookingManagementService _bookingService;
        public GetServices(ICredential credential,IRR_Layer rr_Layer, ITI_DBData dbData,ITI_Search search, IGetFare getfare, IGetApiFlightFareRule getapiFlightFareRule, IBookingManagementService bookingService)
        {
            _credential = credential;
            _rr_Layer = rr_Layer;
            _dbData = dbData;
            _search = search;
            _getfare = getfare;
            _getapiFlightFareRule= getapiFlightFareRule;
            _bookingService = bookingService;
        }
        //AVAILABILITY============================================================================================================================================================================
        public string GetFlights(string JourneyType, string SupplierID, string SearchID, string CompanyID, string AirRQ)
        {
            try
            {
                string SupplierCode = "";
                if (SupplierID.Equals("QPMAA8752B"))
                {
                    SupplierCode = "QP";
                }

                string Sector = "D";

                //TI_Search objTI = new TI_Search();
                string _SearchRQ = _search.GetFlightSearchRequest(AirRQ, JourneyType, SearchID, SupplierCode, ref Sector);

               
                

                SearchRQ ObjsearchRQ = SerializeDeserialize.DeserializeObject<SearchRQ>(_SearchRQ, true);
                ObjsearchRQ.GeneralInfo.ShowPropertyWhileSerialize = true;

                string jsonSearchRQ = SerializeDeserialize.SerializeInJsonString(ObjsearchRQ, "SearchRQ");
                string jsonSearchRS = HitApi.HitToApi(TI_ServiceURL.GetSearchURL, jsonSearchRQ, "application/json");
                TIRequestResponse.SearchResult ObjsearchRS = SerializeDeserialize.DeserializeObject<SearchResult>(jsonSearchRS, true);

                


                if (ObjsearchRS != null && ObjsearchRS.Airlines != null && ObjsearchRS.Airlines.Flight != null && ObjsearchRS.Airlines.Flight.Length > 0)
                {
                    TI_Converter objConverter = new TI_Converter();
                    DataTable dtBound = objConverter.TISearch_2_Common(SearchID, CompanyID, ObjsearchRS, JourneyType, Sector, AirRQ, SupplierID);

                    //_rr_Layer objC = new air_db_cal.rr_Layer();
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, SearchID, CompanyID, dtBound);

                    DataSet dsBound = new DataSet();
                    dsBound.Tables.Add(dtBound.Copy());
                    dsBound.DataSetName = "root";
                    return Common.CommonFunction.DataSetToString(dsBound);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "air_tiCaller-GetServices-GetFlights", CompanyID, AirRQ, SearchID, ex.Message + "," + ex.StackTrace);
            }
            return string.Empty;
        }

        public async Task<string> GetFlightsAsync(string JourneyType, string SupplierID, string SearchID, string CompanyID, string AirRQ)
        {
            try
            {
               
                if (SupplierID.Equals("QPMAA8752B"))
                {
                    SupplierCode = "QP";
                }

                string Sector = "D";


                //DBCommon.Logger.dbLogg(CompanyID, 0, "air_tiCaller-GetServices-GetFlights-Test-BF", "", "", "", AirRQ);

                
                string SearchRQ = _search.GetFlightSearchRequest(AirRQ, JourneyType, SearchID, SupplierCode, ref Sector);


                //DBCommon.Logger.dbLogg(CompanyID, 0, "air_tiCaller-GetServices-GetFlights-Test-AF", "", "", "", SearchRQ);

                
                var commonQP = new CommonQP(_credential);
                string _Token = await commonQP.GetTokenAsync(SupplierID, SearchID);

                SearchRQ ObjsearchRQ = SerializeDeserialize.DeserializeObject<SearchRQ>(SearchRQ, true);
                ObjsearchRQ.GeneralInfo.ShowPropertyWhileSerialize = true;


                SearchRqQpV2 request = new SearchRqQpV2();
                request = CommonUtility.ConvertOldRequest2V2Request(ObjsearchRQ);
                string jsonSearchRQ = JsonConvert.SerializeObject(request);


                string jsonSearchRS = await CommonQP.GetResponseQpAsync(SearchID, jsonSearchRQ, "Search", _Token);
                DBCommon.Logger.dbLogg(CompanyID, 0, "air_tiCaller-GetServices-GetFlights-Qp", "", "", "", jsonSearchRS);

                SearchRsQpV2 ObjsearchRSV2 = JsonConvert.DeserializeObject<SearchRsQpV2>(jsonSearchRS);

                if (ObjsearchRSV2 != null && ObjsearchRSV2.Data.Results != null && ObjsearchRSV2.Data.Results != null && ObjsearchRSV2.Data.Results.Count() > 0)
                {
                    TI_Converter objConverter = new TI_Converter();
                    DataTable dtBound = objConverter.TISearch_2_Common(SearchID, CompanyID, ObjsearchRSV2, JourneyType, Sector, AirRQ, SupplierID);

                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, SearchID, CompanyID, dtBound);

                    DataSet dsBound = new DataSet();
                    dsBound.Tables.Add(dtBound.Copy());
                    dsBound.DataSetName = "root";
                    return await Task.Run(() => DBCommon.CommonFunction.DataSetToString(dsBound));
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "air_tiCaller-GetServices-GetFlights", CompanyID, AirRQ, SearchID, ex.Message + "," + ex.StackTrace);
            }
            //return string.Empty;
            return await Task.Run(() => string.Empty);
        }


        //============= get fare rule

        //FARE RULE=======================================================================================================================================================================
        public async Task<string> GetFareRuleAsync(string JourneyType, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                   //var _getapiFlightFareRule = new GetApiFlightFareRule(Searchid, Companyid, 0);
                    return _getapiFlightFareRule.GetFareRule(dtBound, Searchid, Companyid, 0);
                }
            }
            return string.Empty;
        }

        //FARE UPDATE=====================================================================================================================================================================
        public async Task<string> GetFareAsync(string JourneyType, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
                {
                    //var objFare = new GetFare(_rr_Layer,_getapiFlightFareRule, _credential,_getfare );
                    return _getfare.GetFareRT(Searchid, Companyid, AirRS);
                }
                else
                {
                    //GetFare objFare = new GetFare();
                    return _getfare.GetFareOneWay(Searchid, Companyid, AirRS);
                }
            }
            return string.Empty;
        }



        //COMMIT =================================================================================================================================================================================
        public async Task<bool> GetCommitAsync(string JourneyType, string SearchID, string CompanyID, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS)
        {
            var status = false;
            try
            {
                string SupplierID = "QPMAA8752B";
                if (AirRS != null && PassengerRS != null && CompanyRS != null && AirRS.IndexOf("RefID") != -1
               && PassengerRS.IndexOf("PassengerInfo") != -1 && CompanyRS.IndexOf("CompanyInfo") != -1)
                {



                    DataTable dtBound = DBCommon.CommonFunction.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                    DataTable dtPassengerInfo = DBCommon.CommonFunction.StringToDataSet(PassengerRS).Tables["PassengerInfo"];
                    DataTable dtCompanyInfo = DBCommon.CommonFunction.StringToDataSet(CompanyRS).Tables["CompanyInfo"];
                    DataTable dtGstInfo = new DataTable();
                    if (GstRS != null && GstRS.IndexOf("GstInfo") != -1)
                    {
                        dtGstInfo = DBCommon.CommonFunction.StringToDataSet(GstRS).Tables["CompanyInfo"];
                    }

                    if (dtBound != null && dtBound.Rows.Count > 0 && dtPassengerInfo != null && dtPassengerInfo.Rows.Count > 0 && dtCompanyInfo != null && dtCompanyInfo.Rows.Count > 0)
                    {
                        string Idx = dtBound.Rows[0]["API_AirlineID"].ToString();
                        string AirlineID = dtBound.Rows[0]["AirlineID"].ToString();
                        string FltType = dtBound.Rows[0]["FltType"].ToString();

                        //TI_Book objbook = new TI_Book();
                        //string JsonBookingXMLPathRQ = objbook.GetBookingXML(JourneyType, SearchID, CompanyID, BookingRef, dtBound, dtPassengerInfo, dtGstInfo, Idx);
                        //string JsonBookingXMLPathRS = HitApi.HitToApi(air_tiCaller.TI_ServiceURL.GetBookURL, JsonBookingXMLPathRQ, "application/json");


                        var commonQP = new CommonQP(_credential);
                        string _Token = commonQP.GetTokenAsync(SupplierID, SearchID).GetAwaiter().GetResult();

                        GetBook objbook = new GetBook(SearchID,SupplierID, CompanyID, BookingRef, _Token, _bookingService);
                        if (JourneyType.Equals("RTLCC") || JourneyType.Equals("RT"))
                        {
                           status = await objbook.GetBookResponse(AirRS, PassengerRS, CompanyRS, GstRS, false);
                            //DBCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", "O", BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, AirlineID);
                            //DBCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", "I", BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, AirlineID);
                        }
                        else
                        {
                          status = await  objbook.GetBookResponse(AirRS, PassengerRS, CompanyRS, GstRS, true);
                            //DBCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", FltType, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, AirlineID);

                        }

                        /*TIRequestResponse.BookingXML BookingXML = SerializeDeserialize.DeserializeObject<BookingXML>(JsonBookingXMLPathRS, true);
                        if (BookingXML != null && BookingXML.FlightInfo != null && BookingXML.FlightInfo.Flight != null && BookingXML.FlightInfo.Flight.PnrDetails != null && BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber != null)
                        {
                            if (JourneyType.Equals("RTLCC") || JourneyType.Equals("RT"))
                            {
                                DBCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", "O", BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, AirlineID);
                                DBCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", "I", BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, AirlineID);
                            }
                            else
                            {
                                DBCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", FltType, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber, AirlineID);
                            }

                            if (BookingXML.FlightInfo.Flight.PnrDetails.PnrNumber.Length > 0)
                            {
                                return true.ToString();
                            }
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "air_tiCaller-GetServices-GetCommit", PassengerRS, AirRS, SearchID, ex.Message + "," + ex.StackTrace);
            }
            Toke_ = "";
            return status;
        }
    }
}
