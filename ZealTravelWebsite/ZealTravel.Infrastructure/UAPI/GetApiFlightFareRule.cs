using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetApiFlightFareRule
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;

        private string SearchID;
        private string Companyid;
        public GetApiFlightFareRule(string NetworkUserName, string NetworkPassword, string TargetBranch, string SearchID, string Companyid)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;

            this.SearchID = SearchID;
            this.Companyid = Companyid;
        }

        public string GetRuleResponse(DataTable dtBound)
        {
            string FareRule = "";

            try
            {
                string Request1 = GetRuleRequest1(dtBound);
                string Request2 = GetRuleRequest2(SearchID, dtBound);

                CommonUapi cs = new CommonUapi();
                string response1 = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, Request1, "AirService", "Rules1");
                string response2 = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, Request2, "AirService", "Rules2");

                DBCommon.Logger.WriteLogg(Companyid, 0, "FareRule", "FARE", Request2 + Environment.NewLine + response2, "", SearchID);

                string FareRuleText = "";
                DataSet dsRequest2 = new DataSet();
                dsRequest2.ReadXml(new System.IO.StringReader(response2));
                foreach (DataRow dr in dsRequest2.Tables["FareRuleLong"].Rows)
                {
                    FareRuleText += "<p>" + dr["FareRuleLong_Text"].ToString() + "</p>";
                }

                DataTable dtFareRule = DBCommon.Schema.SchemaFareRule;
                DataRow drAdd = dtFareRule.NewRow();
                drAdd["Origin"] = dtBound.Rows[0]["Origin"].ToString();
                drAdd["Destination"] = dtBound.Rows[0]["Destination"].ToString();
                drAdd["RuleDetail"] = FareRuleText;
                dtFareRule.Rows.Add(drAdd);
                FareRule = DBCommon.CommonFunction.DataTableToString(dtFareRule, "FareRuleInfo", "root");
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "GetRuleResponse", "air_uapi", "", SearchID, ex.Message);
            }
            return FareRule;
        }
        private string GetRuleRequest1(DataTable dtSelect)
        {
            string Request = "";
            Request += @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soap:Body>";

            foreach (DataRow dr in dtSelect.Rows)
            {
                Request += @"<air:AirFareRulesReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<com:BillingPointOfSaleInfo OriginApplication=""uAPI""/>";
                Request += @"<air:FareRuleKey FareInfoRef=""FINFO"" ProviderCode=""XX"">KKKKKKKK</air:FareRuleKey>";
                Request += @"<air:FareRulesFilterCategory>";
                Request += @"<air:CategoryCode>CHG</air:CategoryCode>";
                Request += @"<air:CategoryCode>VOL</air:CategoryCode>";
                Request += @"</air:FareRulesFilterCategory>";
                Request += @"</air:AirFareRulesReq>";

                if (dr["CarrierCode"].ToString().Equals("6E"))
                {
                    Request = Request.Replace("XX", "ACH").Trim();
                }
                else
                {
                    Request = Request.Replace("XX", "1G").Trim();
                }

                Request = Request.Replace("FINFO", dr["AdtFareInfoRef"].ToString()).Trim();
                Request = Request.Replace("KKKKKKKK", dr["FareRuleInfo_Text"].ToString()).Trim();
            }

            Request += @"</soap:Body>";
            Request += @"</soap:Envelope>";

            return Request;
        }
        //private string GetRuleRequest2(string SearchID, DataTable dtSelect)
        //{
        //    string Request = "";
        //    Request += @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
        //    Request += @"<soap:Body>";
        //    string FareInfoRef = dtSelect.Rows[0]["AdtFareInfoRef"].ToString();
        //    string[] split = FareInfoRef.Split('?');
        //    for (int i = 0; i < dtSelect.Rows.Count; i++)
        //    {
        //        DataRow dr = dtSelect.Rows[i];
        //        Request += @"<AirFareRulesReq xmlns=""http://www.travelport.com/schema/air_v51_0""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
        //        Request += @"<BillingPointOfSaleInfo xmlns=""http://www.travelport.com/schema/common_v51_0"" OriginApplication=""uAPI""/>";
        //        Request += @"<FareRuleKey FareInfoRef=""FINFO"" ProviderCode=" + "\"" + dr["ProviderCode"].ToString() + "\"" + ">" + "\"" + dr["FareRuleInfo_Text"].ToString() + "\"" + "</FareRuleKey></AirFareRulesReq>";

        //        if (dtSelect.Rows.Count.Equals(split.Length))
        //        {
        //            Request = Request.Replace("FINFO", split[i].ToString()).Trim();
        //        }
        //        else
        //        {
        //            Request = Request.Replace("FINFO", split[0].ToString()).Trim();
        //        }
        //        break;
        //    }

        //    Request += @"</soap:Body>";
        //    Request += @"</soap:Envelope>";

        //    return Request;
        //}
        private string GetRuleRequest2(string SearchID, DataTable dtSelect)
        {
            string Request = "";
            Request += @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soap:Body>";

            string[] splitAdtFareInfoRef = dtSelect.Rows[0]["AdtFareInfoRef"].ToString().Trim().Split('?');



            for (int i = 0; i < dtSelect.Rows.Count; i++)
            {
                DataRow dr = dtSelect.Rows[i];               

                Request += @"<AirFareRulesReq xmlns=""http://www.travelport.com/schema/air_v51_0""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<BillingPointOfSaleInfo xmlns=""http://www.travelport.com/schema/common_v51_0"" OriginApplication=""uAPI""/>";
                Request += @"<FareRuleKey FareInfoRef=" + "\"" + splitAdtFareInfoRef[i].ToString().Trim() + "\"" + " ProviderCode=" + "\"" + dr["ProviderCode"].ToString() + "\"" + ">" + dr["FareRuleInfo_Text"].ToString() + "</FareRuleKey></AirFareRulesReq>";
                break;
            }

            Request += @"</soap:Body>";
            Request += @"</soap:Envelope>";

            return Request;
        }
    }
}
