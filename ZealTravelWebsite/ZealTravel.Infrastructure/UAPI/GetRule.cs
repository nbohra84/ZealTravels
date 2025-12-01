using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Xml;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetRule
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        private string SearchID;
        private string CompanyID;
        public GetRule(string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string SearchID, string CompanyID)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.UserName = UserName;
            this.Password = Password;

            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
        }

        public string GetRuleResponse(DataTable dtBound)
        {
            string FareRule = "";
            try
            {
                string Request1 = GetRuleRequest1(dtBound);
                string Request2 = GetRuleRequest2(dtBound);

                CommonUapi cs = new CommonUapi();
                string response1 = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, Request1, "AirService", "Rules1");
                string response2 = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, Request2, "AirService", "Rules2");

                DBCommon.Logger.WriteLogg(CompanyID, 0, "FareRule", "FARE", Request2 + Environment.NewLine + response2, "", SearchID);

                string FareRuleText = "";
                DataSet dsRequest2 = new DataSet();
                dsRequest2.ReadXml(new System.IO.StringReader(response2));
                foreach (DataRow dr in dsRequest2.Tables["FareRuleLong"].Rows)
                {
                    FareRuleText += "<p>" + dr["FareRuleLong_Text"].ToString() + "</p>";
                }

                //DataSet dsRequest1 = new DataSet();
                //dsRequest1.ReadXml(new System.IO.StringReader(response1));
                //ArrayList ArCategories = DBCommon.CommonFunction.DataTable2ArrayList(dsRequest1.Tables["CategoryDetails"], "FareRuleCategoryType_Id", true);
                //if (ArCategories != null && ArCategories.Count > 0)
                //{
                //    string Heading = "";
                //    string Value = "";
                //    for (int i = 0; i < ArCategories.Count; i++)
                //    {
                //        DataRow[] drSelectRows = dsRequest1.Tables["CategoryDetails"].Select("FareRuleCategoryType_Id='" + ArCategories[i].ToString() + "'");
                //        foreach (DataRow dr in drSelectRows.CopyToDataTable().Rows)
                //        {
                //            if (dr["Value"].ToString().Equals("X"))
                //            {
                //                Heading += dr["Name"].ToString() + "|";
                //            }
                //            if (dr["Name"].ToString().Equals("Amt1"))
                //            {
                //                int k = 0;
                //                int.TryParse(dr["Value"].ToString(), out k);
                //                if (k > 0)
                //                {
                //                    Value = k.ToString();
                //                }
                //            }
                //        }

                //        if (Heading.IndexOf("|") != -1 && Value.Length > 0)
                //        {
                //            Heading = Heading.Substring(0, Heading.Length - 1);
                //            FareRuleText += "<p>" + Heading + "-" + Value + "</p>";
                //        }

                //        Heading = "";
                //        Value = "";
                //    }
                //}

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
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetRuleResponse", "air_uapi", "", SearchID, ex.Message);
            }
            return FareRule;
        }

        public async System.Threading.Tasks.Task<string> GetRuleResponseAsync(DataTable dtBound)
        {
            string FareRule = "";
            try
            {
                string Request1 = GetRuleRequest1(dtBound);
                string Request2 = GetRuleRequest2(dtBound);

                CommonUapi cs = new CommonUapi();
                string response1 =await cs.GetResponseUapiAsync(NetworkUserName, NetworkPassword, SearchID, Request1, "AirService", "Rules1");
                string response2 =await cs.GetResponseUapiAsync(NetworkUserName, NetworkPassword, SearchID, Request2, "AirService", "Rules2");

                DBCommon.Logger.WriteLogg(CompanyID, 0, "FareRule", "FARE", Request2 + Environment.NewLine + response2, "", SearchID);

                string FareRuleText = "";
                DataSet dsRequest2 = new DataSet();
                dsRequest2.ReadXml(new System.IO.StringReader(response2));
                foreach (DataRow dr in dsRequest2.Tables["FareRuleLong"].Rows)
                {
                    FareRuleText += "<p>" + dr["FareRuleLong_Text"].ToString() + "</p>";
                }

                //DataSet dsRequest1 = new DataSet();
                //dsRequest1.ReadXml(new System.IO.StringReader(response1));
                //ArrayList ArCategories = DBCommon.CommonFunction.DataTable2ArrayList(dsRequest1.Tables["CategoryDetails"], "FareRuleCategoryType_Id", true);
                //if (ArCategories != null && ArCategories.Count > 0)
                //{
                //    string Heading = "";
                //    string Value = "";
                //    for (int i = 0; i < ArCategories.Count; i++)
                //    {
                //        DataRow[] drSelectRows = dsRequest1.Tables["CategoryDetails"].Select("FareRuleCategoryType_Id='" + ArCategories[i].ToString() + "'");
                //        foreach (DataRow dr in drSelectRows.CopyToDataTable().Rows)
                //        {
                //            if (dr["Value"].ToString().Equals("X"))
                //            {
                //                Heading += dr["Name"].ToString() + "|";
                //            }
                //            if (dr["Name"].ToString().Equals("Amt1"))
                //            {
                //                int k = 0;
                //                int.TryParse(dr["Value"].ToString(), out k);
                //                if (k > 0)
                //                {
                //                    Value = k.ToString();
                //                }
                //            }
                //        }

                //        if (Heading.IndexOf("|") != -1 && Value.Length > 0)
                //        {
                //            Heading = Heading.Substring(0, Heading.Length - 1);
                //            FareRuleText += "<p>" + Heading + "-" + Value + "</p>";
                //        }

                //        Heading = "";
                //        Value = "";
                //    }
                //}

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
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetRuleResponse", "air_uapi", "", SearchID, ex.Message);
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
        private string GetRuleRequest2(DataTable dtSelect)
        {
            string Request = "";
            Request += @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soap:Body>";

            foreach (DataRow dr in dtSelect.Rows)
            {
                Request += @"<AirFareRulesReq xmlns=""http://www.travelport.com/schema/air_v51_0""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<BillingPointOfSaleInfo xmlns=""http://www.travelport.com/schema/common_v51_0"" OriginApplication=""uAPI""/>";
                Request += @"<FareRuleKey FareInfoRef=" + "\"" + dr["AdtFareInfoRef"].ToString() + "\"" + " ProviderCode=" + "\"" + dr["ProviderCode"].ToString() + "\"" + ">" + "\"" + dr["FareRuleInfo_Text"].ToString() + "\"" + "</FareRuleKey></AirFareRulesReq>";
            }

            Request += @"</soap:Body>";
            Request += @"</soap:Envelope>";

            return Request;
        }
    }
}
