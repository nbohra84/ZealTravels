using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Infrastructure.TBO
{
    class SetAvailabilityModifierMC
    {
        public string errorMessage;
        //public DataTable dtOutbound;
        //public DataTable dtInbound;
        //public DataTable dtCombine;
        //-----------------------------------------------------------------------------------------------

        private string Supplierid;
        private string Searchid;
        private string Companyid;

        private string Tokenid;
        private string Traceid;

        private string Cabin;
        private string Sector;

        //-----------------------------------------------------------------------------------------------
        public SetAvailabilityModifierMC(string Supplierid, string Searchid, string Companyid, string Cabin, string Sector, string Tokenid)
        {
            this.Supplierid = Supplierid;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.Cabin = Cabin;
            this.Sector = Sector;
            this.Tokenid = Tokenid;
        }

        public DataTable FlightModifier(string Request, string Response, bool IsCombi)
        {
            DataTable dtFlights = DBCommon.Schema.SchemaFlights;

            try
            {
                if (Cabin.Equals("Y")) // 1 for All 2 for Economy 3 for PremiumEconomy 4 for Business 5 for PremiumBusiness 6 for First)
                {
                    Cabin = "1";
                }
                else if (Cabin.Equals("C"))
                {
                    Cabin = "4";
                }
                else if (Cabin.Equals("A"))
                {
                    Cabin = "3";
                }
                else if (Cabin.Equals("B"))
                {
                    Cabin = "5";
                }

                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(Response);
                Traceid = xmlflt.SelectSingleNode("Response/TraceId").InnerText;

                XmlElement root = xmlflt.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("Results/Results"); // You can also use XPath here

                int iRefID = 0;
                DataTable dtTemp = GetFlightModifierFunctions.Tempxml_Schema;
                foreach (XmlNode node in nodes)
                {
                    iRefID++;
                    FirstFilter(node.OuterXml, iRefID, dtTemp);
                }

                foreach (DataRow dr in dtTemp.Rows)
                {
                    SecondFilter(Request, dr, dtFlights);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "FlightModifier", "air_tbo-SetAvailabilityModifierMC", Supplierid, Searchid, errorMessage);
            }

            if (dtFlights != null && dtFlights.Rows.Count > 0)
            {
                if (IsCombi.Equals(true))
                {
                    dtFlights = GetFlightModifierFunctions.ValidateRTFlights(dtFlights);
                }
            }

            return dtFlights;
        }
        private void FirstFilter(string OuterXml, Int32 RefID, DataTable dtTemp)
        {
            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(OuterXml);

                string ResultIndex = xmlflt.SelectSingleNode("Results/ResultIndex").InnerText;
                string Source = xmlflt.SelectSingleNode("Results/Source").InnerText;
                string IsLCC = xmlflt.SelectSingleNode("Results/IsLCC").InnerText;
                string IsRefundable = xmlflt.SelectSingleNode("Results/IsRefundable").InnerText;
                string AirlineRemark = xmlflt.SelectSingleNode("Results/AirlineRemark").InnerText;

                string LastTicketDate = xmlflt.SelectSingleNode("Results/LastTicketDate").InnerText;
                string TicketAdvisory = xmlflt.SelectSingleNode("Results/TicketAdvisory").InnerText;
                string AirlineCode = xmlflt.SelectSingleNode("Results/AirlineCode").InnerText;
                string ValidatingAirline = xmlflt.SelectSingleNode("Results/ValidatingAirline").InnerText;

                string Fare = string.Empty;
                string FareBreakdown = string.Empty;
                string Segments = string.Empty;
                string FareRules = string.Empty;

                XmlNodeList nodesFare = xmlflt.SelectNodes("Results/Fare"); // You can also use XPath here
                foreach (XmlNode node in nodesFare)
                {
                    Fare = node.OuterXml;
                }

                FareBreakdown = "<Segments>";
                XmlNodeList nodesFareBreakdown = xmlflt.SelectNodes("Results/FareBreakdown"); // You can also use XPath here
                foreach (XmlNode node in nodesFareBreakdown)
                {
                    FareBreakdown += node.OuterXml;
                }
                FareBreakdown += "</Segments>";

                Segments = "<Segments>";
                XmlNodeList nodesSegments = xmlflt.SelectNodes("Results/Segments"); // You can also use XPath here
                foreach (XmlNode node in nodesSegments)
                {
                    Segments += node.OuterXml;
                }
                Segments += "</Segments>";

                FareRules = "<Segments>";
                XmlNodeList nodesFareRules = xmlflt.SelectNodes("Results/FareRules"); // You can also use XPath here
                foreach (XmlNode node in nodesFareRules)
                {
                    FareRules += node.OuterXml;
                }
                FareRules += "</Segments>";

                DataRow drAdd = dtTemp.NewRow();
                drAdd["ResultIndex"] = ResultIndex;
                drAdd["RefID"] = RefID;
                if (ResultIndex.IndexOf("IB") != -1)
                {
                    drAdd["RefID"] = RefID + 10000;
                }

                drAdd["Fare"] = Fare;
                drAdd["FareBreakdown"] = FareBreakdown;
                drAdd["Segments"] = Segments;
                drAdd["FareRules"] = FareRules;
                drAdd["Source"] = Source;
                drAdd["IsLCC"] = IsLCC;
                drAdd["IsRefundable"] = IsRefundable;
                drAdd["AirlineRemark"] = AirlineRemark;
                drAdd["LastTicketDate"] = LastTicketDate;
                drAdd["TicketAdvisory"] = TicketAdvisory;
                drAdd["AirlineCode"] = AirlineCode;
                drAdd["ValidatingAirline"] = ValidatingAirline;
                dtTemp.Rows.Add(drAdd);
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "FirstFilter", "air_tbo-SetAvailabilityModifierMC", Supplierid, Searchid, errorMessage);
            }
        }
        private void SecondFilter(string Request, DataRow dr, DataTable dtFlights)
        {
            DataSet dsRequest = GetCommonFunctions.StringToDataSet(Request);
            DataTable dtStation = dsRequest.Tables["AirSrchInfo"].Clone();
            dtStation.Columns.Add("row", typeof(System.Int32));

            int k = 1;
            foreach (DataRow drr in dsRequest.Tables["AirSrchInfo"].Rows)
            {
                DataRow drAdd = dtStation.NewRow();
                drAdd["DepartureStation"] = drr["DepartureStation"];
                drAdd["ArrivalStation"] = drr["ArrivalStation"];
                drAdd["row"] = k;
                dtStation.Rows.Add(drAdd);
                k++;
            }

            XmlDocument xmlflt = new XmlDocument();
            xmlflt.LoadXml(dr["Segments"].ToString());
            XmlNodeList nodesSegments = xmlflt.SelectNodes("Segments/Segments"); // You can also use XPath here
            foreach (XmlNode node in nodesSegments)
            {
                ThirdFilter(dtStation, node.OuterXml, dr, dtFlights);
            }

        }
        private void ThirdFilter(DataTable dtStation, string Segments, DataRow dr, DataTable dtFlights)
        {
            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(Segments);

                XmlNodeList nodesSegments = xmlflt.SelectNodes("Segments/Segments"); // You can also use XPath here
                foreach (XmlNode node in nodesSegments)
                {
                    FourthFilter(dtStation, node.OuterXml, dr, dtFlights);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "ThirdFilter", "air_tbo-SetAvailabilityModifierMC", Supplierid, Searchid, errorMessage);
            }
        }
        private void FourthFilter(DataTable dtStation, string Segments, DataRow dr, DataTable dtFlights)
        {
            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(Segments);

                XmlDocument xmlFR = new XmlDocument();
                xmlFR.LoadXml(dr["FareRules"].ToString());

                XmlDocument xmlFare = new XmlDocument();
                xmlFare.LoadXml(dr["Fare"].ToString());

                Int32 iTotalAdt = 0;
                Int32 iTotalChd = 0;
                Int32 iTotalInf = 0;

                string Baggage = xmlflt.SelectSingleNode("Segments/Baggage").InnerText.Trim();
                if (Baggage.Equals(string.Empty))
                {
                    Baggage = "0 K";
                }
                else
                {
                    Baggage = Baggage.ToUpper().Replace("KG", "K").Trim();
                }

                string CabinBaggage = xmlflt.SelectSingleNode("Segments/CabinBaggage").InnerText.Trim();
                if (CabinBaggage.Equals(string.Empty))
                {
                    CabinBaggage = "0 K";
                }
                else
                {
                    CabinBaggage = CabinBaggage.ToUpper().Replace("KG", "K").Trim();
                }

                string TripIndicator = xmlflt.SelectSingleNode("Segments/TripIndicator").InnerText;
                string SegmentIndicator = xmlflt.SelectSingleNode("Segments/SegmentIndicator").InnerText;

                string NoOfSeatAvailable = "0";
                if (xmlflt.SelectSingleNode("Segments/NoOfSeatAvailable") != null)
                {
                    NoOfSeatAvailable = xmlflt.SelectSingleNode("Segments/NoOfSeatAvailable").InnerText;
                }

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

                string Duration = xmlflt.SelectSingleNode("Segments/Duration").InnerText;
                string Journey = xmlflt.SelectSingleNode("Segments/Duration").InnerText;
                if (xmlflt.SelectSingleNode("Segments/AccumulatedDuration") != null)
                {
                    Journey = xmlflt.SelectSingleNode("Segments/AccumulatedDuration").InnerText;
                }

                string StopPointArrivalTime = xmlflt.SelectSingleNode("Segments/StopPointArrivalTime").InnerText;
                string StopPointDepartureTime = xmlflt.SelectSingleNode("Segments/StopPointDepartureTime").InnerText;
                string Craft = xmlflt.SelectSingleNode("Segments/Craft").InnerText;

                string IsETicketEligible = xmlflt.SelectSingleNode("Segments/IsETicketEligible").InnerText;
                string FlightStatus = xmlflt.SelectSingleNode("Segments/FlightStatus").InnerText;

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

                //Decimal dFare_BaseFare = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/BaseFare").InnerText.Trim().ToUpper());
                //Decimal dFare_Tax = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/Tax").InnerText.Trim().ToUpper());
                //Decimal dFare_YQTax = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/YQTax").InnerText.Trim().ToUpper());
                //Decimal dFare_OtherCharges = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/OtherCharges").InnerText.Trim().ToUpper());
                //Decimal dFare_PublishedFare = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/PublishedFare").InnerText.Trim().ToUpper());
                //Decimal dFare_CommissionEarned = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/CommissionEarned").InnerText.Trim().ToUpper());
                //Decimal dFare_OfferedFare = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/OfferedFare").InnerText.Trim().ToUpper());
                //Decimal dFare_TdsOnCommission = Convert.ToDecimal(xmlFare.SelectSingleNode("Fare/TdsOnCommission").InnerText.Trim().ToUpper());

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
                Commission = GetCommonFunctions.AvgAmount1(Commission.ToString());

                string FareRules_FareBasisCode = string.Empty;
                XmlNodeList nodeList1 = xmlFR.SelectNodes("Segments/FareRules");
                foreach (XmlNode no in nodeList1)
                {
                    if (no["Origin"].InnerText.Equals(Origin_AirportCode) && no["Destination"].InnerText.Equals(Destination_AirportCode))
                    {
                        FareRules_FareBasisCode = no["FareBasisCode"].InnerText;
                    }
                }

                DataRow drAdd = dtFlights.NewRow();
                drAdd["Sector"] = Sector;
                drAdd["PriceType"] = GetFlightModifierFunctions.GetPriceType(dr["Source"].ToString().Trim(), FareClass, AirlineCode, dr["AirlineRemark"].ToString().Trim(), FareRules_FareBasisCode);
                drAdd["BaggageDetail"] = Baggage + "*" + CabinBaggage;
                drAdd["AirlineID"] = Supplierid;
                drAdd["Adt"] = iTotalAdt;
                drAdd["Chd"] = iTotalChd;
                drAdd["Inf"] = iTotalInf;

                drAdd["Via"] = Via;
                drAdd["ViaName"] = ViaName;

                DataRow[] drSelect = dtStation.Select("row='" + TripIndicator + "'");
                if (drSelect.Length > 0)
                {
                    drAdd["FltType"] = "O";
                    drAdd["Origin"] = drSelect.CopyToDataTable().Rows[0]["DepartureStation"].ToString();
                    drAdd["Destination"] = drSelect.CopyToDataTable().Rows[0]["ArrivalStation"].ToString();
                }

                drAdd["OrderNo"] = SegmentIndicator;

                //if (TripIndicator.Equals("1"))
                //{

                //}

                //if (TripIndicator.Equals("2") || dr["ResultIndex"].ToString().IndexOf("IB") != -1)
                //{
                //    drAdd["FltType"] = "I";
                //    drAdd["Origin"] = ArrivalStation;
                //    drAdd["Destination"] = DepartureStation;
                //}

                if (Cabin.Equals("1"))
                {
                    drAdd["Cabin"] = "Economy";
                }
                else if (Cabin.Equals("2"))
                {
                    drAdd["Cabin"] = "Economy";
                }
                else if (Cabin.Equals("4"))
                {
                    drAdd["Cabin"] = "Business";
                }
                else if (Cabin.Equals("3"))
                {
                    drAdd["Cabin"] = "PremiumEconomy";
                }
                else if (Cabin.Equals("5"))
                {
                    drAdd["Cabin"] = "PremiumBusiness";
                }

                string AirlineRemark = dr["AirlineRemark"].ToString().Trim();
                if (AirlineRemark.Equals("<>."))
                {
                    AirlineRemark = "";
                }
                else if (AirlineRemark.Equals("<>"))
                {
                    AirlineRemark = "";
                }

                drAdd["RuleTarrif"] = AirlineRemark;
                if (drAdd["PriceType"].ToString().IndexOf("SME") != -1)
                {
                    drAdd["RuleTarrif"] = "Meal free|GST mandatory|" + AirlineRemark;
                }

                if (Convert.ToBoolean(dr["IsRefundable"].ToString()).Equals(true))
                {
                    drAdd["RefundType"] = "N";
                }
                else
                {
                    drAdd["RefundType"] = "Y";
                }

                drAdd["API_SearchID"] = Commission.ToString();
                drAdd["API_RefID"] = Convert.ToInt32(SegmentIndicator);
                drAdd["RefID"] = dr["RefID"];
                drAdd["BookingFareID"] = dr["ResultIndex"];

                drAdd["CarrierName"] = AirlineName;
                drAdd["CarrierCode"] = AirlineCode;
                drAdd["FlightNumber"] = FlightNumber;

                drAdd["DepartureStation"] = Origin_AirportCode;
                drAdd["ArrivalStation"] = Destination_AirportCode;

                drAdd["DepartureDate"] = Origin_DepTime;
                drAdd["ArrivalDate"] = Destination_ArrTime;
                drAdd["DepartureTime"] = Origin_DepTime;
                drAdd["ArrivalTime"] = Destination_ArrTime;

                drAdd["DepDate"] = Origin_DepTime;
                drAdd["ArrDate"] = Destination_ArrTime;
                drAdd["DepTime"] = Origin_DepTime;
                drAdd["ArrTime"] = Destination_ArrTime;

                drAdd["JourneyTime"] = Journey;
                drAdd["Duration"] = Duration;

                drAdd["ClassOfService"] = FareClass;
                drAdd["FareBasisCode"] = FareRules_FareBasisCode;

                drAdd["Api_SessionID"] = Tokenid;
                drAdd["JourneySellKey"] = Traceid;

                drAdd["EquipmentType"] = Craft;

                drAdd["SeatsAvailable"] = NoOfSeatAvailable;

                drAdd["DepartureTerminal"] = Origin_Terminal;
                drAdd["ArrivalTerminal"] = Destination_Terminal;

                drAdd["AdtTotalBasic"] = dAdt_Basic;
                drAdd["AdtTotalTax"] = dAdt_Tax;
                drAdd["AdtTotalFare"] = dAdt_Basic + dAdt_Tax;

                drAdd["Adt_BASIC"] = dAdt_Basic;
                drAdd["Adt_YQ"] = dAdt_YQ;
                drAdd["Adt_CUTE"] = dCUTE;
                drAdd["Adt_PSF"] = dPSF;
                drAdd["Adt_UDF"] = dUDF;
                drAdd["Adt_GST"] = dSVT;
                drAdd["Adt_AUDF"] = dAUDF;
                drAdd["Adt_TF"] = dTF;

                Decimal dExtraTax = dAdt_Tax - (dAdt_YQ + dCUTE + dPSF + dUDF + dSVT + dAUDF + dTF);
                drAdd["Adt_EX"] = dExtraTax + dOC;

                if (iTotalChd > 0)
                {
                    drAdd["ChdTotalBasic"] = dChd_Basic;
                    drAdd["ChdTotalTax"] = dChd_Tax;
                    drAdd["ChdTotalFare"] = dChd_Basic + dChd_Tax;

                    drAdd["Chd_BASIC"] = dChd_Basic;
                    drAdd["Chd_YQ"] = dChd_YQ;
                    drAdd["Chd_CUTE"] = dCUTE;
                    drAdd["Chd_PSF"] = dPSF;
                    drAdd["Chd_UDF"] = dUDF;
                    drAdd["Chd_GST"] = dSVT;
                    drAdd["Chd_AUDF"] = dAUDF;
                    drAdd["Chd_TF"] = dTF;

                    dExtraTax = dChd_Tax - (dChd_YQ + dCUTE + dPSF + dUDF + dSVT + dAUDF + dTF);
                    drAdd["Chd_EX"] = dExtraTax + dOC;
                }

                if (iTotalInf > 0)
                {
                    drAdd["InfTotalBasic"] = dInf_Basic;
                    drAdd["InfTotalTax"] = dInf_Tax;
                    drAdd["InfTotalFare"] = dInf_Basic + dInf_Tax;

                    drAdd["Inf_BASIC"] = dInf_Basic;
                    drAdd["Inf_TAX"] = dInf_Tax;
                }

                dtFlights.Rows.Add(drAdd);
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "FourthFilter", "air_tbo-SetAvailabilityModifierMC", Supplierid, Searchid, errorMessage);
            }
        }
    }
}
