using Newtonsoft.Json;
using System.Data;
using System.Xml;
using ZealTravel.Common;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class FlightUpdateAvailabilityHelper
    {
        public static string UpdateAvailabilityFareRule(string SearchID, string CompanyID, string TResponse, string RefID, string FltType, string FareRule)
        {
            try
            {
                DataSet dsFareRule = new DataSet();
                if (FareRule.IndexOf("FareRuleInfo") != -1)
                {
                    dsFareRule.ReadXml(new System.IO.StringReader(FareRule));
                }

                DataSet dsAvailability = new DataSet();
                dsAvailability.ReadXml(new System.IO.StringReader(TResponse));

                DataRow[] rows;
                if (FltType.Equals(""))
                {
                    rows = dsAvailability.Tables[0].Select("RefID='" + RefID + "'");
                }
                else
                {
                    rows = dsAvailability.Tables[0].Select("RefID='" + RefID + "' And FltType='" + FltType + "'");
                }

                if (rows.Length > 0 && dsFareRule != null && dsFareRule.Tables["FareRuleInfo"] != null)
                {
                    foreach (DataRow dr in rows)
                    {
                        string strFareRule = string.Empty;
                        DataRow[] drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["Origin"].ToString() + "' And Destination='" + dr["Destination"].ToString() + "'");
                        if (drFareRule.Length > 0)
                        {
                            strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                        }
                        else
                        {
                            drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["DepartureStation"].ToString() + "' And Destination='" + dr["ArrivalStation"].ToString() + "'");
                            if (drFareRule.Length > 0)
                            {
                                strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                            }

                            if (dsFareRule.Tables["FareRuleInfo"].Rows.Count.Equals(1) && strFareRule.Trim().Length.Equals(0))
                            {
                                strFareRule = dsFareRule.Tables["FareRuleInfo"].Rows[0]["RuleDetail"].ToString();
                            }
                        }

                        if (strFareRule.Length > 1)
                        {
                            dr["FareRule"] = strFareRule;
                        }


                        dr.AcceptChanges();
                        dsAvailability.Tables[0].AcceptChanges();
                        dsAvailability.AcceptChanges();
                    }
                }
                TResponse = CommonFunction.DataSetToString(dsAvailability);
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "UpdateAvailability_FareRule", "clsFareUpdate", FareRule, SearchID, ex.Message);
            }

            return TResponse;
        }
        public static string UpdateAvailability(string SearchID, string CompanyID, string TResponse, string TSelectedResponse, string FltType, string FareRule)
        {
            try
            {
                DataSet dsFareRule = new DataSet();
                if (FareRule.IndexOf("FareRuleInfo") != -1)
                {
                    dsFareRule.ReadXml(new System.IO.StringReader(FareRule));
                }

                DataSet dsAvailability = new DataSet();
                dsAvailability.ReadXml(new System.IO.StringReader(TResponse));

                DataSet dsSelectedAvailability = new DataSet();
                dsSelectedAvailability.ReadXml(new System.IO.StringReader(TSelectedResponse));

                if (dsSelectedAvailability.Tables["SSRInfo"] != null)
                {
                    if (HttpContextHelper.Current?.Session.GetString("SSRInfo") != null)
                    {
                        string serializedDataTable = HttpContextHelper.Current?.Session.GetString("SSRInfo");
                        DataTable dtSSRInfo1 = null;

                        if (!string.IsNullOrEmpty(serializedDataTable))
                        {
                            dtSSRInfo1 = JsonConvert.DeserializeObject<DataTable>(serializedDataTable);
                        }
                        dtSSRInfo1.Merge(dsSelectedAvailability.Tables["SSRInfo"].Copy());
                        serializedDataTable = JsonConvert.SerializeObject(dtSSRInfo1);
                        HttpContextHelper.Current.Session.SetString("SSRInfo", serializedDataTable);
                    }
                    else
                    {
                        var serializedDataTable = JsonConvert.SerializeObject(dsSelectedAvailability.Tables["SSRInfo"].Copy());
                        HttpContextHelper.Current?.Session.SetString("SSRInfo", serializedDataTable);
                    }
                }
                else
                {
                    HttpContextHelper.Current.Session.Remove("SSRInfo");
                }

                DataRow[] drGet = dsSelectedAvailability.Tables["AvailabilityInfo"].Select("FltType='" + FltType + "'");
                if (drGet.Length > 0)
                {
                    string RefID = drGet.CopyToDataTable().Rows[0]["RefID"].ToString().Trim();
                    Int32 Chd = Convert.ToInt32(drGet.CopyToDataTable().Rows[0]["Chd"].ToString().Trim());
                    Int32 Inf = Convert.ToInt32(drGet.CopyToDataTable().Rows[0]["Inf"].ToString().Trim());

                    DataRow[] rows = dsAvailability.Tables[0].Select("RefID='" + RefID + "' And FltType='" + FltType + "'");
                    if (rows.Length > 0)
                    {
                        DataTable dtff = rows.CopyToDataTable();
                        foreach (DataRow dr in rows)
                        {
                            DataRow[] drSelect = drGet.CopyToDataTable().Select("RowID='" + dr["RowID"].ToString().Trim() + "'");
                            if (drSelect.Length > 0)
                            {
                                if (drSelect.CopyToDataTable().Rows[0]["DepartureStation"].ToString().Equals(dr["DepartureStation"].ToString()) && drSelect.CopyToDataTable().Rows[0]["ArrivalStation"].ToString().Equals(dr["ArrivalStation"].ToString()))
                                {
                                    string strFareRule = string.Empty;
                                    if (dsFareRule != null && dsFareRule.Tables["FareRuleInfo"] != null)
                                    {
                                        DataRow[] drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["Origin"].ToString() + "' And Destination='" + dr["Destination"].ToString() + "'");
                                        if (drFareRule.Length > 0)
                                        {
                                            strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                                        }
                                        else
                                        {
                                            drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["DepartureStation"].ToString() + "' And Destination='" + dr["ArrivalStation"].ToString() + "'");
                                            if (drFareRule.Length > 0)
                                            {
                                                strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                                            }

                                            if (dsFareRule.Tables["FareRuleInfo"].Rows.Count.Equals(1) && strFareRule.Trim().Length.Equals(0))
                                            {
                                                strFareRule = dsFareRule.Tables["FareRuleInfo"].Rows[0]["RuleDetail"].ToString();
                                            }
                                        }
                                    }

                                    if (strFareRule.Length > 1)
                                    {
                                        dr["FareRule"] = strFareRule;
                                    }

                                    //if (dr["CarrierCode"].ToString().Equals("QP") && dr["AirlineID"].ToString().Equals("QPMAA8752B"))
                                    //{
                                    //    dr["FareRule"] = GetQPFareRule(dr["Origin"].ToString(), dr["Destination"].ToString(), TSelectedResponse);
                                    //}

                                    dr["FareStatus"] = drSelect.CopyToDataTable().Rows[0]["FareStatus"];
                                    dr["IsPriceChanged"] = drSelect.CopyToDataTable().Rows[0]["IsPriceChanged"];

                                    if (drSelect.CopyToDataTable().Rows[0]["RuleTarrif"].ToString().Trim().Length > 0)
                                    {
                                        dr["RuleTarrif"] = drSelect.CopyToDataTable().Rows[0]["RuleTarrif"];
                                    }
                                    dr["RefundType"] = drSelect.CopyToDataTable().Rows[0]["RefundType"];

                                    dr["DepDate"] = drSelect.CopyToDataTable().Rows[0]["DepDate"];
                                    dr["ArrDate"] = drSelect.CopyToDataTable().Rows[0]["ArrDate"];
                                    dr["DepTime"] = drSelect.CopyToDataTable().Rows[0]["DepTime"];
                                    dr["ArrTime"] = drSelect.CopyToDataTable().Rows[0]["ArrTime"];
                                    dr["JourneyTime"] = drSelect.CopyToDataTable().Rows[0]["JourneyTime"];
                                    dr["Duration"] = drSelect.CopyToDataTable().Rows[0]["Duration"];


                                    dr["API_BookingFareID"] = drSelect.CopyToDataTable().Rows[0]["API_BookingFareID"];
                                    dr["Api_SessionID"] = drSelect.CopyToDataTable().Rows[0]["Api_SessionID"];
                                    dr["JourneySellKey"] = drSelect.CopyToDataTable().Rows[0]["JourneySellKey"];
                                    dr["FareBasisCode"] = drSelect.CopyToDataTable().Rows[0]["FareBasisCode"];
                                    dr["Classes"] = drSelect.CopyToDataTable().Rows[0]["Classes"];
                                    dr["ViaName"] = drSelect.CopyToDataTable().Rows[0]["ViaName"];
                                    dr["FareSellKey"] = drSelect.CopyToDataTable().Rows[0]["FareSellKey"];
                                    dr["BookingFareID"] = drSelect.CopyToDataTable().Rows[0]["BookingFareID"];
                                    dr["API_SearchID"] = drSelect.CopyToDataTable().Rows[0]["API_SearchID"];
                                    dr["ProductClass"] = drSelect.CopyToDataTable().Rows[0]["ProductClass"];
                                    dr["IsPriceChanged"] = drSelect.CopyToDataTable().Rows[0]["IsPriceChanged"];
                                    dr["API_AirlineID"] = drSelect.CopyToDataTable().Rows[0]["API_AirlineID"];
                                    dr["TempData1"] = drSelect.CopyToDataTable().Rows[0]["TempData1"];
                                    dr["TempData2"] = drSelect.CopyToDataTable().Rows[0]["TempData2"];
                                    dr["TempData3"] = drSelect.CopyToDataTable().Rows[0]["TempData3"];
                                    dr["FlightID"] = drSelect.CopyToDataTable().Rows[0]["FlightID"];
                                    dr["Via"] = drSelect.CopyToDataTable().Rows[0]["Via"];
                                    dr["SeatsAvailable"] = drSelect.CopyToDataTable().Rows[0]["SeatsAvailable"];
                                    dr["RuleNumber"] = drSelect.CopyToDataTable().Rows[0]["RuleNumber"];
                                    dr["BaggageDetail"] = drSelect.CopyToDataTable().Rows[0]["BaggageDetail"];
                                    dr["API_RefID"] = drSelect.CopyToDataTable().Rows[0]["API_RefID"];

                                    dr["JourneyTimeDesc"] = drSelect.CopyToDataTable().Rows[0]["JourneyTimeDesc"];
                                    dr["DurationDesc"] = drSelect.CopyToDataTable().Rows[0]["DurationDesc"];
                                    dr["DepartureDate"] = drSelect.CopyToDataTable().Rows[0]["DepartureDate"];
                                    dr["ArrivalDate"] = drSelect.CopyToDataTable().Rows[0]["ArrivalDate"];
                                    dr["DepartureTime"] = drSelect.CopyToDataTable().Rows[0]["DepartureTime"];
                                    dr["ArrivalTime"] = drSelect.CopyToDataTable().Rows[0]["ArrivalTime"];

                                    dr["ClassOfService"] = drSelect.CopyToDataTable().Rows[0]["ClassOfService"];
                                    dr["FareBasisCode"] = drSelect.CopyToDataTable().Rows[0]["FareBasisCode"];
                                    dr["EquipmentType"] = drSelect.CopyToDataTable().Rows[0]["EquipmentType"];

                                    dr["DepartureTerminal"] = drSelect.CopyToDataTable().Rows[0]["DepartureTerminal"];
                                    if (drSelect.CopyToDataTable().Columns.Contains("ArrivalTerminal"))
                                    {
                                        dr["ArrivalTerminal"] = drSelect.CopyToDataTable().Rows[0]["ArrivalTerminal"];
                                    }

                                    if (drSelect.CopyToDataTable().Columns.Contains("AG_Markup"))
                                    {
                                        dr["AG_Markup"] = drSelect.CopyToDataTable().Rows[0]["AG_Markup"];
                                    }


                                    dr["AdtTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalBasic"];
                                    dr["AdtTotalTax"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalTax"];
                                    dr["AdtTotalFare"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalFare"];

                                    dr["Adt_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Adt_BASIC"];
                                    dr["Adt_YQ"] = drSelect.CopyToDataTable().Rows[0]["Adt_YQ"];
                                    dr["Adt_CUTE"] = drSelect.CopyToDataTable().Rows[0]["Adt_CUTE"];
                                    dr["Adt_PSF"] = drSelect.CopyToDataTable().Rows[0]["Adt_PSF"];
                                    dr["Adt_UDF"] = drSelect.CopyToDataTable().Rows[0]["Adt_UDF"];
                                    dr["Adt_GST"] = drSelect.CopyToDataTable().Rows[0]["Adt_GST"];
                                    dr["Adt_AUDF"] = drSelect.CopyToDataTable().Rows[0]["Adt_AUDF"];
                                    dr["Adt_TF"] = drSelect.CopyToDataTable().Rows[0]["Adt_TF"];
                                    dr["Adt_EX"] = drSelect.CopyToDataTable().Rows[0]["Adt_EX"];

                                    dr["Adt_BAS"] = drSelect.CopyToDataTable().Rows[0]["Adt_BAS"];
                                    dr["Adt_Y"] = drSelect.CopyToDataTable().Rows[0]["Adt_Y"];
                                    dr["Adt_CB"] = drSelect.CopyToDataTable().Rows[0]["Adt_CB"];
                                    dr["Adt_PR"] = drSelect.CopyToDataTable().Rows[0]["Adt_PR"];
                                    dr["Adt_MU"] = drSelect.CopyToDataTable().Rows[0]["Adt_MU"];
                                    dr["Adt_SF"] = drSelect.CopyToDataTable().Rows[0]["Adt_SF"];
                                    dr["Adt_ST"] = drSelect.CopyToDataTable().Rows[0]["Adt_ST"];
                                    dr["Adt_TDS"] = drSelect.CopyToDataTable().Rows[0]["Adt_TDS"];

                                    if (Chd > 0)
                                    {
                                        dr["ChdTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalBasic"];
                                        dr["ChdTotalTax"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalTax"];
                                        dr["ChdTotalFare"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalFare"];

                                        dr["Chd_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Chd_BASIC"];
                                        dr["Chd_YQ"] = drSelect.CopyToDataTable().Rows[0]["Chd_YQ"];
                                        dr["Chd_CUTE"] = drSelect.CopyToDataTable().Rows[0]["Chd_CUTE"];
                                        dr["Chd_PSF"] = drSelect.CopyToDataTable().Rows[0]["Chd_PSF"];
                                        dr["Chd_UDF"] = drSelect.CopyToDataTable().Rows[0]["Chd_UDF"];
                                        dr["Chd_GST"] = drSelect.CopyToDataTable().Rows[0]["Chd_GST"];
                                        dr["Chd_AUDF"] = drSelect.CopyToDataTable().Rows[0]["Chd_AUDF"];
                                        dr["Chd_TF"] = drSelect.CopyToDataTable().Rows[0]["Chd_TF"];
                                        dr["Chd_EX"] = drSelect.CopyToDataTable().Rows[0]["Chd_EX"];

                                        dr["Chd_BAS"] = drSelect.CopyToDataTable().Rows[0]["Chd_BAS"];
                                        dr["Chd_Y"] = drSelect.CopyToDataTable().Rows[0]["Chd_Y"];
                                        dr["Chd_CB"] = drSelect.CopyToDataTable().Rows[0]["Chd_CB"];
                                        dr["Chd_PR"] = drSelect.CopyToDataTable().Rows[0]["Chd_PR"];
                                        dr["Chd_MU"] = drSelect.CopyToDataTable().Rows[0]["Chd_MU"];
                                        dr["Chd_SF"] = drSelect.CopyToDataTable().Rows[0]["Chd_SF"];
                                        dr["Chd_ST"] = drSelect.CopyToDataTable().Rows[0]["Chd_ST"];
                                        dr["Chd_TDS"] = drSelect.CopyToDataTable().Rows[0]["Chd_TDS"];
                                    }

                                    if (Inf > 0)
                                    {
                                        dr["InfTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["InfTotalBasic"];
                                        dr["InfTotalTax"] = drSelect.CopyToDataTable().Rows[0]["InfTotalTax"];
                                        dr["InfTotalFare"] = drSelect.CopyToDataTable().Rows[0]["InfTotalFare"];

                                        dr["Inf_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Inf_BASIC"];
                                        dr["Inf_TAX"] = drSelect.CopyToDataTable().Rows[0]["Inf_TAX"];
                                    }

                                    dr["TotalServiceTax"] = drSelect.CopyToDataTable().Rows[0]["TotalServiceTax"];
                                    dr["TotalServiceFee"] = drSelect.CopyToDataTable().Rows[0]["TotalServiceFee"];
                                    dr["TotalMarkup"] = drSelect.CopyToDataTable().Rows[0]["TotalMarkup"];
                                    dr["TotalTds"] = drSelect.CopyToDataTable().Rows[0]["TotalTds"];
                                    dr["TotalTds_SA"] = drSelect.CopyToDataTable().Rows[0]["TotalTds_SA"];

                                    dr["TotalCommission"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                                    dr["TotalCommission_SA"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());

                                    dr["TotalBasic"] = drSelect.CopyToDataTable().Rows[0]["TotalBasic"];
                                    dr["TotalTax"] = drSelect.CopyToDataTable().Rows[0]["TotalTax"];
                                    dr["TotalFare"] = drSelect.CopyToDataTable().Rows[0]["TotalFare"];

                                    dr["FareQuote"] = drSelect.CopyToDataTable().Rows[0]["FareQuote"];

                                    dr["Group"] = drSelect.CopyToDataTable().Rows[0]["Group"];
                                    dr["FlightTime"] = drSelect.CopyToDataTable().Rows[0]["FlightTime"];
                                    dr["Distance"] = drSelect.CopyToDataTable().Rows[0]["Distance"];
                                    dr["ChangeOfPlane"] = drSelect.CopyToDataTable().Rows[0]["ChangeOfPlane"];
                                    dr["ParticipantLevel"] = drSelect.CopyToDataTable().Rows[0]["ParticipantLevel"];
                                    dr["LinkAvailability"] = drSelect.CopyToDataTable().Rows[0]["LinkAvailability"];
                                    dr["PolledAvailabilityOption"] = drSelect.CopyToDataTable().Rows[0]["PolledAvailabilityOption"];
                                    dr["OptionalServicesIndicator"] = drSelect.CopyToDataTable().Rows[0]["OptionalServicesIndicator"];
                                    dr["AvailabilitySource"] = drSelect.CopyToDataTable().Rows[0]["AvailabilitySource"];
                                    dr["AvailabilityDisplayType"] = drSelect.CopyToDataTable().Rows[0]["AvailabilityDisplayType"];
                                    dr["CodeshareInfoOperatingCarrier"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfoOperatingCarrier"];
                                    dr["CodeshareInfoOperatingFlightNumber"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfoOperatingFlightNumber"];
                                    dr["CodeshareInfo"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfo"];
                                    dr["TravelTime"] = drSelect.CopyToDataTable().Rows[0]["TravelTime"];
                                    dr["BookingCode"] = drSelect.CopyToDataTable().Rows[0]["BookingCode"];
                                    dr["CabinClass"] = drSelect.CopyToDataTable().Rows[0]["CabinClass"];
                                    dr["LatestTicketingTime"] = drSelect.CopyToDataTable().Rows[0]["LatestTicketingTime"];
                                    dr["PricingMethod"] = drSelect.CopyToDataTable().Rows[0]["PricingMethod"];
                                    dr["Refundable"] = drSelect.CopyToDataTable().Rows[0]["Refundable"];
                                    dr["ETicketability"] = drSelect.CopyToDataTable().Rows[0]["ETicketability"];
                                    dr["PlatingCarrier"] = drSelect.CopyToDataTable().Rows[0]["PlatingCarrier"];
                                    dr["ProviderCode"] = drSelect.CopyToDataTable().Rows[0]["ProviderCode"];
                                    dr["Cat35Indicator"] = drSelect.CopyToDataTable().Rows[0]["Cat35Indicator"];

                                    dr["AirPricePointKey"] = drSelect.CopyToDataTable().Rows[0]["AirPricePointKey"];//single      
                                    dr["SegmentRef"] = drSelect.CopyToDataTable().Rows[0]["SegmentRef"];//single
                                    dr["FlightDetailsRefKey"] = drSelect.CopyToDataTable().Rows[0]["FlightDetailsRefKey"];//single
                                    dr["AdtAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["AdtAirPricingInfoKey"];
                                    dr["ChdAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["ChdAirPricingInfoKey"];
                                    dr["InfAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["InfAirPricingInfoKey"];
                                    dr["AdtFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["AdtFareInfoRef"];
                                    dr["ChdFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["ChdFareInfoRef"];
                                    dr["InfFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["InfFareInfoRef"];
                                    dr["AdtOptionKey"] = drSelect.CopyToDataTable().Rows[0]["AdtOptionKey"];
                                    dr["ChdOptionKey"] = drSelect.CopyToDataTable().Rows[0]["ChdOptionKey"];
                                    dr["InfOptionKey"] = drSelect.CopyToDataTable().Rows[0]["InfOptionKey"];
                                    dr["AdtTaxes"] = drSelect.CopyToDataTable().Rows[0]["AdtTaxes"];
                                    dr["ChdTaxes"] = drSelect.CopyToDataTable().Rows[0]["ChdTaxes"];
                                    dr["InfTaxes"] = drSelect.CopyToDataTable().Rows[0]["InfTaxes"];

                                    dr["AdtHTR"] = drSelect.CopyToDataTable().Rows[0]["AdtHTR"];
                                    dr["ChdHTR"] = drSelect.CopyToDataTable().Rows[0]["ChdHTR"];
                                    dr["InfHTR"] = drSelect.CopyToDataTable().Rows[0]["InfHTR"];

                                    dr["HostTokenRef"] = drSelect.CopyToDataTable().Rows[0]["HostTokenRef"];

                                    dr["FareRuleInfo_Text"] = drSelect.CopyToDataTable().Rows[0]["FareRuleInfo_Text"];

                                    dr["AdtFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["AdtFareInfoRef_data"];
                                    dr["ChdFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["ChdFareInfoRef_data"];
                                    dr["InfFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["InfFareInfoRef_data"];

                                    dr["AdtBTR"] = drSelect.CopyToDataTable().Rows[0]["AdtBTR"];
                                    dr["ChdBTR"] = drSelect.CopyToDataTable().Rows[0]["ChdBTR"];
                                    dr["InfBTR"] = drSelect.CopyToDataTable().Rows[0]["InfBTR"];

                                    dr["Connection_O"] = drSelect.CopyToDataTable().Rows[0]["Connection_O"];
                                    dr["Connection_I"] = drSelect.CopyToDataTable().Rows[0]["Connection_I"];

                                    dr.AcceptChanges();
                                    dsAvailability.Tables[0].AcceptChanges();
                                    dsAvailability.AcceptChanges();
                                }
                            }
                        }
                    }
                }

                TResponse = CommonFunction.DataSetToString(dsAvailability);
            }
            catch (Exception ex)
            {
             //   dbCommon.Logger.dbLogg(CompanyID, 0, "UpdateAvailability", "clsFareUpdate", TSelectedResponse, SearchID, ex.Message);
            }

            return TResponse;
        }
        public static string UpdateAvailabilityCO(string SearchID, string CompanyID, string TResponse, string TSelectedResponse, string FareRule)
        {
            try
            {
                DataSet dsFareRule = new DataSet();
                if (FareRule.IndexOf("FareRuleInfo") != -1)
                {
                    dsFareRule.ReadXml(new System.IO.StringReader(FareRule));
                }

                DataSet dsAvailability = new DataSet();
                dsAvailability.ReadXml(new System.IO.StringReader(TResponse));

                DataSet dsSelectedAvailability = new DataSet();
                dsSelectedAvailability.ReadXml(new System.IO.StringReader(TSelectedResponse));

                if (dsSelectedAvailability.Tables["SSRInfo"] != null)
                {
                   
                    HttpContextHelper.Current?.Session.SetString("SSRInfo", JsonConvert.SerializeObject(dsSelectedAvailability.Tables["SSRInfo"].Copy()));
                }
                else
                {
                    HttpContextHelper.Current?.Session.Remove("SSRInfo");
                }

                string RefID = dsSelectedAvailability.Tables["AvailabilityInfo"].Rows[0]["RefID"].ToString().Trim();
                Int32 Chd = Convert.ToInt32(dsSelectedAvailability.Tables["AvailabilityInfo"].Rows[0]["Chd"].ToString().Trim());
                Int32 Inf = Convert.ToInt32(dsSelectedAvailability.Tables["AvailabilityInfo"].Rows[0]["Inf"].ToString().Trim());

                DataRow[] rows = dsAvailability.Tables[0].Select("RefID='" + RefID + "'");
                if (rows.Length > 0)
                {
                    foreach (DataRow dr in rows)
                    {
                        DataRow[] drSelect = dsSelectedAvailability.Tables["AvailabilityInfo"].Select("RowID='" + dr["RowID"].ToString().Trim() + "'");
                        if (drSelect.Length > 0)
                        {
                            if (drSelect.CopyToDataTable().Rows[0]["DepartureStation"].ToString().Equals(dr["DepartureStation"].ToString()) && drSelect.CopyToDataTable().Rows[0]["ArrivalStation"].ToString().Equals(dr["ArrivalStation"].ToString()))
                            {
                                string strFareRule = string.Empty;
                                if (dsFareRule != null && dsFareRule.Tables["FareRuleInfo"] != null)
                                {
                                    DataRow[] drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["Origin"].ToString() + "' And Destination='" + dr["Destination"].ToString() + "'");
                                    if (drFareRule.Length > 0)
                                    {
                                        strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                                    }
                                    else
                                    {
                                        drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["DepartureStation"].ToString() + "' And Destination='" + dr["ArrivalStation"].ToString() + "'");
                                        if (drFareRule.Length > 0)
                                        {
                                            strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                                        }

                                        if (dsFareRule.Tables["FareRuleInfo"].Rows.Count.Equals(1) && strFareRule.Trim().Length.Equals(0))
                                        {
                                            strFareRule = dsFareRule.Tables["FareRuleInfo"].Rows[0]["RuleDetail"].ToString();
                                        }
                                    }
                                }

                                if (strFareRule.Length > 1)
                                {
                                    dr["FareRule"] = strFareRule;
                                }

                                if (dr["CarrierCode"].ToString().Equals("QP") && dr["AirlineID"].ToString().Equals("QPMAA8752B"))
                                {
                                    dr["FareRule"] = GetQPFareRule(dr["Origin"].ToString(), dr["Destination"].ToString(), TSelectedResponse);
                                }

                                dr["FareStatus"] = drSelect.CopyToDataTable().Rows[0]["FareStatus"];
                                dr["IsPriceChanged"] = drSelect.CopyToDataTable().Rows[0]["IsPriceChanged"];

                                if (drSelect.CopyToDataTable().Rows[0]["RuleTarrif"].ToString().Trim().Length > 0)
                                {
                                    dr["RuleTarrif"] = drSelect.CopyToDataTable().Rows[0]["RuleTarrif"];
                                }
                                dr["RefundType"] = drSelect.CopyToDataTable().Rows[0]["RefundType"];

                                dr["DepDate"] = drSelect.CopyToDataTable().Rows[0]["DepDate"];
                                dr["ArrDate"] = drSelect.CopyToDataTable().Rows[0]["ArrDate"];
                                dr["DepTime"] = drSelect.CopyToDataTable().Rows[0]["DepTime"];
                                dr["ArrTime"] = drSelect.CopyToDataTable().Rows[0]["ArrTime"];
                                dr["JourneyTime"] = drSelect.CopyToDataTable().Rows[0]["JourneyTime"];
                                dr["Duration"] = drSelect.CopyToDataTable().Rows[0]["Duration"];


                                dr["API_BookingFareID"] = drSelect.CopyToDataTable().Rows[0]["API_BookingFareID"];
                                dr["Api_SessionID"] = drSelect.CopyToDataTable().Rows[0]["Api_SessionID"];
                                dr["JourneySellKey"] = drSelect.CopyToDataTable().Rows[0]["JourneySellKey"];
                                dr["FareBasisCode"] = drSelect.CopyToDataTable().Rows[0]["FareBasisCode"];
                                dr["Classes"] = drSelect.CopyToDataTable().Rows[0]["Classes"];
                                dr["ViaName"] = drSelect.CopyToDataTable().Rows[0]["ViaName"];
                                dr["FareSellKey"] = drSelect.CopyToDataTable().Rows[0]["FareSellKey"];
                                dr["BookingFareID"] = drSelect.CopyToDataTable().Rows[0]["BookingFareID"];
                                dr["API_SearchID"] = drSelect.CopyToDataTable().Rows[0]["API_SearchID"];
                                dr["ProductClass"] = drSelect.CopyToDataTable().Rows[0]["ProductClass"];
                                dr["IsPriceChanged"] = drSelect.CopyToDataTable().Rows[0]["IsPriceChanged"];
                                dr["API_AirlineID"] = drSelect.CopyToDataTable().Rows[0]["API_AirlineID"];
                                dr["TempData1"] = drSelect.CopyToDataTable().Rows[0]["TempData1"];
                                dr["TempData2"] = drSelect.CopyToDataTable().Rows[0]["TempData2"];
                                dr["TempData3"] = drSelect.CopyToDataTable().Rows[0]["TempData3"];
                                dr["FlightID"] = drSelect.CopyToDataTable().Rows[0]["FlightID"];
                                dr["Via"] = drSelect.CopyToDataTable().Rows[0]["Via"];
                                dr["SeatsAvailable"] = drSelect.CopyToDataTable().Rows[0]["SeatsAvailable"];
                                dr["RuleNumber"] = drSelect.CopyToDataTable().Rows[0]["RuleNumber"];
                                dr["BaggageDetail"] = drSelect.CopyToDataTable().Rows[0]["BaggageDetail"];
                                dr["API_RefID"] = drSelect.CopyToDataTable().Rows[0]["API_RefID"];

                                dr["JourneyTimeDesc"] = drSelect.CopyToDataTable().Rows[0]["JourneyTimeDesc"];
                                dr["DurationDesc"] = drSelect.CopyToDataTable().Rows[0]["DurationDesc"];
                                dr["DepartureDate"] = drSelect.CopyToDataTable().Rows[0]["DepartureDate"];
                                dr["ArrivalDate"] = drSelect.CopyToDataTable().Rows[0]["ArrivalDate"];
                                dr["DepartureTime"] = drSelect.CopyToDataTable().Rows[0]["DepartureTime"];
                                dr["ArrivalTime"] = drSelect.CopyToDataTable().Rows[0]["ArrivalTime"];

                                dr["ClassOfService"] = drSelect.CopyToDataTable().Rows[0]["ClassOfService"];
                                dr["FareBasisCode"] = drSelect.CopyToDataTable().Rows[0]["FareBasisCode"];
                                dr["EquipmentType"] = drSelect.CopyToDataTable().Rows[0]["EquipmentType"];

                                dr["DepartureTerminal"] = drSelect.CopyToDataTable().Rows[0]["DepartureTerminal"];
                                dr["ArrivalTerminal"] = drSelect.CopyToDataTable().Rows[0]["ArrivalTerminal"];

                                if (drSelect.CopyToDataTable().Columns.Contains("AG_Markup"))
                                {
                                    dr["AG_Markup"] = drSelect.CopyToDataTable().Rows[0]["AG_Markup"];
                                }


                                dr["AdtTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalBasic"];
                                dr["AdtTotalTax"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalTax"];
                                dr["AdtTotalFare"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalFare"];

                                dr["Adt_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Adt_BASIC"];
                                dr["Adt_YQ"] = drSelect.CopyToDataTable().Rows[0]["Adt_YQ"];
                                dr["Adt_CUTE"] = drSelect.CopyToDataTable().Rows[0]["Adt_CUTE"];
                                dr["Adt_PSF"] = drSelect.CopyToDataTable().Rows[0]["Adt_PSF"];
                                dr["Adt_UDF"] = drSelect.CopyToDataTable().Rows[0]["Adt_UDF"];
                                dr["Adt_GST"] = drSelect.CopyToDataTable().Rows[0]["Adt_GST"];
                                dr["Adt_AUDF"] = drSelect.CopyToDataTable().Rows[0]["Adt_AUDF"];
                                dr["Adt_TF"] = drSelect.CopyToDataTable().Rows[0]["Adt_TF"];
                                dr["Adt_EX"] = drSelect.CopyToDataTable().Rows[0]["Adt_EX"];

                                dr["Adt_BAS"] = drSelect.CopyToDataTable().Rows[0]["Adt_BAS"];
                                dr["Adt_Y"] = drSelect.CopyToDataTable().Rows[0]["Adt_Y"];
                                dr["Adt_CB"] = drSelect.CopyToDataTable().Rows[0]["Adt_CB"];
                                dr["Adt_PR"] = drSelect.CopyToDataTable().Rows[0]["Adt_PR"];
                                dr["Adt_MU"] = drSelect.CopyToDataTable().Rows[0]["Adt_MU"];
                                dr["Adt_SF"] = drSelect.CopyToDataTable().Rows[0]["Adt_SF"];
                                dr["Adt_ST"] = drSelect.CopyToDataTable().Rows[0]["Adt_ST"];
                                dr["Adt_TDS"] = drSelect.CopyToDataTable().Rows[0]["Adt_TDS"];

                                if (Chd > 0)
                                {
                                    dr["ChdTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalBasic"];
                                    dr["ChdTotalTax"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalTax"];
                                    dr["ChdTotalFare"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalFare"];

                                    dr["Chd_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Chd_BASIC"];
                                    dr["Chd_YQ"] = drSelect.CopyToDataTable().Rows[0]["Chd_YQ"];
                                    dr["Chd_CUTE"] = drSelect.CopyToDataTable().Rows[0]["Chd_CUTE"];
                                    dr["Chd_PSF"] = drSelect.CopyToDataTable().Rows[0]["Chd_PSF"];
                                    dr["Chd_UDF"] = drSelect.CopyToDataTable().Rows[0]["Chd_UDF"];
                                    dr["Chd_GST"] = drSelect.CopyToDataTable().Rows[0]["Chd_GST"];
                                    dr["Chd_AUDF"] = drSelect.CopyToDataTable().Rows[0]["Chd_AUDF"];
                                    dr["Chd_TF"] = drSelect.CopyToDataTable().Rows[0]["Chd_TF"];
                                    dr["Chd_EX"] = drSelect.CopyToDataTable().Rows[0]["Chd_EX"];

                                    dr["Chd_BAS"] = drSelect.CopyToDataTable().Rows[0]["Chd_BAS"];
                                    dr["Chd_Y"] = drSelect.CopyToDataTable().Rows[0]["Chd_Y"];
                                    dr["Chd_CB"] = drSelect.CopyToDataTable().Rows[0]["Chd_CB"];
                                    dr["Chd_PR"] = drSelect.CopyToDataTable().Rows[0]["Chd_PR"];
                                    dr["Chd_MU"] = drSelect.CopyToDataTable().Rows[0]["Chd_MU"];
                                    dr["Chd_SF"] = drSelect.CopyToDataTable().Rows[0]["Chd_SF"];
                                    dr["Chd_ST"] = drSelect.CopyToDataTable().Rows[0]["Chd_ST"];
                                    dr["Chd_TDS"] = drSelect.CopyToDataTable().Rows[0]["Chd_TDS"];
                                }

                                if (Inf > 0)
                                {
                                    dr["InfTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["InfTotalBasic"];
                                    dr["InfTotalTax"] = drSelect.CopyToDataTable().Rows[0]["InfTotalTax"];
                                    dr["InfTotalFare"] = drSelect.CopyToDataTable().Rows[0]["InfTotalFare"];

                                    dr["Inf_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Inf_BASIC"];
                                    dr["Inf_TAX"] = drSelect.CopyToDataTable().Rows[0]["Inf_TAX"];
                                }

                                dr["TotalServiceTax"] = drSelect.CopyToDataTable().Rows[0]["TotalServiceTax"];
                                dr["TotalServiceFee"] = drSelect.CopyToDataTable().Rows[0]["TotalServiceFee"];
                                dr["TotalMarkup"] = drSelect.CopyToDataTable().Rows[0]["TotalMarkup"];
                                dr["TotalTds"] = drSelect.CopyToDataTable().Rows[0]["TotalTds"];
                                dr["TotalTds_SA"] = drSelect.CopyToDataTable().Rows[0]["TotalTds_SA"];

                                dr["TotalCommission"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                                dr["TotalCommission_SA"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());

                                dr["TotalBasic"] = drSelect.CopyToDataTable().Rows[0]["TotalBasic"];
                                dr["TotalTax"] = drSelect.CopyToDataTable().Rows[0]["TotalTax"];
                                dr["TotalFare"] = drSelect.CopyToDataTable().Rows[0]["TotalFare"];

                                dr["FareQuote"] = drSelect.CopyToDataTable().Rows[0]["FareQuote"];


                                dr["Group"] = drSelect.CopyToDataTable().Rows[0]["Group"];
                                dr["FlightTime"] = drSelect.CopyToDataTable().Rows[0]["FlightTime"];
                                dr["Distance"] = drSelect.CopyToDataTable().Rows[0]["Distance"];
                                dr["ChangeOfPlane"] = drSelect.CopyToDataTable().Rows[0]["ChangeOfPlane"];
                                dr["ParticipantLevel"] = drSelect.CopyToDataTable().Rows[0]["ParticipantLevel"];
                                dr["LinkAvailability"] = drSelect.CopyToDataTable().Rows[0]["LinkAvailability"];
                                dr["PolledAvailabilityOption"] = drSelect.CopyToDataTable().Rows[0]["PolledAvailabilityOption"];
                                dr["OptionalServicesIndicator"] = drSelect.CopyToDataTable().Rows[0]["OptionalServicesIndicator"];
                                dr["AvailabilitySource"] = drSelect.CopyToDataTable().Rows[0]["AvailabilitySource"];
                                dr["AvailabilityDisplayType"] = drSelect.CopyToDataTable().Rows[0]["AvailabilityDisplayType"];
                                dr["CodeshareInfoOperatingCarrier"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfoOperatingCarrier"];
                                dr["CodeshareInfoOperatingFlightNumber"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfoOperatingFlightNumber"];
                                dr["CodeshareInfo"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfo"];
                                dr["TravelTime"] = drSelect.CopyToDataTable().Rows[0]["TravelTime"];
                                dr["BookingCode"] = drSelect.CopyToDataTable().Rows[0]["BookingCode"];
                                dr["CabinClass"] = drSelect.CopyToDataTable().Rows[0]["CabinClass"];
                                dr["LatestTicketingTime"] = drSelect.CopyToDataTable().Rows[0]["LatestTicketingTime"];
                                dr["PricingMethod"] = drSelect.CopyToDataTable().Rows[0]["PricingMethod"];
                                dr["Refundable"] = drSelect.CopyToDataTable().Rows[0]["Refundable"];
                                dr["ETicketability"] = drSelect.CopyToDataTable().Rows[0]["ETicketability"];
                                dr["PlatingCarrier"] = drSelect.CopyToDataTable().Rows[0]["PlatingCarrier"];
                                dr["ProviderCode"] = drSelect.CopyToDataTable().Rows[0]["ProviderCode"];
                                dr["Cat35Indicator"] = drSelect.CopyToDataTable().Rows[0]["Cat35Indicator"];

                                dr["AirPricePointKey"] = drSelect.CopyToDataTable().Rows[0]["AirPricePointKey"];//single      
                                dr["SegmentRef"] = drSelect.CopyToDataTable().Rows[0]["SegmentRef"];//single
                                dr["FlightDetailsRefKey"] = drSelect.CopyToDataTable().Rows[0]["FlightDetailsRefKey"];//single
                                dr["AdtAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["AdtAirPricingInfoKey"];
                                dr["ChdAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["ChdAirPricingInfoKey"];
                                dr["InfAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["InfAirPricingInfoKey"];
                                dr["AdtFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["AdtFareInfoRef"];
                                dr["ChdFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["ChdFareInfoRef"];
                                dr["InfFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["InfFareInfoRef"];
                                dr["AdtOptionKey"] = drSelect.CopyToDataTable().Rows[0]["AdtOptionKey"];
                                dr["ChdOptionKey"] = drSelect.CopyToDataTable().Rows[0]["ChdOptionKey"];
                                dr["InfOptionKey"] = drSelect.CopyToDataTable().Rows[0]["InfOptionKey"];
                                dr["AdtTaxes"] = drSelect.CopyToDataTable().Rows[0]["AdtTaxes"];
                                dr["ChdTaxes"] = drSelect.CopyToDataTable().Rows[0]["ChdTaxes"];
                                dr["InfTaxes"] = drSelect.CopyToDataTable().Rows[0]["InfTaxes"];

                                dr["AdtHTR"] = drSelect.CopyToDataTable().Rows[0]["AdtHTR"];
                                dr["ChdHTR"] = drSelect.CopyToDataTable().Rows[0]["ChdHTR"];
                                dr["InfHTR"] = drSelect.CopyToDataTable().Rows[0]["InfHTR"];

                                dr["HostTokenRef"] = drSelect.CopyToDataTable().Rows[0]["HostTokenRef"];

                                dr["FareRuleInfo_Text"] = drSelect.CopyToDataTable().Rows[0]["FareRuleInfo_Text"];

                                dr["AdtFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["AdtFareInfoRef_data"];
                                dr["ChdFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["ChdFareInfoRef_data"];
                                dr["InfFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["InfFareInfoRef_data"];

                                dr["AdtBTR"] = drSelect.CopyToDataTable().Rows[0]["AdtBTR"];
                                dr["ChdBTR"] = drSelect.CopyToDataTable().Rows[0]["ChdBTR"];
                                dr["InfBTR"] = drSelect.CopyToDataTable().Rows[0]["InfBTR"];

                                dr["Connection_O"] = drSelect.CopyToDataTable().Rows[0]["Connection_O"];
                                dr["Connection_I"] = drSelect.CopyToDataTable().Rows[0]["Connection_I"];
                                dr.AcceptChanges();
                                dsAvailability.Tables[0].AcceptChanges();
                                dsAvailability.AcceptChanges();

                            }
                        }
                    }
                }


                TResponse = CommonFunction.DataSetToString(dsAvailability);
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "UpdateAvailabilityCO", "clsFareUpdate", TSelectedResponse, SearchID, ex.Message);
            }

            return TResponse;
        }
        public static string UpdateAvailabilityRT(string SearchID, string CompanyID, string TResponse, string TSelectedResponse, string RefID, string FltType, string FareRule)
        {
            try
            {
                DataSet dsFareRule = new DataSet();
                if (FareRule.IndexOf("FareRuleInfo") != -1)
                {
                    dsFareRule.ReadXml(new System.IO.StringReader(FareRule));
                }

                DataSet dsAvailability = new DataSet();
                dsAvailability.ReadXml(new System.IO.StringReader(TResponse));

                DataSet dsSelectedAvailability = new DataSet();
                dsSelectedAvailability.ReadXml(new System.IO.StringReader(TSelectedResponse));
               
                if (dsSelectedAvailability.Tables["SSRInfo"] != null)
                {

                    HttpContextHelper.Current?.Session.SetString("SSRInfo", JsonConvert.SerializeObject(dsSelectedAvailability.Tables["SSRInfo"].Copy()));
                }
                else
                {
                    HttpContextHelper.Current?.Session.Remove("SSRInfo");
                }

                DataRow[] drGet = dsSelectedAvailability.Tables["AvailabilityInfo"].Select("FltType='" + FltType + "' And RefID='" + RefID + "'");
                if (drGet.Length > 0)
                {
                    Int32 Chd = Convert.ToInt32(drGet.CopyToDataTable().Rows[0]["Chd"].ToString().Trim());
                    Int32 Inf = Convert.ToInt32(drGet.CopyToDataTable().Rows[0]["Inf"].ToString().Trim());

                    DataRow[] rows = dsAvailability.Tables[0].Select("RefID='" + RefID + "' And FltType='" + FltType + "'");
                    if (rows.Length > 0)
                    {
                        foreach (DataRow dr in rows)
                        {
                            DataRow[] drSelect = drGet.CopyToDataTable().Select("RowID='" + dr["RowID"].ToString().Trim() + "'");
                            if (drSelect.Length > 0)
                            {
                                if (drSelect.CopyToDataTable().Rows[0]["DepartureStation"].ToString().Equals(dr["DepartureStation"].ToString()) && drSelect.CopyToDataTable().Rows[0]["ArrivalStation"].ToString().Equals(dr["ArrivalStation"].ToString()))
                                {
                                    string strFareRule = string.Empty;
                                    if (dsFareRule != null && dsFareRule.Tables["FareRuleInfo"] != null)
                                    {
                                        DataRow[] drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["Origin"].ToString() + "' And Destination='" + dr["Destination"].ToString() + "'");
                                        if (drFareRule.Length > 0)
                                        {
                                            strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                                        }
                                        else
                                        {
                                            drFareRule = dsFareRule.Tables["FareRuleInfo"].Select("Origin='" + dr["DepartureStation"].ToString() + "' And Destination='" + dr["ArrivalStation"].ToString() + "'");
                                            if (drFareRule.Length > 0)
                                            {
                                                strFareRule = drFareRule.CopyToDataTable().Rows[0]["RuleDetail"].ToString();
                                            }

                                            if (dsFareRule.Tables["FareRuleInfo"].Rows.Count.Equals(1) && strFareRule.Trim().Length.Equals(0))
                                            {
                                                strFareRule = dsFareRule.Tables["FareRuleInfo"].Rows[0]["RuleDetail"].ToString();
                                            }
                                        }
                                    }

                                    if (strFareRule.Length > 1)
                                    {
                                        dr["FareRule"] = strFareRule;
                                    }

                                    if (dr["CarrierCode"].ToString().Equals("QP") && dr["AirlineID"].ToString().Equals("QPMAA8752B"))
                                    {
                                        dr["FareRule"] = GetQPFareRule(dr["Origin"].ToString(), dr["Destination"].ToString(), TSelectedResponse);
                                    }

                                    dr["FareStatus"] = drSelect.CopyToDataTable().Rows[0]["FareStatus"];
                                    dr["IsPriceChanged"] = drSelect.CopyToDataTable().Rows[0]["IsPriceChanged"];

                                    if (drSelect.CopyToDataTable().Rows[0]["RuleTarrif"].ToString().Trim().Length > 0)
                                    {
                                        dr["RuleTarrif"] = drSelect.CopyToDataTable().Rows[0]["RuleTarrif"];
                                    }
                                    dr["RefundType"] = drSelect.CopyToDataTable().Rows[0]["RefundType"];

                                    dr["DepDate"] = drSelect.CopyToDataTable().Rows[0]["DepDate"];
                                    dr["ArrDate"] = drSelect.CopyToDataTable().Rows[0]["ArrDate"];
                                    dr["DepTime"] = drSelect.CopyToDataTable().Rows[0]["DepTime"];
                                    dr["ArrTime"] = drSelect.CopyToDataTable().Rows[0]["ArrTime"];
                                    dr["JourneyTime"] = drSelect.CopyToDataTable().Rows[0]["JourneyTime"];
                                    dr["Duration"] = drSelect.CopyToDataTable().Rows[0]["Duration"];


                                    dr["API_BookingFareID"] = drSelect.CopyToDataTable().Rows[0]["API_BookingFareID"];
                                    dr["Api_SessionID"] = drSelect.CopyToDataTable().Rows[0]["Api_SessionID"];
                                    dr["JourneySellKey"] = drSelect.CopyToDataTable().Rows[0]["JourneySellKey"];
                                    dr["FareBasisCode"] = drSelect.CopyToDataTable().Rows[0]["FareBasisCode"];
                                    dr["Classes"] = drSelect.CopyToDataTable().Rows[0]["Classes"];
                                    dr["ViaName"] = drSelect.CopyToDataTable().Rows[0]["ViaName"];
                                    dr["FareSellKey"] = drSelect.CopyToDataTable().Rows[0]["FareSellKey"];
                                    dr["BookingFareID"] = drSelect.CopyToDataTable().Rows[0]["BookingFareID"];
                                    dr["API_SearchID"] = drSelect.CopyToDataTable().Rows[0]["API_SearchID"];
                                    dr["ProductClass"] = drSelect.CopyToDataTable().Rows[0]["ProductClass"];
                                    dr["IsPriceChanged"] = drSelect.CopyToDataTable().Rows[0]["IsPriceChanged"];
                                    dr["API_AirlineID"] = drSelect.CopyToDataTable().Rows[0]["API_AirlineID"];
                                    dr["TempData1"] = drSelect.CopyToDataTable().Rows[0]["TempData1"];
                                    dr["TempData2"] = drSelect.CopyToDataTable().Rows[0]["TempData2"];
                                    dr["TempData3"] = drSelect.CopyToDataTable().Rows[0]["TempData3"];
                                    dr["FlightID"] = drSelect.CopyToDataTable().Rows[0]["FlightID"];
                                    dr["Via"] = drSelect.CopyToDataTable().Rows[0]["Via"];
                                    dr["SeatsAvailable"] = drSelect.CopyToDataTable().Rows[0]["SeatsAvailable"];
                                    dr["RuleNumber"] = drSelect.CopyToDataTable().Rows[0]["RuleNumber"];
                                    dr["BaggageDetail"] = drSelect.CopyToDataTable().Rows[0]["BaggageDetail"];
                                    dr["API_RefID"] = drSelect.CopyToDataTable().Rows[0]["API_RefID"];

                                    dr["JourneyTimeDesc"] = drSelect.CopyToDataTable().Rows[0]["JourneyTimeDesc"];
                                    dr["DurationDesc"] = drSelect.CopyToDataTable().Rows[0]["DurationDesc"];
                                    dr["DepartureDate"] = drSelect.CopyToDataTable().Rows[0]["DepartureDate"];
                                    dr["ArrivalDate"] = drSelect.CopyToDataTable().Rows[0]["ArrivalDate"];
                                    dr["DepartureTime"] = drSelect.CopyToDataTable().Rows[0]["DepartureTime"];
                                    dr["ArrivalTime"] = drSelect.CopyToDataTable().Rows[0]["ArrivalTime"];

                                    dr["ClassOfService"] = drSelect.CopyToDataTable().Rows[0]["ClassOfService"];
                                    dr["FareBasisCode"] = drSelect.CopyToDataTable().Rows[0]["FareBasisCode"];
                                    dr["EquipmentType"] = drSelect.CopyToDataTable().Rows[0]["EquipmentType"];

                                    dr["DepartureTerminal"] = drSelect.CopyToDataTable().Rows[0]["DepartureTerminal"];
                                    dr["ArrivalTerminal"] = drSelect.CopyToDataTable().Rows[0]["ArrivalTerminal"];

                                    if (drSelect.CopyToDataTable().Columns.Contains("AG_Markup"))
                                    {
                                        dr["AG_Markup"] = drSelect.CopyToDataTable().Rows[0]["AG_Markup"];
                                    }


                                    dr["AdtTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalBasic"];
                                    dr["AdtTotalTax"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalTax"];
                                    dr["AdtTotalFare"] = drSelect.CopyToDataTable().Rows[0]["AdtTotalFare"];

                                    dr["Adt_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Adt_BASIC"];
                                    dr["Adt_YQ"] = drSelect.CopyToDataTable().Rows[0]["Adt_YQ"];
                                    dr["Adt_CUTE"] = drSelect.CopyToDataTable().Rows[0]["Adt_CUTE"];
                                    dr["Adt_PSF"] = drSelect.CopyToDataTable().Rows[0]["Adt_PSF"];
                                    dr["Adt_UDF"] = drSelect.CopyToDataTable().Rows[0]["Adt_UDF"];
                                    dr["Adt_GST"] = drSelect.CopyToDataTable().Rows[0]["Adt_GST"];
                                    dr["Adt_AUDF"] = drSelect.CopyToDataTable().Rows[0]["Adt_AUDF"];
                                    dr["Adt_TF"] = drSelect.CopyToDataTable().Rows[0]["Adt_TF"];
                                    dr["Adt_EX"] = drSelect.CopyToDataTable().Rows[0]["Adt_EX"];

                                    dr["Adt_BAS"] = drSelect.CopyToDataTable().Rows[0]["Adt_BAS"];
                                    dr["Adt_Y"] = drSelect.CopyToDataTable().Rows[0]["Adt_Y"];
                                    dr["Adt_CB"] = drSelect.CopyToDataTable().Rows[0]["Adt_CB"];
                                    dr["Adt_PR"] = drSelect.CopyToDataTable().Rows[0]["Adt_PR"];
                                    dr["Adt_MU"] = drSelect.CopyToDataTable().Rows[0]["Adt_MU"];
                                    dr["Adt_SF"] = drSelect.CopyToDataTable().Rows[0]["Adt_SF"];
                                    dr["Adt_ST"] = drSelect.CopyToDataTable().Rows[0]["Adt_ST"];
                                    dr["Adt_TDS"] = drSelect.CopyToDataTable().Rows[0]["Adt_TDS"];

                                    if (Chd > 0)
                                    {
                                        dr["ChdTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalBasic"];
                                        dr["ChdTotalTax"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalTax"];
                                        dr["ChdTotalFare"] = drSelect.CopyToDataTable().Rows[0]["ChdTotalFare"];

                                        dr["Chd_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Chd_BASIC"];
                                        dr["Chd_YQ"] = drSelect.CopyToDataTable().Rows[0]["Chd_YQ"];
                                        dr["Chd_CUTE"] = drSelect.CopyToDataTable().Rows[0]["Chd_CUTE"];
                                        dr["Chd_PSF"] = drSelect.CopyToDataTable().Rows[0]["Chd_PSF"];
                                        dr["Chd_UDF"] = drSelect.CopyToDataTable().Rows[0]["Chd_UDF"];
                                        dr["Chd_GST"] = drSelect.CopyToDataTable().Rows[0]["Chd_GST"];
                                        dr["Chd_AUDF"] = drSelect.CopyToDataTable().Rows[0]["Chd_AUDF"];
                                        dr["Chd_TF"] = drSelect.CopyToDataTable().Rows[0]["Chd_TF"];
                                        dr["Chd_EX"] = drSelect.CopyToDataTable().Rows[0]["Chd_EX"];

                                        dr["Chd_BAS"] = drSelect.CopyToDataTable().Rows[0]["Chd_BAS"];
                                        dr["Chd_Y"] = drSelect.CopyToDataTable().Rows[0]["Chd_Y"];
                                        dr["Chd_CB"] = drSelect.CopyToDataTable().Rows[0]["Chd_CB"];
                                        dr["Chd_PR"] = drSelect.CopyToDataTable().Rows[0]["Chd_PR"];
                                        dr["Chd_MU"] = drSelect.CopyToDataTable().Rows[0]["Chd_MU"];
                                        dr["Chd_SF"] = drSelect.CopyToDataTable().Rows[0]["Chd_SF"];
                                        dr["Chd_ST"] = drSelect.CopyToDataTable().Rows[0]["Chd_ST"];
                                        dr["Chd_TDS"] = drSelect.CopyToDataTable().Rows[0]["Chd_TDS"];
                                    }

                                    if (Inf > 0)
                                    {
                                        dr["InfTotalBasic"] = drSelect.CopyToDataTable().Rows[0]["InfTotalBasic"];
                                        dr["InfTotalTax"] = drSelect.CopyToDataTable().Rows[0]["InfTotalTax"];
                                        dr["InfTotalFare"] = drSelect.CopyToDataTable().Rows[0]["InfTotalFare"];

                                        dr["Inf_BASIC"] = drSelect.CopyToDataTable().Rows[0]["Inf_BASIC"];
                                        dr["Inf_TAX"] = drSelect.CopyToDataTable().Rows[0]["Inf_TAX"];
                                    }

                                    dr["TotalServiceTax"] = drSelect.CopyToDataTable().Rows[0]["TotalServiceTax"];
                                    dr["TotalServiceFee"] = drSelect.CopyToDataTable().Rows[0]["TotalServiceFee"];
                                    dr["TotalMarkup"] = drSelect.CopyToDataTable().Rows[0]["TotalMarkup"];
                                    dr["TotalTds"] = drSelect.CopyToDataTable().Rows[0]["TotalTds"];
                                    dr["TotalTds_SA"] = drSelect.CopyToDataTable().Rows[0]["TotalTds_SA"];

                                    dr["TotalCommission"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                                    dr["TotalCommission_SA"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());

                                    dr["TotalBasic"] = drSelect.CopyToDataTable().Rows[0]["TotalBasic"];
                                    dr["TotalTax"] = drSelect.CopyToDataTable().Rows[0]["TotalTax"];
                                    dr["TotalFare"] = drSelect.CopyToDataTable().Rows[0]["TotalFare"];

                                    dr["FareQuote"] = drSelect.CopyToDataTable().Rows[0]["FareQuote"];

                                    dr["Group"] = drSelect.CopyToDataTable().Rows[0]["Group"];
                                    dr["FlightTime"] = drSelect.CopyToDataTable().Rows[0]["FlightTime"];
                                    dr["Distance"] = drSelect.CopyToDataTable().Rows[0]["Distance"];
                                    dr["ChangeOfPlane"] = drSelect.CopyToDataTable().Rows[0]["ChangeOfPlane"];
                                    dr["ParticipantLevel"] = drSelect.CopyToDataTable().Rows[0]["ParticipantLevel"];
                                    dr["LinkAvailability"] = drSelect.CopyToDataTable().Rows[0]["LinkAvailability"];
                                    dr["PolledAvailabilityOption"] = drSelect.CopyToDataTable().Rows[0]["PolledAvailabilityOption"];
                                    dr["OptionalServicesIndicator"] = drSelect.CopyToDataTable().Rows[0]["OptionalServicesIndicator"];
                                    dr["AvailabilitySource"] = drSelect.CopyToDataTable().Rows[0]["AvailabilitySource"];
                                    dr["AvailabilityDisplayType"] = drSelect.CopyToDataTable().Rows[0]["AvailabilityDisplayType"];
                                    dr["CodeshareInfoOperatingCarrier"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfoOperatingCarrier"];
                                    dr["CodeshareInfoOperatingFlightNumber"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfoOperatingFlightNumber"];
                                    dr["CodeshareInfo"] = drSelect.CopyToDataTable().Rows[0]["CodeshareInfo"];
                                    dr["TravelTime"] = drSelect.CopyToDataTable().Rows[0]["TravelTime"];
                                    dr["BookingCode"] = drSelect.CopyToDataTable().Rows[0]["BookingCode"];
                                    dr["CabinClass"] = drSelect.CopyToDataTable().Rows[0]["CabinClass"];
                                    dr["LatestTicketingTime"] = drSelect.CopyToDataTable().Rows[0]["LatestTicketingTime"];
                                    dr["PricingMethod"] = drSelect.CopyToDataTable().Rows[0]["PricingMethod"];
                                    dr["Refundable"] = drSelect.CopyToDataTable().Rows[0]["Refundable"];
                                    dr["ETicketability"] = drSelect.CopyToDataTable().Rows[0]["ETicketability"];
                                    dr["PlatingCarrier"] = drSelect.CopyToDataTable().Rows[0]["PlatingCarrier"];
                                    dr["ProviderCode"] = drSelect.CopyToDataTable().Rows[0]["ProviderCode"];
                                    dr["Cat35Indicator"] = drSelect.CopyToDataTable().Rows[0]["Cat35Indicator"];

                                    dr["AirPricePointKey"] = drSelect.CopyToDataTable().Rows[0]["AirPricePointKey"];//single      
                                    dr["SegmentRef"] = drSelect.CopyToDataTable().Rows[0]["SegmentRef"];//single
                                    dr["FlightDetailsRefKey"] = drSelect.CopyToDataTable().Rows[0]["FlightDetailsRefKey"];//single
                                    dr["AdtAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["AdtAirPricingInfoKey"];
                                    dr["ChdAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["ChdAirPricingInfoKey"];
                                    dr["InfAirPricingInfoKey"] = drSelect.CopyToDataTable().Rows[0]["InfAirPricingInfoKey"];
                                    dr["AdtFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["AdtFareInfoRef"];
                                    dr["ChdFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["ChdFareInfoRef"];
                                    dr["InfFareInfoRef"] = drSelect.CopyToDataTable().Rows[0]["InfFareInfoRef"];
                                    dr["AdtOptionKey"] = drSelect.CopyToDataTable().Rows[0]["AdtOptionKey"];
                                    dr["ChdOptionKey"] = drSelect.CopyToDataTable().Rows[0]["ChdOptionKey"];
                                    dr["InfOptionKey"] = drSelect.CopyToDataTable().Rows[0]["InfOptionKey"];
                                    dr["AdtTaxes"] = drSelect.CopyToDataTable().Rows[0]["AdtTaxes"];
                                    dr["ChdTaxes"] = drSelect.CopyToDataTable().Rows[0]["ChdTaxes"];
                                    dr["InfTaxes"] = drSelect.CopyToDataTable().Rows[0]["InfTaxes"];

                                    dr["AdtHTR"] = drSelect.CopyToDataTable().Rows[0]["AdtHTR"];
                                    dr["ChdHTR"] = drSelect.CopyToDataTable().Rows[0]["ChdHTR"];
                                    dr["InfHTR"] = drSelect.CopyToDataTable().Rows[0]["InfHTR"];

                                    dr["HostTokenRef"] = drSelect.CopyToDataTable().Rows[0]["HostTokenRef"];

                                    dr["FareRuleInfo_Text"] = drSelect.CopyToDataTable().Rows[0]["FareRuleInfo_Text"];

                                    dr["AdtFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["AdtFareInfoRef_data"];
                                    dr["ChdFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["ChdFareInfoRef_data"];
                                    dr["InfFareInfoRef_data"] = drSelect.CopyToDataTable().Rows[0]["InfFareInfoRef_data"];

                                    dr["AdtBTR"] = drSelect.CopyToDataTable().Rows[0]["AdtBTR"];
                                    dr["ChdBTR"] = drSelect.CopyToDataTable().Rows[0]["ChdBTR"];
                                    dr["InfBTR"] = drSelect.CopyToDataTable().Rows[0]["InfBTR"];

                                    dr["Connection_O"] = drSelect.CopyToDataTable().Rows[0]["Connection_O"];
                                    dr["Connection_I"] = drSelect.CopyToDataTable().Rows[0]["Connection_I"];

                                    dr.AcceptChanges();
                                    dsAvailability.Tables[0].AcceptChanges();
                                    dsAvailability.AcceptChanges();
                                }
                            }
                        }
                    }
                }

                TResponse = CommonFunction.DataSetToString(dsAvailability);
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "UpdateAvailabilityRT", "clsFareUpdate", TSelectedResponse, SearchID, ex.Message);
            }

            return TResponse;
        }
        private static string GetQPFareRule(string Origin, string Destination, string tResponse)
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(tResponse);

                XmlNodeList dataNodes = xmldoc.SelectNodes("root");

                foreach (XmlNode node in dataNodes)
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        string KK = node2["Origin"].InnerText.ToString();
                        if (node2["Origin"].InnerText.Equals(Origin) && node2["Destination"].InnerText.Equals(Destination))
                        {
                            return node2["FareRule"].InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg("", 0, "GetQPFareRule", "clsFareUpdate", tResponse, Origin + Destination, ex.Message);
            }
            return "";
        }
    }
}
