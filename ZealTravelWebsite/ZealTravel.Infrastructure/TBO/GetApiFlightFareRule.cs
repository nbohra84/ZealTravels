using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetApiFlightFareRule
    {
        public string errorMessage;
        private string JourneyType;
        private string Searchid;
        private string Companyid;
        private string EndUserIp;
        public GetApiFlightFareRule(string JourneyType, string Searchid, string Companyid, string EndUserIp)
        {
            this.JourneyType = JourneyType;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.EndUserIp = EndUserIp;
        }
        //-----------------------------------------------------------------------------------------------
        public string GetFareRule(DataTable dtBound)
        {
            string Tokenid = dtBound.Rows[0]["Api_SessionID"].ToString().Trim().ToUpper();
            string Traceid = dtBound.Rows[0]["JourneySellKey"].ToString().Trim().ToUpper();
            string Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim().ToUpper();

            if (JourneyType.Equals("RTLCC"))
            {
                DataRow[] drOutbound = dtBound.Select("FltType='" + "O" + "'");
                DataRow[] drInbound = dtBound.Select("FltType='" + "I" + "'");

                GetApiRequests objApiRequests = new GetApiRequests();
                string RQ_FareRule = objApiRequests.GetFareRuleFareQuoteSSRRequest(Tokenid, Traceid, drOutbound.CopyToDataTable().Rows[0]["BookingFareID"].ToString() + "," + drInbound.CopyToDataTable().Rows[0]["BookingFareID"].ToString(), EndUserIp);

                GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
                string RS_FareRule = objCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, RQ_FareRule, GetApiServiceURL.getfarerule_url, "FareRule");
                errorMessage = objCommonFunctions.errorMessage;

                if (RS_FareRule != null && RS_FareRule.Length > 0 && RS_FareRule.IndexOf("expired") == -1 && RS_FareRule.IndexOf("Fare Quote failed from the Supplier end") == -1)
                {
                    DataTable dtFareRule = DBCommon.Schema.SchemaFareRule;

                    DataSet dsFareRule = new DataSet();
                    dsFareRule.ReadXml(new System.IO.StringReader(RS_FareRule));

                    if (dsFareRule.Tables["FareRules"] != null)
                    {
                        foreach (DataRow dr in dsFareRule.Tables["FareRules"].Rows)
                        {
                            string Origin = drOutbound.CopyToDataTable().Rows[0]["Origin"].ToString().Trim().ToUpper();
                            string Destination = drOutbound.CopyToDataTable().Rows[0]["Destination"].ToString().Trim().ToUpper();

                            if (dr["Origin"].ToString().Trim().Equals(Origin))
                            {
                                DataRow drAdd = dtFareRule.NewRow();
                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;
                                drAdd["RuleDetail"] = dr["FareRuleDetail"].ToString();
                                dtFareRule.Rows.Add(drAdd);
                            }
                            else if (dr["Origin"].ToString().Trim().Equals(Destination))
                            {
                                DataRow drAdd = dtFareRule.NewRow();
                                drAdd["Origin"] = Destination;
                                drAdd["Destination"] = Origin;
                                drAdd["RuleDetail"] = dr["FareRuleDetail"].ToString();
                                dtFareRule.Rows.Add(drAdd);
                            }
                            else
                            {
                                if (dr["Destination"].ToString().Trim().Equals(Origin))
                                {
                                    DataRow drAdd = dtFareRule.NewRow();
                                    drAdd["Origin"] = Destination;
                                    drAdd["Destination"] = Origin;
                                    drAdd["RuleDetail"] = dr["FareRuleDetail"].ToString();
                                    dtFareRule.Rows.Add(drAdd);
                                }
                                else if (dr["Destination"].ToString().Trim().Equals(Destination))
                                {
                                    DataRow drAdd = dtFareRule.NewRow();
                                    drAdd["Origin"] = Origin;
                                    drAdd["Destination"] = Destination;
                                    drAdd["RuleDetail"] = dr["FareRuleDetail"].ToString();
                                    dtFareRule.Rows.Add(drAdd);
                                }
                            }
                        }

                        return GetCommonFunctions.DataTableToString(dtFareRule, "FareRuleInfo", "root");
                    }
                }
            }
            else
            {
                GetApiRequests objApiRequests = new GetApiRequests();
                string RQ_FareRule = objApiRequests.GetFareRuleFareQuoteSSRRequest(Tokenid, Traceid, dtBound.Rows[0]["BookingFareID"].ToString(), EndUserIp);

                GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
                string RS_FareRule = objCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, RQ_FareRule, GetApiServiceURL.getfarerule_url, "FareRule");
                errorMessage = objCommonFunctions.errorMessage;

                if (RS_FareRule != null && RS_FareRule.Length > 0 && RS_FareRule.IndexOf("expired") == -1 && RS_FareRule.IndexOf("Fare Quote failed from the Supplier end") == -1)
                {
                    DataTable dtFareRule = DBCommon.Schema.SchemaFareRule;

                    DataSet dsFareRule = new DataSet();
                    dsFareRule.ReadXml(new System.IO.StringReader(RS_FareRule));

                    if (dsFareRule.Tables["FareRules"] != null)
                    {
                        foreach (DataRow dr in dsFareRule.Tables["FareRules"].Rows)
                        {
                            DataRow drAdd = dtFareRule.NewRow();
                            drAdd["Origin"] = dr["Origin"].ToString();
                            drAdd["Destination"] = dr["Destination"].ToString();
                            drAdd["RuleDetail"] = dr["FareRuleDetail"].ToString();
                            dtFareRule.Rows.Add(drAdd);
                        }
                    }

                    return GetCommonFunctions.DataTableToString(dtFareRule, "FareRuleInfo", "root");
                }
            }
            return "";
        }
    }
}
