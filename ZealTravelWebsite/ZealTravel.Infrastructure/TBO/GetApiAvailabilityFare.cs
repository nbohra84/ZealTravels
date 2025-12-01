using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Infrastructure.AirCalculations;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetApiFareQuote
    {
        public string errorMessage;
        private string JourneyType;
        private string Searchid;
        private string Companyid;
        private string EndUserIp;
        IRR_Layer _rr_layer;
        IGetApiRequests _getApiRequests;
        IApiCommonFunctions _apiCommonFunctions;

        public GetApiFareQuote(string JourneyType, string Searchid, string Companyid, string EndUserIp)
        {
            this.JourneyType = JourneyType;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.EndUserIp = EndUserIp;
            _rr_layer=new rr_Layer();
           // _getApiRequests=new GetApiRequests();
            _apiCommonFunctions=new GetApiCommonFunctions();
            //GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();

        }
        //-----------------------------------------------------------------------------------------------
        public string GetFare(DataTable dtBound)
        {
            string Tokenid = dtBound.Rows[0]["Api_SessionID"].ToString().Trim().ToUpper();
            string Traceid = dtBound.Rows[0]["JourneySellKey"].ToString().Trim().ToUpper();
            string Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim().ToUpper();

            if (JourneyType.Equals("RTLCC"))
            {
                DataTable dtOutbound = dtBound.Select("FltType='" + "O" + "'").CopyToDataTable();
                DataTable dtInbound = dtBound.Select("FltType='" + "I" + "'").CopyToDataTable();

                //GetApiRequests objApiRequests = new GetApiRequests();
                string RQ_Fare = _getApiRequests.GetFareRuleFareQuoteSSRRequest(Tokenid, Traceid, dtOutbound.Rows[0]["BookingFareID"].ToString() + "," + dtInbound.Rows[0]["BookingFareID"].ToString(), EndUserIp);

               // GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
                string RS_Fare = _apiCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, RQ_Fare, GetApiServiceURL.getfarequote_url, "FareQuote");
                errorMessage = _apiCommonFunctions.errorMessage;

                if (RS_Fare != null && RS_Fare.Length > 0 && RS_Fare.IndexOf("expired") == -1 && RS_Fare.IndexOf("Fare Quote failed from the Supplier end") == -1)
                {
                    SetFareModifiers objFareModifiers = new SetFareModifiers(Searchid, Companyid);
                    objFareModifiers.Filter(RS_Fare, dtOutbound, true);
                    objFareModifiers.Filter(RS_Fare, dtInbound, true);
                    dtOutbound.Merge(dtInbound);

                    //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                    dtOutbound = _rr_layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);

                    GetApiCommonFunctions.Set_Journey_Duration_TimeDetail(dtOutbound);

                    return GetCommonFunctions.DataTableToString(dtOutbound.Copy(), "AvailabilityInfo", "root");
                }
                else
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        dr["IsPriceChanged"] = false;
                        dr["FareStatus"] = false;
                        dr["FareQuote"] = "";
                    }
                    dtBound.AcceptChanges();

                    return GetCommonFunctions.DataTableToString(dtBound.Copy(), "AvailabilityInfo", "root");
                }                
            }
            else
            {
                GetApiRequests objApiRequests = new GetApiRequests();
                string RQ_Fare = objApiRequests.GetFareRuleFareQuoteSSRRequest(Tokenid, Traceid, dtBound.Rows[0]["BookingFareID"].ToString(), EndUserIp);

                GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
                string RS_Fare = objCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, RQ_Fare, GetApiServiceURL.getfarequote_url, "FareQuote");
                errorMessage = objCommonFunctions.errorMessage;

                if (RS_Fare != null && RS_Fare.Length > 0 && RS_Fare.IndexOf("expired") == -1 && RS_Fare.IndexOf("Fare Quote failed from the Supplier end") == -1)
                {
                    SetFareModifiers objFareModifiers = new SetFareModifiers(Searchid, Companyid);
                    objFareModifiers.Filter(RS_Fare, dtBound, false);

                    //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                    dtBound = _rr_layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);
                }
                else
                {
                    foreach (DataRow drSelect in dtBound.Rows)
                    {
                        drSelect["IsPriceChanged"] = false;
                        drSelect["FareStatus"] = false;
                        drSelect["FareQuote"] = "";
                    }
                    dtBound.AcceptChanges();
                }

                GetApiCommonFunctions.Set_Journey_Duration_TimeDetail(dtBound);
                return GetCommonFunctions.DataTableToString(dtBound.Copy(), "AvailabilityInfo", "root");
            }
        }
    }
}
