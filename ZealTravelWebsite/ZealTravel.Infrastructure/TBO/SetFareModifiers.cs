using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Infrastructure.TBO
{
    class SetFareModifiers
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;
        //-----------------------------------------------------------------------------------------------
        public SetFareModifiers(string Searchid, string Companyid)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
        }
        public void Filter(string RS_Fare, DataTable dtFlights, bool IsRT)
        {
            try
            {
                DataTable dtTemp = GetFlightModifierFunctions.Tempxml_Schema;

                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(RS_Fare);

                string ResultIndex = xmlflt.SelectSingleNode("Response/Results/ResultIndex").InnerText;
                string Source = xmlflt.SelectSingleNode("Response/Results/Source").InnerText;
                string IsLCC = xmlflt.SelectSingleNode("Response/Results/IsLCC").InnerText;
                string IsRefundable = xmlflt.SelectSingleNode("Response/Results/IsRefundable").InnerText;
                string AirlineRemark = xmlflt.SelectSingleNode("Response/Results/AirlineRemark").InnerText;

                string LastTicketDate = xmlflt.SelectSingleNode("Response/Results/LastTicketDate").InnerText;
                string TicketAdvisory = xmlflt.SelectSingleNode("Response/Results/TicketAdvisory").InnerText;
                string AirlineCode = xmlflt.SelectSingleNode("Response/Results/AirlineCode").InnerText;
                string ValidatingAirline = xmlflt.SelectSingleNode("Response/Results/ValidatingAirline").InnerText;

                string Fare = string.Empty;
                string FareBreakdown = string.Empty;
                string Segments = string.Empty;
                string FareRules = string.Empty;

                XmlNodeList nodesFare = xmlflt.SelectNodes("Response/Results/Fare"); // You can also use XPath here
                foreach (XmlNode node in nodesFare)
                {
                    Fare = node.OuterXml;
                }

                FareBreakdown = "<Segments>";
                XmlNodeList nodesFareBreakdown = xmlflt.SelectNodes("Response/Results/FareBreakdown"); // You can also use XPath here
                foreach (XmlNode node in nodesFareBreakdown)
                {
                    FareBreakdown += node.OuterXml;
                }
                FareBreakdown += "</Segments>";

                Segments = "<Segments>";
                XmlNodeList nodesSegments = xmlflt.SelectNodes("Response/Results/Segments"); // You can also use XPath here
                foreach (XmlNode node in nodesSegments)
                {
                    Segments += node.OuterXml;
                }
                Segments += "</Segments>";

                FareRules = "<Segments>";
                XmlNodeList nodesFareRules = xmlflt.SelectNodes("Response/Results/FareRules"); // You can also use XPath here
                foreach (XmlNode node in nodesFareRules)
                {
                    FareRules += node.OuterXml;
                }
                FareRules += "</Segments>";

                DataRow drAdd = dtTemp.NewRow();
                drAdd["RefID"] = dtFlights.Rows[0]["RefID"].ToString();
                drAdd["Fare"] = Fare;
                drAdd["FareBreakdown"] = FareBreakdown;
                drAdd["Segments"] = Segments;
                drAdd["FareRules"] = FareRules;
                drAdd["ResultIndex"] = ResultIndex;
                drAdd["Source"] = Source;
                drAdd["IsLCC"] = IsLCC;
                drAdd["IsRefundable"] = IsRefundable;
                drAdd["AirlineRemark"] = AirlineRemark;
                drAdd["LastTicketDate"] = LastTicketDate;
                drAdd["TicketAdvisory"] = TicketAdvisory;
                drAdd["AirlineCode"] = AirlineCode;
                drAdd["ValidatingAirline"] = ValidatingAirline;
                dtTemp.Rows.Add(drAdd);

                foreach (DataRow dr in dtTemp.Rows)
                {
                    SecondFilter(RS_Fare, dr, dtFlights, IsRT);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(Companyid, 0, "Filter", "air_tbo-SetFareModifiers", RS_Fare, Searchid, errorMessage);
            }
        }
        private void SecondFilter(string OuterXml, DataRow dr, DataTable dtFlights, bool IsRT)
        {
            XmlDocument xmlflt = new XmlDocument();
            xmlflt.LoadXml(dr["Segments"].ToString());
            string xmlSegments = string.Empty;
            XmlNodeList nodesSegments = xmlflt.SelectNodes("Segments/Segments"); // You can also use XPath here
            foreach (XmlNode node in nodesSegments)
            {
                ThirdFilter(OuterXml, node.OuterXml, dr, dtFlights, IsRT);
            }
        }
        private void ThirdFilter(string OuterXml, string Segments, DataRow dr, DataTable dtFlights, bool IsRT)
        {
            XmlDocument xmlflt = new XmlDocument();
            xmlflt.LoadXml(Segments);

            XmlNodeList nodesSegments = xmlflt.SelectNodes("Segments/Segments"); // You can also use XPath here
            foreach (XmlNode node in nodesSegments)
            {
                FourthFilter(OuterXml, node.OuterXml, dr, dtFlights, IsRT);
            }
        }
        private void FourthFilter(string OuterXml, string Segments, DataRow dr, DataTable dtFlights, bool IsRT)
        {
            try
            {
                XmlDocument xmlOuter = new XmlDocument();
                xmlOuter.LoadXml(OuterXml);

                int ErrorCode = Convert.ToInt32(xmlOuter.SelectSingleNode("Response/Error/ErrorCode").InnerText);
                string ErrorMessage = xmlOuter.SelectSingleNode("Response/Error/ErrorMessage").InnerText;

                bool IsPriceChanged = Convert.ToBoolean(xmlOuter.SelectSingleNode("Response/IsPriceChanged").InnerText);
                string ResponseStatus = xmlOuter.SelectSingleNode("Response/ResponseStatus").InnerText;

                XmlDocument xmlFR = new XmlDocument();
                xmlFR.LoadXml(dr["FareRules"].ToString());
                XmlDocument xmlFare = new XmlDocument();
                xmlFare.LoadXml(dr["Fare"].ToString());

                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(Segments);

                string Baggage = xmlflt.SelectSingleNode("Segments/Baggage").InnerText;
                if (Baggage.Equals(string.Empty))
                {
                    Baggage = "0 KG";
                }
                else
                {
                    Baggage = Baggage.ToUpper();
                }

                string CabinBaggage = xmlflt.SelectSingleNode("Segments/CabinBaggage").InnerText;
                if (CabinBaggage.Equals(string.Empty))
                {
                    CabinBaggage = "0 KG";
                }
                else
                {
                    CabinBaggage = CabinBaggage.ToUpper();
                }


                string TripIndicator = xmlflt.SelectSingleNode("Segments/TripIndicator").InnerText;
                string SegmentIndicator = xmlflt.SelectSingleNode("Segments/SegmentIndicator").InnerText;
                string Duration = xmlflt.SelectSingleNode("Segments/Duration").InnerText;
                string Journey = xmlflt.SelectSingleNode("Segments/Duration").InnerText;
                if (xmlflt.SelectSingleNode("Segments/AccumulatedDuration") != null)
                {
                    Journey = xmlflt.SelectSingleNode("Segments/AccumulatedDuration").InnerText;
                }

                string StopPointArrivalTime = xmlflt.SelectSingleNode("Segments/StopPointArrivalTime").InnerText;
                string StopPointDepartureTime = xmlflt.SelectSingleNode("Segments/StopPointDepartureTime").InnerText;
                string Craft = xmlflt.SelectSingleNode("Segments/Craft").InnerText;

                bool IsETicketEligible = Convert.ToBoolean(xmlflt.SelectSingleNode("Segments/IsETicketEligible").InnerText);
                string FlightStatus = xmlflt.SelectSingleNode("Segments/FlightStatus").InnerText;


                int Via = 0;
                string ViaName = string.Empty;
                if (xmlflt.SelectSingleNode("Segments/StopOver") != null)
                {
                    if (Convert.ToBoolean(xmlflt.SelectSingleNode("Segments/StopOver").InnerText.Trim()).Equals(true))
                    {
                        Via = 1;
                        if (xmlflt.SelectSingleNode("Segments/StopPoint") != null)
                        {
                            ViaName = xmlflt.SelectSingleNode("Segments/StopPoint").InnerText.Trim();
                        }
                    }
                }


                string AirlineCode = xmlflt.SelectSingleNode("Segments/Airline/AirlineCode").InnerText;
                string AirlineName = xmlflt.SelectSingleNode("Segments/Airline/AirlineName").InnerText;
                string FlightNumber = xmlflt.SelectSingleNode("Segments/Airline/FlightNumber").InnerText;
                string FareClass = xmlflt.SelectSingleNode("Segments/Airline/FareClass").InnerText;
                string OperatingCarrier = xmlflt.SelectSingleNode("Segments/Airline/OperatingCarrier").InnerText;

                string Origin_AirportCode = xmlflt.SelectSingleNode("Segments/Origin/Airport/AirportCode").InnerText;
                string Origin_Terminal = xmlflt.SelectSingleNode("Segments/Origin/Airport/Terminal").InnerText;
                string Origin_CountryCode = xmlflt.SelectSingleNode("Segments/Origin/Airport/CountryCode").InnerText;
                string Origin_DepTime = xmlflt.SelectSingleNode("Segments/Origin/DepTime").InnerText;

                string Destination_AirportCode = xmlflt.SelectSingleNode("Segments/Destination/Airport/AirportCode").InnerText;
                string Destination_Terminal = xmlflt.SelectSingleNode("Segments/Destination/Airport/Terminal").InnerText;
                string Destination_CountryCode = xmlflt.SelectSingleNode("Segments/Destination/Airport/CountryCode").InnerText;
                string Destination_ArrTime = xmlflt.SelectSingleNode("Segments/Destination/ArrTime").InnerText;

                XmlDocument xmlFb = new XmlDocument();
                xmlFb.LoadXml(dr["FareBreakdown"].ToString());

                Decimal Commission = 0;
                Decimal dAdt_Basic = 0;
                Decimal dAdt_Tax = 0;
                Decimal dAdt_YQ = 0;

                Decimal dChd_Basic = 0;
                Decimal dChd_Tax = 0;
                Decimal dChd_YQ = 0;

                Decimal dInf_Basic = 0;
                Decimal dInf_Tax = 0;
                Decimal dInf_YQ = 0;

                Decimal dCUTE = 0;
                Decimal dPSF = 0;
                Decimal dUDF = 0;
                Decimal dSVT = 0;
                Decimal dAUDF = 0;
                Decimal dTF = 0;
                Decimal dOC = 0;

                Int32 iTotalAdt = 0;
                Int32 iTotalChd = 0;
                Int32 iTotalInf = 0;

                XmlNodeList xnList1 = xmlFb.SelectNodes("Segments/FareBreakdown[PassengerType=" + "1" + "]");
                if (xnList1 != null)
                {
                    foreach (XmlNode node in xnList1)
                    {
                        iTotalAdt = Convert.ToInt32(node["PassengerCount"].InnerText.Trim().ToUpper());
                        dAdt_Basic = Convert.ToDecimal(node["BaseFare"].InnerText.Trim().ToUpper());
                        dAdt_Tax = Convert.ToDecimal(node["Tax"].InnerText.Trim().ToUpper());
                        dAdt_YQ = Convert.ToDecimal(node["YQTax"].InnerText.Trim().ToUpper());

                        dAdt_Basic = dAdt_Basic / iTotalAdt;
                        dAdt_Tax = dAdt_Tax / iTotalAdt;
                        dAdt_YQ = dAdt_YQ / iTotalAdt;
                    }
                }

                XmlNodeList xnList2 = xmlFb.SelectNodes("Segments/FareBreakdown[PassengerType=" + "2" + "]");
                if (xnList2 != null)
                {
                    foreach (XmlNode node in xnList2)
                    {
                        iTotalChd = Convert.ToInt32(node["PassengerCount"].InnerText.Trim().ToUpper());
                        dChd_Basic = Convert.ToDecimal(node["BaseFare"].InnerText.Trim().ToUpper());
                        dChd_Tax = Convert.ToDecimal(node["Tax"].InnerText.Trim().ToUpper());
                        dChd_YQ = Convert.ToDecimal(node["YQTax"].InnerText.Trim().ToUpper());

                        dChd_Basic = dChd_Basic / iTotalChd;
                        dChd_Tax = dChd_Tax / iTotalChd;
                        dChd_YQ = dChd_YQ / iTotalChd;
                    }
                }

                XmlNodeList xnList3 = xmlFb.SelectNodes("Segments/FareBreakdown[PassengerType=" + "3" + "]");
                if (xnList3 != null)
                {
                    foreach (XmlNode node in xnList3)
                    {
                        iTotalInf = Convert.ToInt32(node["PassengerCount"].InnerText.Trim().ToUpper());
                        dInf_Basic = Convert.ToDecimal(node["BaseFare"].InnerText.Trim().ToUpper());
                        dInf_Tax = Convert.ToDecimal(node["Tax"].InnerText.Trim().ToUpper());
                        dInf_YQ = Convert.ToDecimal(node["YQTax"].InnerText.Trim().ToUpper());

                        dInf_Basic = dInf_Basic / iTotalInf;
                        dInf_Tax = dInf_Tax / iTotalInf;
                        dInf_YQ = dInf_YQ / iTotalInf;
                    }
                }

                XmlNodeList nodeListfare = xmlFare.SelectNodes("/Fare/TaxBreakup");
                foreach (XmlNode no in nodeListfare)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(no.OuterXml);
                    string key = xmldoc.SelectSingleNode("TaxBreakup/key").InnerText.Trim().ToUpper();

                    if (key.IndexOf("YR") != -1)
                    {
                        dCUTE += Convert.ToDecimal(xmldoc.SelectSingleNode("TaxBreakup/value").InnerText.Trim().ToUpper());
                    }
                    else if (key.IndexOf("PSF") != -1)
                    {
                        dPSF += Convert.ToDecimal(xmldoc.SelectSingleNode("TaxBreakup/value").InnerText.Trim().ToUpper());
                    }
                    else if (key.IndexOf("UDF") != -1)
                    {
                        dUDF += Convert.ToDecimal(xmldoc.SelectSingleNode("TaxBreakup/value").InnerText.Trim().ToUpper());
                    }
                    else if (key.IndexOf("JNTax".ToUpper()) != -1 || key.IndexOf("K3".ToUpper()) != -1)
                    {
                        dSVT += Convert.ToDecimal(xmldoc.SelectSingleNode("TaxBreakup/value").InnerText.Trim().ToUpper());
                    }
                    else if (key.IndexOf("INTax".ToUpper()) != -1)
                    {
                        dAUDF += Convert.ToDecimal(xmldoc.SelectSingleNode("TaxBreakup/value").InnerText.Trim().ToUpper());
                    }
                    else if (key.IndexOf("TransactionFee".ToUpper()) != -1)
                    {
                        dTF += Convert.ToDecimal(xmldoc.SelectSingleNode("TaxBreakup/value").InnerText.Trim().ToUpper());
                    }
                }
                if (xmlFare.SelectNodes("/Fare/OtherCharges") != null)
                {
                    dOC = Convert.ToDecimal(xmlFare.SelectSingleNode("/Fare/OtherCharges").InnerText);
                }

                if (xmlFare.SelectNodes("/Fare/CommissionEarned") != null)
                {
                    Commission = Convert.ToDecimal(xmlFare.SelectSingleNode("/Fare/CommissionEarned").InnerText);
                }
                if (xmlFare.SelectNodes("/Fare/PLBEarned") != null)
                {
                    Commission += Convert.ToDecimal(xmlFare.SelectSingleNode("/Fare/PLBEarned").InnerText);
                }
                if (xmlFare.SelectNodes("/Fare/IncentiveEarned") != null)
                {
                    Commission += Convert.ToDecimal(xmlFare.SelectSingleNode("/Fare/IncentiveEarned").InnerText);
                }

                dCUTE = dCUTE / (iTotalAdt + iTotalChd);
                dCUTE = GetCommonFunctions.AvgAmount1(dCUTE.ToString());

                dPSF = dPSF / (iTotalAdt + iTotalChd);
                dPSF = GetCommonFunctions.AvgAmount1(dPSF.ToString());

                dUDF = dUDF / (iTotalAdt + iTotalChd);
                dUDF = GetCommonFunctions.AvgAmount1(dUDF.ToString());

                dSVT = dSVT / (iTotalAdt + iTotalChd);
                dSVT = GetCommonFunctions.AvgAmount1(dSVT.ToString());

                dAUDF = dAUDF / (iTotalAdt + iTotalChd);
                dAUDF = GetCommonFunctions.AvgAmount1(dAUDF.ToString());

                dTF = dTF / (iTotalAdt + iTotalChd);
                dTF = GetCommonFunctions.AvgAmount1(dTF.ToString());

                dOC = dOC / (iTotalAdt + iTotalChd);
                dOC = GetCommonFunctions.AvgAmount1(dOC.ToString());

                Commission = Commission / (iTotalAdt + iTotalChd);

                string FareRules_FareBasisCode = string.Empty;
                XmlNodeList nodeList1 = xmlFR.SelectNodes("Segments/FareRules");
                foreach (XmlNode no in nodeList1)
                {
                    if (no["Origin"].InnerText.Equals(Origin_AirportCode) && no["Destination"].InnerText.Equals(Destination_AirportCode))
                    {
                        FareRules_FareBasisCode = no["FareBasisCode"].InnerText;
                    }
                }

                foreach (DataRow drSelect in dtFlights.Rows)
                {
                    if (drSelect["DepartureStation"].ToString().Equals(Origin_AirportCode) && drSelect["ArrivalStation"].ToString().Equals(Destination_AirportCode))
                    {
                        if ((drSelect["BookingFareID"].ToString().Trim().Equals(dr["ResultIndex"].ToString().Trim())) || IsRT.Equals(true))
                        {
                            drSelect["Via"] = Via;
                            drSelect["ViaName"] = ViaName;

                            drSelect["API_SearchID"] = Commission.ToString();
                            drSelect["BaggageDetail"] = Baggage + "*" + CabinBaggage;

                            string AirlineRemark = dr["AirlineRemark"].ToString().Trim();
                            if (AirlineRemark.Equals("<>."))
                            {
                                AirlineRemark = "";
                            }
                            else if (AirlineRemark.Equals("<>"))
                            {
                                AirlineRemark = "";
                            }
                            else if (AirlineRemark.IndexOf("<>") != -1)
                            {
                                AirlineRemark = AirlineRemark.Replace("<>", "").Trim();
                            }

                            drSelect["RuleTarrif"] = AirlineRemark;
                            if (drSelect["PriceType"].ToString().IndexOf("SME") != -1)
                            {
                                drSelect["RuleTarrif"] = "Meal free|GST mandatory|" + AirlineRemark;
                            }

                            if (Convert.ToBoolean(dr["IsRefundable"].ToString()).Equals(true))
                            {
                                drSelect["RefundType"] = "N";
                            }
                            else
                            {
                                drSelect["RefundType"] = "Y";
                            }

                            drSelect["DepDate"] = Origin_DepTime;
                            drSelect["ArrDate"] = Destination_ArrTime;
                            drSelect["DepTime"] = Origin_DepTime;
                            drSelect["ArrTime"] = Destination_ArrTime;

                            drSelect["JourneyTime"] = Journey;
                            drSelect["Duration"] = Duration;

                            drSelect["ClassOfService"] = FareClass;
                            drSelect["FareBasisCode"] = FareRules_FareBasisCode;
                            drSelect["EquipmentType"] = Craft;

                            drSelect["DepartureTerminal"] = Origin_Terminal;
                            drSelect["ArrivalTerminal"] = Destination_Terminal;

                            drSelect["AdtTotalBasic"] = dAdt_Basic;
                            drSelect["AdtTotalTax"] = dAdt_Tax;
                            drSelect["AdtTotalFare"] = dAdt_Basic + dAdt_Tax;

                            drSelect["Adt_BASIC"] = dAdt_Basic;
                            drSelect["Adt_YQ"] = dAdt_YQ;
                            drSelect["Adt_CUTE"] = dCUTE;
                            drSelect["Adt_PSF"] = dPSF;
                            drSelect["Adt_UDF"] = dUDF;
                            drSelect["Adt_GST"] = dSVT;
                            drSelect["Adt_AUDF"] = dAUDF;
                            drSelect["Adt_TF"] = dTF;

                            Decimal dExtraTax = dAdt_Tax - (dAdt_YQ + dCUTE + dPSF + dUDF + dSVT + dAUDF + dTF);
                            drSelect["Adt_EX"] = dExtraTax + dOC;

                            if (iTotalChd > 0)
                            {
                                drSelect["ChdTotalBasic"] = dChd_Basic;
                                drSelect["ChdTotalTax"] = dChd_Tax;
                                drSelect["ChdTotalFare"] = dChd_Basic + dChd_Tax;

                                drSelect["Chd_BASIC"] = dChd_Basic;
                                drSelect["Chd_YQ"] = dChd_YQ;
                                drSelect["Chd_CUTE"] = dCUTE;
                                drSelect["Chd_PSF"] = dPSF;
                                drSelect["Chd_UDF"] = dUDF;
                                drSelect["Chd_GST"] = dSVT;
                                drSelect["Chd_AUDF"] = dAUDF;
                                drSelect["Chd_TF"] = dTF;

                                dExtraTax = dChd_Tax - (dChd_YQ + dCUTE + dPSF + dUDF + dSVT + dAUDF + dTF);
                                drSelect["Chd_EX"] = dExtraTax + dOC;
                            }

                            if (iTotalInf > 0)
                            {
                                drSelect["InfTotalBasic"] = dInf_Basic;
                                drSelect["InfTotalTax"] = dInf_Tax;
                                drSelect["InfTotalFare"] = dInf_Basic + dInf_Tax;

                                drSelect["Inf_BASIC"] = dInf_Basic;
                                drSelect["Inf_TAX"] = dInf_Tax;
                            }

                            drSelect["IsPriceChanged"] = IsPriceChanged;
                            drSelect["FareStatus"] = IsETicketEligible;
                            drSelect["FareQuote"] = "Updated";
                        }
                    }
                    dtFlights.AcceptChanges();
                    drSelect.SetModified();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(Companyid, 0, "FourthFilter", "air_tbo-SetFareModifiers", "", Searchid, errorMessage);
            }
        }
    }
}
