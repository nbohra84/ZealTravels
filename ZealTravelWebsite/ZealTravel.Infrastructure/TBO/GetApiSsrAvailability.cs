using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ZealTravel.Infrastructure.TBO
{
    class GetApiSsrAvailability
    {
        public string errorMessage;
        private string JourneyType;
        private string Searchid;
        private string Companyid;
        private string EndUserIp;
        public GetApiSsrAvailability(string JourneyType, string Searchid, string Companyid, string EndUserIp)
        {
            this.JourneyType = JourneyType;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.EndUserIp = EndUserIp;
        }
        //-----------------------------------------------------------------------------------------------
        public string GetSSR(DataTable dtBound)
        {
            string Tokenid = dtBound.Rows[0]["Api_SessionID"].ToString().Trim().ToUpper();
            string Traceid = dtBound.Rows[0]["JourneySellKey"].ToString().Trim().ToUpper();
            string Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim().ToUpper();

            string FltType = dtBound.Rows[0]["FltType"].ToString().Trim().ToUpper();
            string Origin = dtBound.Rows[0]["Origin"].ToString().Trim().ToUpper();
            string Destination = dtBound.Rows[0]["Destination"].ToString().Trim().ToUpper();
            string CarrierCode = dtBound.Rows[0]["CarrierCode"].ToString().Trim().ToUpper();
            string Sector = dtBound.Rows[0]["Sector"].ToString().Trim().ToUpper();

            if (JourneyType.Equals("RTLCC"))
            {
                DataTable dtOutbound = dtBound.Select("FltType='" + "O" + "'").CopyToDataTable();
                DataTable dtInbound = dtBound.Select("FltType='" + "I" + "'").CopyToDataTable();

                GetApiRequests objApiRequests = new GetApiRequests();
                string RQ_SSR = objApiRequests.GetFareRuleFareQuoteSSRRequest(Tokenid, Traceid, dtOutbound.Rows[0]["BookingFareID"].ToString(), EndUserIp);

                GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
                string RS_SSR = objCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, RQ_SSR, GetApiServiceURL.getssr_url, "SSR");
                errorMessage = objCommonFunctions.errorMessage;

                if (RS_SSR != null && RS_SSR.Length > 0 && RS_SSR.IndexOf("expired") == -1 && RS_SSR.IndexOf("failed from the Supplier end") == -1)
                {
                    SetSSRModifier objSSRModifier = new SetSSRModifier(Searchid, Companyid);
                    DataTable dtAddOn = objSSRModifier.GetSSRModifierRTLCC(RS_SSR, Origin, Destination, FltType, false, CarrierCode);

                    if (dtAddOn != null && dtAddOn.Rows.Count > 0)
                    {
                        return GetCommonFunctions.DataTableToString(dtAddOn, "SSRInfo", "root");
                    }
                }
            }
            else
            {
                GetApiRequests objApiRequests = new GetApiRequests();
                string RQ_SSR = objApiRequests.GetFareRuleFareQuoteSSRRequest(Tokenid, Traceid, dtBound.Rows[0]["BookingFareID"].ToString(), EndUserIp);

                GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
                string RS_SSR = objCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, RQ_SSR, GetApiServiceURL.getssr_url, "SSR");
                errorMessage = objCommonFunctions.errorMessage;

                if (RS_SSR != null && RS_SSR.Length > 0 && RS_SSR.IndexOf("expired") == -1 && RS_SSR.IndexOf("failed from the Supplier end") == -1)
                {
                    bool IsCombi = false;
                    if (Sector.Equals("I") && JourneyType.Equals("RT"))
                    {
                        IsCombi = true;
                    }

                    SetSSRModifier objSSRModifier = new SetSSRModifier(Searchid, Companyid);
                    DataTable dtAddOn = objSSRModifier.GetSSRModifier(RS_SSR, Origin, Destination, FltType, IsCombi, CarrierCode);

                    if (dtAddOn != null && dtAddOn.Rows.Count > 0)
                    {
                        return GetCommonFunctions.DataTableToString(dtAddOn, "SSRInfo", "root");
                    }
                }
            }

            return "";
        }
    }
}
