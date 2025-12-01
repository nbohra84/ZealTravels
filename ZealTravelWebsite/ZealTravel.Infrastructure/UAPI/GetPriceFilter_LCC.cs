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
    class GetPriceFilter_LCC
    {
        public DataTable dtSSRItinerary;
        private int iRow;
        public string errorMessage;
        public void SetPriceUpdate(string SearchID, string CompanyID, DataTable dtBound, string PriceAvailability)
        {
            try
            {
                dtSSRItinerary = DBCommon.Schema.SchemaSSR;
                if (!dtSSRItinerary.Columns.Contains("AirSegment"))
                {
                    dtSSRItinerary.Columns.Add("AirSegment", typeof(string));
                }

                string strSelectedPrice = "";
                string strAirItinerary = "";
                ArrayList ArayAirPriceResult = new ArrayList();
                ArrayList ArayFareRule = new ArrayList();
                if (PriceAvailability.IndexOf("AirPriceRsp") != -1 && PriceAvailability.IndexOf("AirSegment") != -1)
                {
                    GetSeprateDataFromPriceResponse(SearchID, CompanyID, PriceAvailability, out strAirItinerary, out ArayAirPriceResult, out ArayFareRule);

                    DataSet dsAirItinerary = new DataSet();
                    if (strAirItinerary != null)
                    {
                        dsAirItinerary.ReadXml(new System.IO.StringReader(strAirItinerary));
                    }
                    if (ArayAirPriceResult != null && ArayAirPriceResult.Count > 0)
                    {
                        strSelectedPrice = GetLowestFareFromPriceResponse(SearchID, CompanyID, ArayAirPriceResult);
                    }


                    XmlDocument PriceDocument1 = new XmlDocument();
                    PriceDocument1.LoadXml(strSelectedPrice);
                    XmlElement PriceRoot1 = PriceDocument1.DocumentElement;
                    XmlAttributeCollection AttributeAirPricingSolutionInfo = PriceRoot1.Attributes;


                    ArrayList ArayAirPricingInfo = new ArrayList();
                    ArrayList ArayHostToken = new ArrayList();
                    ArrayList ArayOptionalServices = new ArrayList();

                    for (int n = 0; n < PriceRoot1.ChildNodes.Count; n++)
                    {
                        if (PriceRoot1.ChildNodes[n].OuterXml.IndexOf("AirPricingInfo") != -1)
                        {
                            ArayAirPricingInfo.Add(PriceRoot1.ChildNodes[n].OuterXml);
                        }
                        else if (PriceRoot1.ChildNodes[n].OuterXml.IndexOf("HostToken") != -1)
                        {
                            ArayHostToken.Add(PriceRoot1.ChildNodes[n].OuterXml);
                        }
                        else if (PriceRoot1.ChildNodes[n].OuterXml.IndexOf("OptionalServices") != -1)
                        {
                            ArayOptionalServices.Add(PriceRoot1.ChildNodes[n].OuterXml);
                        }
                    }

                    ArrayList ArayOptionalServicesssr = new ArrayList();

                    for (int i = 0; i < ArayOptionalServices.Count; i++)
                    {
                        DataTable dtpassengerType = new DataTable();

                        XmlDocument PriceDocument2 = new XmlDocument();
                        PriceDocument2.LoadXml(ArayOptionalServices[i].ToString());

                        for (int K = 0; K < PriceDocument2.ChildNodes[0].ChildNodes.Count; K++)
                        {
                            ArayOptionalServicesssr.Add(PriceDocument2.ChildNodes[0].ChildNodes[K].OuterXml);
                        }
                    }

                    for (int i = 0; i < ArayOptionalServicesssr.Count; i++)
                    {
                        SetSSR(SearchID, CompanyID, 0, ArayOptionalServicesssr[i].ToString(), dtBound.Rows[0]["FltType"].ToString());
                    }


                    if (dtBound.Rows.Count.Equals(dsAirItinerary.Tables["AirSegment"].Rows.Count))
                    {
                        foreach (DataRow dr in dtBound.Rows)
                        {
                            DataRow[] drFlts = dsAirItinerary.Tables["AirSegment"].Select("Origin='" + dr["DepartureStation"].ToString() + "' And Destination='" + dr["ArrivalStation"].ToString() + "' And Carrier='" + dr["CarrierCode"].ToString() + "' And FlightNumber='" + dr["FlightNumber"].ToString() + "'");
                            if (drFlts.Length > 0)
                            {
                                DataRow drAirSegment = drFlts.CopyToDataTable().Rows[0];


                                if (dsAirItinerary.Tables["Connection"] != null)
                                {
                                    DataRow[] drSegmentIndex2 = dsAirItinerary.Tables["Connection"].Select("AirSegment_Id='" + drAirSegment["AirSegment_Id"].ToString() + "'");
                                    if (drSegmentIndex2.Length > 0)
                                    {
                                        string segment = "";
                                        foreach (DataRow drSeg in drSegmentIndex2.CopyToDataTable().Rows)
                                        {
                                            segment += drSeg["SegmentIndex"].ToString() + "?";
                                        }
                                        segment = segment.Substring(0, segment.Length - 1);

                                        if (dr["FltType"].ToString().Equals("O"))
                                        {
                                            dr["Connection_O"] = segment;
                                        }
                                        else
                                        {
                                            dr["Connection_I"] = segment;
                                        }
                                    }
                                }

                                dr["SegmentRef"] = drAirSegment["Key"].ToString();
                                dr["Group"] = drAirSegment["Group"].ToString();
                                dr["CarrierCode"] = drAirSegment["Carrier"].ToString();
                                dr["FlightNumber"] = drAirSegment["FlightNumber"].ToString();
                                dr["DepartureStation"] = drAirSegment["Origin"].ToString();
                                dr["ArrivalStation"] = drAirSegment["Destination"].ToString();

                                dr["DepDate"] = drAirSegment["DepartureTime"].ToString();
                                dr["ArrDate"] = drAirSegment["ArrivalTime"].ToString();
                                dr["DepTime"] = drAirSegment["DepartureTime"].ToString();
                                dr["ArrTime"] = drAirSegment["ArrivalTime"].ToString();

                                if (drAirSegment.Table.Columns.Contains("APISRequirementsRef") && drAirSegment["APISRequirementsRef"].ToString().Length > 0)
                                {
                                    dr["TempData1"] = drAirSegment["APISRequirementsRef"].ToString().Trim();
                                }
                                else
                                {
                                    dr["TempData1"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("Status") && drAirSegment["Status"].ToString().Length > 0)
                                {
                                    dr["TempData2"] = drAirSegment["Status"].ToString().Trim();
                                }
                                else
                                {
                                    dr["TempData2"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("ChangeOfPlane") && drAirSegment["ChangeOfPlane"].ToString().Length > 0)
                                {
                                    dr["TempData3"] = drAirSegment["ChangeOfPlane"].ToString().Trim();
                                }
                                else
                                {
                                    dr["TempData3"] = "";
                                }


                                if (drAirSegment.Table.Columns.Contains("FlightTime") && drAirSegment["FlightTime"].ToString().Length > 0)
                                {
                                    dr["FlightTime"] = Convert.ToInt32(drAirSegment["FlightTime"].ToString().Trim());
                                }
                                else
                                {
                                    dr["FlightTime"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("Distance") && drAirSegment["Distance"].ToString().Length > 0)
                                {
                                    dr["Distance"] = Convert.ToInt32(drAirSegment["Distance"].ToString().Trim());
                                }
                                else
                                {
                                    dr["Distance"] = "";
                                }

                                //dr["FlightTime"] = drAirSegment["FlightTime"].ToString();
                                //dr["Distance"] = drAirSegment["Distance"].ToString();

                                if (drAirSegment.Table.Columns.Contains("ETicketability") && drAirSegment["ETicketability"].ToString().Length > 0)
                                {
                                    dr["ETicketability"] = Convert.ToInt32(drAirSegment["ETicketability"].ToString().Trim());
                                }
                                else
                                {
                                    dr["ETicketability"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("EquipmentType") && drAirSegment["EquipmentType"].ToString().Length > 0)
                                {
                                    dr["EquipmentType"] = drAirSegment["Equipment"].ToString();
                                }
                                else
                                {
                                    dr["EquipmentType"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("ChangeOfPlane") && drAirSegment["ChangeOfPlane"].ToString().Length > 0)
                                {
                                    dr["ChangeOfPlane"] = drAirSegment["ChangeOfPlane"].ToString();
                                }
                                else
                                {
                                    dr["ChangeOfPlane"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("ParticipantLevel") && drAirSegment["ParticipantLevel"].ToString().Length > 0)
                                {
                                    dr["ParticipantLevel"] = drAirSegment["ParticipantLevel"].ToString();
                                }
                                else
                                {
                                    dr["ParticipantLevel"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("LinkAvailability") && drAirSegment["LinkAvailability"].ToString().Length > 0)
                                {
                                    dr["LinkAvailability"] = drAirSegment["LinkAvailability"].ToString();
                                }
                                else
                                {
                                    dr["LinkAvailability"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("PolledAvailabilityOption") && drAirSegment["PolledAvailabilityOption"].ToString().Length > 0)
                                {
                                    dr["PolledAvailabilityOption"] = drAirSegment["PolledAvailabilityOption"].ToString();
                                }
                                else
                                {
                                    dr["PolledAvailabilityOption"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("OptionalServicesIndicator") && drAirSegment["OptionalServicesIndicator"].ToString().Length > 0)
                                {
                                    dr["OptionalServicesIndicator"] = drAirSegment["OptionalServicesIndicator"].ToString();
                                }
                                else
                                {
                                    dr["OptionalServicesIndicator"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("AvailabilitySource") && drAirSegment["AvailabilitySource"].ToString().Length > 0)
                                {
                                    dr["AvailabilitySource"] = drAirSegment["AvailabilitySource"].ToString();
                                }
                                else
                                {
                                    dr["AvailabilitySource"] = "";
                                }

                                if (drAirSegment.Table.Columns.Contains("AvailabilityDisplayType") && drAirSegment["AvailabilityDisplayType"].ToString().Length > 0)
                                {
                                    dr["AvailabilityDisplayType"] = drAirSegment["AvailabilityDisplayType"].ToString();
                                }
                                else
                                {
                                    dr["AvailabilityDisplayType"] = "";
                                }

                                if (dsAirItinerary.Tables["AirAvailInfo"] != null)
                                {
                                    DataRow[] drAirAvailInfo = dsAirItinerary.Tables["AirAvailInfo"].Select("AirSegment_Id='" + drAirSegment["AirSegment_Id"].ToString() + "'");
                                    if (drAirAvailInfo.Length > 0)
                                    {
                                        dr["ProviderCode"] = drAirAvailInfo.CopyToDataTable().Rows[0]["ProviderCode"].ToString();
                                    }
                                }

                                if (dsAirItinerary.Tables["FlightDetails"] != null)
                                {
                                    DataRow[] drFlightDetailsRef = dsAirItinerary.Tables["FlightDetails"].Select("AirSegment_Id='" + drAirSegment["AirSegment_Id"].ToString() + "'");
                                    if (drFlightDetailsRef.Length > 0)
                                    {
                                        dr["FlightDetailsRefKey"] = drFlightDetailsRef.CopyToDataTable().Rows[0]["Key"].ToString();

                                        DataRow[] drFlightDetails = dsAirItinerary.Tables["FlightDetails"].Select("Key='" + drFlightDetailsRef.CopyToDataTable().Rows[0]["Key"].ToString() + "'");
                                        if (drFlightDetails.Length > 0)
                                        {
                                            dr["TravelTime"] = drFlightDetails.CopyToDataTable().Rows[0]["TravelTime"].ToString();

                                            if (drFlightDetails.CopyToDataTable().Columns.Contains("OriginTerminal"))
                                            {
                                                dr["DepartureTerminal"] = drFlightDetails.CopyToDataTable().Rows[0]["OriginTerminal"].ToString();
                                            }
                                            if (drFlightDetails.CopyToDataTable().Columns.Contains("DestinationTerminal"))
                                            {
                                                dr["ArrivalTerminal"] = drFlightDetails.CopyToDataTable().Rows[0]["DestinationTerminal"].ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    dr["TravelTime"] = "";
                                    dr["FlightDetailsRefKey"] = "";
                                }


                                if (dsAirItinerary.Tables["CodeshareInfo"] != null)
                                {
                                    DataRow[] drCodeshareInfo = dsAirItinerary.Tables["CodeshareInfo"].Select("AirSegment_Id='" + drAirSegment["AirSegment_Id"].ToString() + "'");
                                    if (drCodeshareInfo.Length > 0)
                                    {
                                        dr["CodeshareInfoOperatingCarrier"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString();
                                        if (drCodeshareInfo.CopyToDataTable().Columns.Contains("OperatingFlightNumber"))
                                        {
                                            dr["CodeshareInfoOperatingFlightNumber"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingFlightNumber"].ToString();
                                        }

                                        dr["CodeshareInfo"] = drCodeshareInfo.CopyToDataTable().Rows[0]["CodeshareInfo_Text"].ToString();
                                    }
                                }

                                if (drAirSegment.Table.Columns.Contains("NumberOfStops") && drAirSegment["NumberOfStops"].ToString().Length > 0)
                                {
                                    dr["Via"] = Convert.ToInt32(drAirSegment["NumberOfStops"].ToString().Trim());
                                }
                                //===============================================================================================================================================

                                if (dsAirItinerary.Tables["AirPricingInfo"] != null)
                                {
                                    string AirPricingInfo_Id = dsAirItinerary.Tables["AirPricingInfo"].Rows[0]["AirPricingInfo_Id"].ToString();
                                    DataRow[] drChangePenalty = dsAirItinerary.Tables["ChangePenalty"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                                    if (drChangePenalty.Length > 0)
                                    {
                                        string DateChangeFee = "";
                                        foreach (DataRow drDCFee in drChangePenalty.CopyToDataTable().Rows)
                                        {
                                            if (drChangePenalty.CopyToDataTable().Columns.Contains("PenaltyApplies"))
                                            {
                                                if (drChangePenalty.CopyToDataTable().Columns.Contains("Amount") && drDCFee["Amount"].ToString().Trim().Length > 0)
                                                {
                                                    DateChangeFee += drDCFee["PenaltyApplies"].ToString() + ":" + drDCFee["Amount"].ToString() + "?";
                                                }
                                                else if (drChangePenalty.CopyToDataTable().Columns.Contains("Percentage") && drChangePenalty.CopyToDataTable().Columns.Contains("Percentage"))
                                                {
                                                    DateChangeFee += drDCFee["PenaltyApplies"].ToString() + ":" + drDCFee["Percentage"].ToString() + "?";
                                                }
                                            }
                                        }
                                        dr["DateChangeFee"] = DateChangeFee;
                                    }


                                    DataRow[] drCancelPenalty = dsAirItinerary.Tables["CancelPenalty"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                                    if (drChangePenalty.Length > 0)
                                    {
                                        string DateChangeFee = "";
                                        foreach (DataRow drDCFee in drChangePenalty.CopyToDataTable().Rows)
                                        {
                                            if (drChangePenalty.CopyToDataTable().Columns.Contains("PenaltyApplies"))
                                            {
                                                if (drChangePenalty.CopyToDataTable().Columns.Contains("Amount") && drDCFee["Amount"].ToString().Trim().Length > 0)
                                                {
                                                    DateChangeFee += drDCFee["PenaltyApplies"].ToString() + ":" + drDCFee["Amount"].ToString() + "?";
                                                }
                                                else if (drChangePenalty.CopyToDataTable().Columns.Contains("Percentage") && drChangePenalty.CopyToDataTable().Columns.Contains("Percentage"))
                                                {
                                                    DateChangeFee += drDCFee["PenaltyApplies"].ToString() + ":" + drDCFee["Percentage"].ToString() + "?";
                                                }
                                            }
                                        }
                                        dr["CancellationFee"] = DateChangeFee;
                                    }
                                }

                                //===============================================================================================================================================
                                string FlightStatus = "";
                                bool IsPriceChanged = true;
                                bool IsTimeChanged = true;
                                bool IsETicketEligible = true;
                                string Status = "";
                                string ResponseStatus = "";
                                int ErrorCode = 0;
                                string ErrorMessage = "";
                                bool IsUpdated = true;
                                string SearchCriteria = "";

                                dr["FareQuote"] = CommonUapi.FareQuote_rr(CompanyID, SearchID, SearchCriteria, FlightStatus, IsPriceChanged, IsTimeChanged, IsETicketEligible, Status, ResponseStatus, ErrorCode, ErrorMessage, IsUpdated);
                            }
                            dr.AcceptChanges();
                        }
                    }

                    dtBound.AcceptChanges();

                    //-----------------------------------------------------------------------------------------------------------------------------
                    for (int i = 0; i < ArayAirPricingInfo.Count; i++)
                    {
                        DataTable dtpassengerType = new DataTable();

                        XmlDocument PriceDocument2 = new XmlDocument();
                        PriceDocument2.LoadXml(ArayAirPricingInfo[i].ToString());
                        XmlElement PriceRoot2 = PriceDocument2.DocumentElement;
                        for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                        {
                            if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("PassengerType") != -1 && PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") == -1)
                            {
                                DataSet dsTemp = new DataSet();
                                dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                                if (dsTemp != null && dsTemp.Tables["PassengerType"] != null)
                                {
                                    dtpassengerType = dsTemp.Tables["PassengerType"].Copy(); 
                                    break;
                                }
                            }
                        }

                        //-----------------------------------------------------------------------------------------------------------------------------

                        //ArrayList ArayHToken = new ArrayList();
                        //string PaxType = dtpassengerType.Rows[0]["Code"].ToString();
                        //for (int k = 0; k < ArayHostToken.Count; k++)
                        //{
                        //    if (ArayHostToken[k].ToString().IndexOf(PaxType) != -1)
                        //    {
                        //        ArayHToken.Add(ArayHostToken[k].ToString());
                        //    }
                        //}

                        string Token = "";
                        for (int k = 0; k < ArayHostToken.Count; k++)
                        {
                            XmlDocument XmlDocumentHostToken = new XmlDocument();
                            XmlDocumentHostToken.LoadXml(ArayHostToken[k].ToString());

                            XmlElement HostTokenXmlElement = XmlDocumentHostToken.DocumentElement;
                            XmlAttributeCollection AttributeHostToken = HostTokenXmlElement.Attributes;

                            string key = AttributeHostToken["Key"].Value.ToString();
                            string text = HostTokenXmlElement.OwnerDocument.InnerText;

                            Token += key + "@" + text + "?";
                        }

                        foreach (DataRow drToken in dtBound.Rows)
                        {
                            drToken["HostTokenRef"] = Token;
                        }

                        if (dtpassengerType.Rows[0]["Code"].ToString().Equals("ADT"))
                        {
                            SetFare_ADT(SearchID, CompanyID, ArayAirPricingInfo[i].ToString(), AttributeAirPricingSolutionInfo, dtBound);
                        }
                        else if (dtpassengerType.Rows[0]["Code"].ToString().Equals("CHD") || dtpassengerType.Rows[0]["Code"].ToString().Equals("CNN"))
                        {
                            SetFare_CHD(SearchID, CompanyID, ArayAirPricingInfo[i].ToString(), AttributeAirPricingSolutionInfo, dtBound);
                        }
                        if (dtpassengerType.Rows[0]["Code"].ToString().Equals("INF"))
                        {
                            SetFare_INF(SearchID, CompanyID, ArayAirPricingInfo[i].ToString(), AttributeAirPricingSolutionInfo, dtBound);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceFilter_GDS-SetPriceUpdate", "air_uapi", PriceAvailability, SearchID, ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------
        private void SetSSR(string SearchID, string CompanyID, int BookingRef, string SSRtext, string FltType)
        {
            try
            {
                if (SSRtext.IndexOf("MealOrBeverage") != -1 || SSRtext.IndexOf("Baggage") != -1)
                {
                    DataSet dsSSRItinerary = new DataSet();
                    dsSSRItinerary.ReadXml(new System.IO.StringReader(SSRtext));

                    iRow++;

                    bool IsInternational = false;
                    if (dsSSRItinerary != null && dsSSRItinerary.Tables["Description"] != null)
                    {
                        foreach (DataRow dr in dsSSRItinerary.Tables["Description"].Rows)
                        {
                            if (dr["Description_Text"].ToString().ToLower().IndexOf("international connections baggage") != -1)
                            {
                                IsInternational = true;
                            }
                        }
                    }

                    if (dsSSRItinerary != null)
                    {
                        string BookingTravelerRef = "BTF-" + dsSSRItinerary.Tables["ServiceData"].Rows[0]["BookingTravelerRef"].ToString();
                        DataRow dr = dsSSRItinerary.Tables["OptionalService"].Rows[0];
                        DataRow drAdd = dtSSRItinerary.NewRow();
                        drAdd["AirSegment"] = dsSSRItinerary.Tables["ServiceData"].Rows[0]["AirSegmentRef"].ToString();
                        drAdd["RowID"] = iRow;
                        drAdd["CarrierCode"] = dr["SupplierCode"].ToString();
                        drAdd["Code"] = dr["ProviderDefinedType"].ToString();
                        drAdd["FltType"] = FltType;

                        if (SSRtext.IndexOf("MealOrBeverage") != -1)
                        {
                            drAdd["CodeType"] = "M";
                            drAdd["Description"] = dr["DisplayText"].ToString();
                        }
                        else
                        {
                            drAdd["CodeType"] = "B";
                            if (IsInternational)
                            {
                                drAdd["Description"] = dr["DisplayText"].ToString() + "- International Connections baggage";
                            }
                            else
                            {
                                drAdd["Description"] = dr["DisplayText"].ToString();
                            }
                        }
                        drAdd["Amount"] = Decimal.ToInt32(Convert.ToDecimal(dr["TotalPrice"].ToString().Trim().Replace("INR", "").Trim()));

                        drAdd["Detail"] = BookingTravelerRef + "?" + dr["Key"].ToString() + "?" + dr["OptionalServicesRuleRef"].ToString();
                        dtSSRItinerary.Rows.Add(drAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SetSSR", "GetPriceFilter_6E_NEW", "", SearchID, ex.Message);
            }
        }
        private void SetFare_ADT(string SearchID, string CompanyID, string SelectedPriceResp, XmlAttributeCollection AttributeAirPricingSolutionInfo, DataTable dtBound)
        {
            try
            {
                DataTable dtFareRuleKey = new DataTable();
                DataTable dtBookingInfo = new DataTable();
                DataTable dtTaxInfo = new DataTable();

                XmlDocument PriceDocument2 = new XmlDocument();
                PriceDocument2.LoadXml(SelectedPriceResp);
                XmlElement PriceRoot2 = PriceDocument2.DocumentElement;
                XmlAttributeCollection AttributeAirPricingInfo = PriceRoot2.Attributes;

                int k = 0;
                int k1 = 0;
                int k3 = 0;
                for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                {
                    if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BaggageAllowances") == -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                        if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BookingInfo") != -1)
                        {
                            if (k1.Equals(0))
                            {
                                k1++;
                                dtBookingInfo = dsTemp.Tables[0].Clone();
                                dtBookingInfo = dsTemp.Tables[0].Copy();
                            }
                            else
                            {
                                dtBookingInfo.Merge(dsTemp.Tables[0].Copy());
                            }
                        }
                        else if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") != -1)
                        {
                            if (k3.Equals(0))
                            {
                                k3++;
                                dtFareRuleKey = dsTemp.Tables["FareRuleKey"].Clone();
                                dtFareRuleKey = dsTemp.Tables["FareRuleKey"].Copy();
                            }
                            else
                            {
                                dtFareRuleKey.Merge(dsTemp.Tables["FareRuleKey"].Copy());
                            }
                        }
                        else if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("TaxInfo") != -1)
                        {
                            if (k.Equals(0))
                            {
                                k++;
                                dtTaxInfo = dsTemp.Tables[0].Clone();
                                dtTaxInfo = dsTemp.Tables[0].Copy();
                            }
                            else
                            {
                                dtTaxInfo.Merge(dsTemp.Tables[0].Copy());
                            }
                        }
                    }
                }


                foreach (DataRow drBookingInfo in dtBookingInfo.Rows)
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        if (dr["SegmentRef"].ToString().Equals(drBookingInfo["SegmentRef"].ToString()))
                        {
                            dr["AdtFareInfoRef"] = drBookingInfo["FareInfoRef"];
                            dr["ClassOfService"] = drBookingInfo["BookingCode"];
                            dr["AdtHTR"] = drBookingInfo["HostTokenRef"];
                        }
                    }
                }
                foreach (DataRow drFareRuleKey in dtFareRuleKey.Rows)
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        if (dr["AdtFareInfoRef"].ToString().Equals(drFareRuleKey["FareInfoRef"].ToString()))
                        {
                            dr["FareRuleInfo_Text"] = drFareRuleKey["FareRuleKey_text"];
                        }
                    }
                }

                string AdtFareRuleKey = "";

                for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                {
                    if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BaggageAllowances") == -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                        if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") != -1)
                        {
                            if (dsTemp.Tables["FareInfo"] != null && dsTemp.Tables["FareInfo"].Rows.Count > 0)
                            {
                                foreach (DataRow drFareInfo in dsTemp.Tables["FareRuleKey"].Rows)
                                {
                                    AdtFareRuleKey += drFareInfo["FareInfoRef"].ToString() + "@" + drFareInfo["FareRuleKey_Text"].ToString() + "?";
                                }

                                foreach (DataRow drFareInfo in dsTemp.Tables["FareInfo"].Rows)
                                {
                                    foreach (DataRow dr in dtBound.Rows)
                                    {
                                        if (dr["AdtFareInfoRef"].ToString().Equals(drFareInfo["Key"].ToString()))
                                        {
                                            string data = "PassengerTypeCode:" + drFareInfo["PassengerTypeCode"].ToString() + "?";
                                            data += "Origin:" + drFareInfo["Origin"].ToString() + "?";
                                            data += "Destination:" + drFareInfo["Destination"].ToString() + "?";



                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("EffectiveDate"))
                                            {
                                                data += "EffectiveDate:" + drFareInfo["EffectiveDate"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "EffectiveDate:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("DepartureDate"))
                                            {
                                                data += "DepartureDate:" + drFareInfo["DepartureDate"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "DepartureDate:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("Amount"))
                                            {
                                                data += "Amount:" + drFareInfo["Amount"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "Amount:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NegotiatedFare"))
                                            {
                                                data += "NegotiatedFare:" + drFareInfo["NegotiatedFare"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NegotiatedFare:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NotValidBefore"))
                                            {
                                                data += "NotValidBefore:" + drFareInfo["NotValidBefore"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NotValidBefore:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NotValidAfter"))
                                            {
                                                data += "NotValidAfter:" + drFareInfo["NotValidAfter"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NotValidAfter:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("TaxAmount"))
                                            {
                                                data += "TaxAmount:" + drFareInfo["TaxAmount"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "TaxAmount:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("PromotionalFare"))
                                            {
                                                data += "PromotionalFare:" + drFareInfo["PromotionalFare"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "PromotionalFare:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("FareFamily"))
                                            {
                                                data += "FareFamily:" + drFareInfo["FareFamily"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "FareFamily:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("SupplierCode"))
                                            {
                                                data += "SupplierCode:" + drFareInfo["SupplierCode"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "SupplierCode:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("FareBasis"))
                                            {
                                                data += "FareBasis:" + drFareInfo["FareBasis"].ToString();
                                            }
                                            else
                                            {
                                                data += "FareBasis:" + "";
                                            }

                                            dr["AdtFareInfoRef_data"] = data;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (DataRow drAdd in dtBound.Rows)
                {
                    drAdd["BookingFareID"] = AdtFareRuleKey;
                    drAdd["AirPricePointKey"] = AttributeAirPricingSolutionInfo["Key"].Value.ToString();
                    drAdd["PricingMethod"] = AttributeAirPricingInfo["PricingMethod"].Value.ToString();
                    drAdd["AdtAirPricingInfoKey"] = AttributeAirPricingInfo["Key"].Value.ToString();

                    Int32 discount = 0;
                    if (AttributeAirPricingInfo["Fees"] != null)
                    {
                        discount = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["Fees"].Value.Replace("INR", "").Trim()));
                    }
                    else if (AttributeAirPricingInfo["ApproximateFees"] != null)
                    {
                        discount = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["ApproximateFees"].Value.Replace("INR", "").Trim()));
                    }


                    if (AttributeAirPricingInfo["BasePrice"] != null)
                    {
                        drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["BasePrice"].Value.ToString().Replace("INR", "").Trim())) + discount;
                    }
                    else if (AttributeAirPricingInfo["ApproximateBasePrice"] != null)
                    {
                        drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["ApproximateBasePrice"].Value.ToString().Replace("INR", "").Trim())) + discount;
                    }

                    drAdd["AdtTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["Taxes"].Value.ToString().Replace("INR", "").Trim()));

                    //if (Convert.ToBoolean(AttributeAirPricingInfo["Refundable"].Value))
                    //{
                    //    drAdd["RefundType"] = "N";
                    //}
                    //else
                    //{
                    //    drAdd["RefundType"] = "Y";
                    //}

                    int K3 = 0;
                    int YR = 0;
                    int YQ = 0;
                    int WO = 0;
                    int JN = 0;
                    int IN = 0;
                    int OT = 0;

                    foreach (DataRow drTax in dtTaxInfo.Rows)
                    {
                        if (drTax["Category"].ToString().Trim().Equals("K3"))
                        {
                            K3 += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("YR"))
                        {
                            YR += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("YQ"))
                        {
                            YQ += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("WO"))
                        {
                            WO += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("JN"))
                        {
                            JN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
                        {
                            IN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else
                        {
                            OT += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                    }

                    if (discount > 0)
                    {
                        OT += Decimal.ToInt32(Convert.ToDecimal(discount));
                    }

                    drAdd["Adt_YQ"] = YQ;
                    drAdd["Adt_PSF"] = WO;
                    drAdd["Adt_UDF"] = IN;
                    drAdd["Adt_AUDF"] = 0;
                    drAdd["Adt_CUTE"] = YR;
                    drAdd["Adt_GST"] = K3;
                    drAdd["Adt_TF"] = JN;
                    drAdd["Adt_CESS"] = 0;
                    drAdd["Adt_EX"] = OT;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceFilter_GDS-SetFare_ADT", "air_uapi", "", SearchID, ex.Message);
            }
        }
        private void SetFare_CHD(string SearchID, string CompanyID, string SelectedPriceResp, XmlAttributeCollection AttributeAirPricingSolutionInfo, DataTable dtBound)
        {
            try
            {
                DataTable dtFareRuleKey = new DataTable();
                DataTable dtBookingInfo = new DataTable();
                DataTable dtTaxInfo = new DataTable();

                XmlDocument PriceDocument2 = new XmlDocument();
                PriceDocument2.LoadXml(SelectedPriceResp);
                XmlElement PriceRoot2 = PriceDocument2.DocumentElement;
                XmlAttributeCollection AttributeAirPricingInfo = PriceRoot2.Attributes;

                int k = 0;
                int k1 = 0;
                int k3 = 0;
                for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                {
                    if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BaggageAllowances") == -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                        if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BookingInfo") != -1)
                        {
                            if (k1.Equals(0))
                            {
                                k1++;
                                dtBookingInfo = dsTemp.Tables[0].Clone();
                                dtBookingInfo = dsTemp.Tables[0].Copy();
                            }
                            else
                            {
                                dtBookingInfo.Merge(dsTemp.Tables[0].Copy());
                            }
                        }
                        else if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") != -1)
                        {
                            if (k3.Equals(0))
                            {
                                k3++;
                                dtFareRuleKey = dsTemp.Tables["FareRuleKey"].Clone();
                                dtFareRuleKey = dsTemp.Tables["FareRuleKey"].Copy();
                            }
                            else
                            {
                                dtFareRuleKey.Merge(dsTemp.Tables["FareRuleKey"].Copy());
                            }
                        }
                        else if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("TaxInfo") != -1)
                        {
                            if (k.Equals(0))
                            {
                                k++;
                                dtTaxInfo = dsTemp.Tables[0].Clone();
                                dtTaxInfo = dsTemp.Tables[0].Copy();
                            }
                            else
                            {
                                dtTaxInfo.Merge(dsTemp.Tables[0].Copy());
                            }
                        }
                    }
                }


                foreach (DataRow drBookingInfo in dtBookingInfo.Rows)
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        if (dr["SegmentRef"].ToString().Equals(drBookingInfo["SegmentRef"].ToString()))
                        {
                            dr["ChdFareInfoRef"] = drBookingInfo["FareInfoRef"];
                            dr["ChdHTR"] = drBookingInfo["HostTokenRef"];
                        }
                    }
                }
                foreach (DataRow drFareRuleKey in dtFareRuleKey.Rows)
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        if (dr["ChdFareInfoRef"].ToString().Equals(drFareRuleKey["FareInfoRef"].ToString()))
                        {
                            dr["FareRuleInfo_Text"] = drFareRuleKey["FareRuleKey_text"];
                        }
                    }
                }


                string ChdFareRuleKey = "";

                for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                {
                    if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BaggageAllowances") == -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                        if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") != -1)
                        {
                            if (dsTemp.Tables["FareInfo"] != null && dsTemp.Tables["FareInfo"].Rows.Count > 0)
                            {
                                foreach (DataRow drFareInfo in dsTemp.Tables["FareRuleKey"].Rows)
                                {
                                    ChdFareRuleKey += drFareInfo["FareInfoRef"].ToString() + "@" + drFareInfo["FareRuleKey_Text"].ToString() + "?";
                                }

                                foreach (DataRow drFareInfo in dsTemp.Tables["FareInfo"].Rows)
                                {
                                    foreach (DataRow dr in dtBound.Rows)
                                    {
                                        if (dr["ChdFareInfoRef"].ToString().Equals(drFareInfo["Key"].ToString()))
                                        {
                                            string data = "PassengerTypeCode:" + drFareInfo["PassengerTypeCode"].ToString() + "?";
                                            data += "Origin:" + drFareInfo["Origin"].ToString() + "?";
                                            data += "Destination:" + drFareInfo["Destination"].ToString() + "?";
                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("EffectiveDate"))
                                            {
                                                data += "EffectiveDate:" + drFareInfo["EffectiveDate"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "EffectiveDate:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("DepartureDate"))
                                            {
                                                data += "DepartureDate:" + drFareInfo["DepartureDate"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "DepartureDate:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("Amount"))
                                            {
                                                data += "Amount:" + drFareInfo["Amount"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "Amount:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NegotiatedFare"))
                                            {
                                                data += "NegotiatedFare:" + drFareInfo["NegotiatedFare"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NegotiatedFare:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NotValidBefore"))
                                            {
                                                data += "NotValidBefore:" + drFareInfo["NotValidBefore"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NotValidBefore:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NotValidAfter"))
                                            {
                                                data += "NotValidAfter:" + drFareInfo["NotValidAfter"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NotValidAfter:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("TaxAmount"))
                                            {
                                                data += "TaxAmount:" + drFareInfo["TaxAmount"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "TaxAmount:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("PromotionalFare"))
                                            {
                                                data += "PromotionalFare:" + drFareInfo["PromotionalFare"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "PromotionalFare:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("FareFamily"))
                                            {
                                                data += "FareFamily:" + drFareInfo["FareFamily"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "FareFamily:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("SupplierCode"))
                                            {
                                                data += "SupplierCode:" + drFareInfo["SupplierCode"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "SupplierCode:" + "" + "?";
                                            }


                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("FareBasis"))
                                            {
                                                data += "FareBasis:" + drFareInfo["FareBasis"].ToString();
                                            }
                                            else
                                            {
                                                data += "FareBasis:" + "";
                                            }
                                            dr["ChdFareInfoRef_data"] = data;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (DataRow drAdd in dtBound.Rows)
                {
                    drAdd["API_AirlineID"] = ChdFareRuleKey;
                    drAdd["AirPricePointKey"] = AttributeAirPricingSolutionInfo["Key"].Value.ToString();
                    drAdd["PricingMethod"] = AttributeAirPricingInfo["PricingMethod"].Value.ToString();
                    drAdd["ChdAirPricingInfoKey"] = AttributeAirPricingInfo["Key"].Value.ToString();

                    Int32 discount = 0;
                    if (AttributeAirPricingInfo["Fees"] != null)
                    {
                        discount = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["Fees"].Value.Replace("INR", "").Trim()));
                    }
                    else if (AttributeAirPricingInfo["ApproximateFees"] != null)
                    {
                        discount = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["ApproximateFees"].Value.Replace("INR", "").Trim()));
                    }


                    if (AttributeAirPricingInfo["BasePrice"] != null)
                    {
                        drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["BasePrice"].Value.ToString().Replace("INR", "").Trim())) + discount;
                    }
                    else if (AttributeAirPricingInfo["ApproximateBasePrice"] != null)
                    {
                        drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["ApproximateBasePrice"].Value.ToString().Replace("INR", "").Trim())) + discount;
                    }

                    drAdd["ChdTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["Taxes"].Value.ToString().Replace("INR", "").Trim()));

                    int K3 = 0;
                    int YR = 0;
                    int YQ = 0;
                    int WO = 0;
                    int JN = 0;
                    int IN = 0;
                    int OT = 0;

                    foreach (DataRow drTax in dtTaxInfo.Rows)
                    {
                        if (drTax["Category"].ToString().Trim().Equals("K3"))
                        {
                            K3 += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("YR"))
                        {
                            YR += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("YQ"))
                        {
                            YQ += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("WO"))
                        {
                            WO += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("JN"))
                        {
                            JN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
                        {
                            IN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                        else
                        {
                            OT += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
                        }
                    }

                    if (discount > 0)
                    {
                        OT += Decimal.ToInt32(Convert.ToDecimal(discount));
                    }

                    drAdd["Chd_YQ"] = YQ;
                    drAdd["Chd_PSF"] = WO;
                    drAdd["Chd_UDF"] = IN;
                    drAdd["Chd_AUDF"] = 0;
                    drAdd["Chd_CUTE"] = YR;
                    drAdd["Chd_GST"] = K3;
                    drAdd["Chd_TF"] = JN;
                    drAdd["Chd_CESS"] = 0;
                    drAdd["Chd_EX"] = OT;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceFilter_GDS-SetFare_CHD", "air_uapi", "", SearchID, ex.Message);
            }
        }
        private void SetFare_INF(string SearchID, string CompanyID, string SelectedPriceResp, XmlAttributeCollection AttributeAirPricingSolutionInfo, DataTable dtBound)
        {
            try
            {
                DataTable dtFareRuleKey = new DataTable();
                DataTable dtBookingInfo = new DataTable();
                DataTable dtTaxInfo = new DataTable();

                XmlDocument PriceDocument2 = new XmlDocument();
                PriceDocument2.LoadXml(SelectedPriceResp);
                XmlElement PriceRoot2 = PriceDocument2.DocumentElement;
                XmlAttributeCollection AttributeAirPricingInfo = PriceRoot2.Attributes;

                int k = 0;
                int k1 = 0;
                int k3 = 0;
                for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                {
                    if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BaggageAllowances") == -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                        if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BookingInfo") != -1)
                        {
                            if (k1.Equals(0))
                            {
                                k1++;
                                dtBookingInfo = dsTemp.Tables[0].Clone();
                                dtBookingInfo = dsTemp.Tables[0].Copy();
                            }
                            else
                            {
                                dtBookingInfo.Merge(dsTemp.Tables[0].Copy());
                            }
                        }
                        else if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") != -1)
                        {
                            if (k3.Equals(0))
                            {
                                k3++;
                                dtFareRuleKey = dsTemp.Tables["FareRuleKey"].Clone();
                                dtFareRuleKey = dsTemp.Tables["FareRuleKey"].Copy();
                            }
                            else
                            {
                                dtFareRuleKey.Merge(dsTemp.Tables["FareRuleKey"].Copy());
                            }
                        }
                        else if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("TaxInfo") != -1)
                        {
                            if (k.Equals(0))
                            {
                                k++;
                                dtTaxInfo = dsTemp.Tables[0].Clone();
                                dtTaxInfo = dsTemp.Tables[0].Copy();
                            }
                            else
                            {
                                dtTaxInfo.Merge(dsTemp.Tables[0].Copy());
                            }
                        }
                    }
                }


                foreach (DataRow drBookingInfo in dtBookingInfo.Rows)
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        if (dr["SegmentRef"].ToString().Equals(drBookingInfo["SegmentRef"].ToString()))
                        {
                            dr["InfFareInfoRef"] = drBookingInfo["FareInfoRef"];
                            dr["InfHTR"] = drBookingInfo["HostTokenRef"];
                        }
                    }
                }
                foreach (DataRow drFareRuleKey in dtFareRuleKey.Rows)
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        if (dr["InfFareInfoRef"].ToString().Equals(drFareRuleKey["FareInfoRef"].ToString()))
                        {
                            dr["FareRuleInfo_Text"] = drFareRuleKey["FareRuleKey_text"];
                        }
                    }
                }

                string InfFareRuleKey = "";
                for (int n = 0; n < PriceRoot2.ChildNodes.Count; n++)
                {
                    if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("BaggageAllowances") == -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.ReadXml(new System.IO.StringReader(PriceRoot2.ChildNodes[n].OuterXml));

                        if (PriceRoot2.ChildNodes[n].OuterXml.IndexOf("FareInfo") != -1)
                        {
                            if (dsTemp.Tables["FareInfo"] != null && dsTemp.Tables["FareInfo"].Rows.Count > 0)
                            {
                                foreach (DataRow drFareInfo in dsTemp.Tables["FareRuleKey"].Rows)
                                {
                                    InfFareRuleKey += drFareInfo["FareInfoRef"].ToString() + "@" + drFareInfo["FareRuleKey_Text"].ToString() + "?";
                                }

                                foreach (DataRow drFareInfo in dsTemp.Tables["FareInfo"].Rows)
                                {
                                    foreach (DataRow dr in dtBound.Rows)
                                    {
                                        if (dr["InfFareInfoRef"].ToString().Equals(drFareInfo["Key"].ToString()))
                                        {
                                            string data = "PassengerTypeCode:" + drFareInfo["PassengerTypeCode"].ToString() + "?";
                                            data += "Origin:" + drFareInfo["Origin"].ToString() + "?";
                                            data += "Destination:" + drFareInfo["Destination"].ToString() + "?";
                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("EffectiveDate"))
                                            {
                                                data += "EffectiveDate:" + drFareInfo["EffectiveDate"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "EffectiveDate:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("DepartureDate"))
                                            {
                                                data += "DepartureDate:" + drFareInfo["DepartureDate"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "DepartureDate:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("Amount"))
                                            {
                                                data += "Amount:" + drFareInfo["Amount"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "Amount:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NegotiatedFare"))
                                            {
                                                data += "NegotiatedFare:" + drFareInfo["NegotiatedFare"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NegotiatedFare:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NotValidBefore"))
                                            {
                                                data += "NotValidBefore:" + drFareInfo["NotValidBefore"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NotValidBefore:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("NotValidAfter"))
                                            {
                                                data += "NotValidAfter:" + drFareInfo["NotValidAfter"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "NotValidAfter:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("TaxAmount"))
                                            {
                                                data += "TaxAmount:" + drFareInfo["TaxAmount"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "TaxAmount:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("PromotionalFare"))
                                            {
                                                data += "PromotionalFare:" + drFareInfo["PromotionalFare"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "PromotionalFare:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("FareFamily"))
                                            {
                                                data += "FareFamily:" + drFareInfo["FareFamily"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "FareFamily:" + "" + "?";
                                            }

                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("SupplierCode"))
                                            {
                                                data += "SupplierCode:" + drFareInfo["SupplierCode"].ToString() + "?";
                                            }
                                            else
                                            {
                                                data += "SupplierCode:" + "" + "?";
                                            }


                                            if (dsTemp.Tables["FareInfo"].Columns.Contains("FareBasis"))
                                            {
                                                data += "FareBasis:" + drFareInfo["FareBasis"].ToString();
                                            }
                                            else
                                            {
                                                data += "FareBasis:" + "";
                                            }

                                            dr["InfFareInfoRef_data"] = data;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (DataRow drAdd in dtBound.Rows)
                {
                    drAdd["API_BookingFareID"] = InfFareRuleKey;
                    drAdd["AirPricePointKey"] = AttributeAirPricingSolutionInfo["Key"].Value.ToString();
                    drAdd["PricingMethod"] = AttributeAirPricingInfo["PricingMethod"].Value.ToString();
                    drAdd["InfAirPricingInfoKey"] = AttributeAirPricingInfo["Key"].Value.ToString();

                    if (AttributeAirPricingInfo["BasePrice"] != null)
                    {
                        drAdd["Inf_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["BasePrice"].Value.ToString().Replace("INR", "").Trim()));
                    }
                    else if (AttributeAirPricingInfo["ApproximateBasePrice"] != null)
                    {
                        drAdd["Inf_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["ApproximateBasePrice"].Value.ToString().Replace("INR", "").Trim()));
                    }

                    int TotalPrice = Decimal.ToInt32(Convert.ToDecimal(AttributeAirPricingInfo["TotalPrice"].Value.ToString().Replace("INR", "").Trim()));

                    drAdd["InfTotalTax"] = TotalPrice - Convert.ToInt32(drAdd["Inf_BASIC"].ToString());
                    drAdd["Inf_Tax"] = TotalPrice - Convert.ToInt32(drAdd["Inf_BASIC"].ToString());
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceFilter_GDS-SetFare_INF", "air_uapi", "", SearchID, ex.Message);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------

        private string GetLowestFareFromPriceResponse(string SearchID, string CompanyID, ArrayList ArayAirPriceResult)
        {
            if (ArayAirPriceResult.Count.Equals(1))
            {
                return ArayAirPriceResult[0].ToString();
            }
            else
            {
                Hashtable htPriceResultID = new Hashtable();
                ArrayList ArayPriceResultID = new ArrayList();
                for (int i = 0; i < ArayAirPriceResult.Count; i++)
                {
                    XmlDocument PriceDocument = new XmlDocument();
                    PriceDocument.LoadXml(ArayAirPriceResult[i].ToString());
                    XmlElement PriceRoot = PriceDocument.DocumentElement;
                    XmlAttributeCollection AttributeAirPricingInfo = PriceRoot.Attributes;

                    foreach (XmlAttribute attrib in AttributeAirPricingInfo)
                    {
                        string AttName = attrib.OuterXml;
                        if (attrib.OuterXml.IndexOf("TotalPrice") != -1 && attrib.OuterXml.IndexOf("ApproximateTotalPrice") == -1)
                        {
                            htPriceResultID.Add(i, Convert.ToDouble(attrib.Value.Replace("INR", "").Trim()));
                            ArayPriceResultID.Add(Convert.ToDouble(attrib.Value.Replace("INR", "").Trim()));
                        }
                    }
                }

                return ArayAirPriceResult[0].ToString();
            }
        }
        private void GetSeprateDataFromPriceResponse(string SearchID, string CompanyID, string PriceResponse, out string strAirItinerary, out ArrayList ArayAirPriceResult, out ArrayList ArayFareRule)
        {
            strAirItinerary = "";
            ArayAirPriceResult = new ArrayList();
            ArayFareRule = new ArrayList();

            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(PriceResponse);
                XmlElement root = xmlflt.DocumentElement;
                if (root.HasChildNodes)
                {
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlDocument xmlflt1 = new XmlDocument();
                        xmlflt1.LoadXml(root.ChildNodes[i].InnerXml);
                        XmlElement RootElement = xmlflt1.DocumentElement;
                        if (RootElement.HasChildNodes)
                        {
                            for (int j = 0; j < RootElement.ChildNodes.Count; j++)
                            {
                                string strAirPriceResp = RootElement.ChildNodes[j].OuterXml;
                                if (strAirPriceResp.IndexOf("AirItinerary") != -1)
                                {
                                    strAirItinerary = strAirPriceResp;
                                }
                                else if (strAirPriceResp.IndexOf("AirPriceResult") != -1)
                                {
                                    XmlDocument objXmlDocument = new XmlDocument();
                                    objXmlDocument.LoadXml(strAirPriceResp);
                                    XmlElement PriceResultRoot = objXmlDocument.DocumentElement;
                                    if (objXmlDocument.HasChildNodes)
                                    {
                                        for (int k = 0; k < PriceResultRoot.ChildNodes.Count; k++)
                                        {
                                            string AirPriceResult = PriceResultRoot.ChildNodes[k].OuterXml;
                                            if (AirPriceResult.IndexOf("AirPricingSolution") != -1)
                                            {
                                                ArayAirPriceResult.Add(AirPriceResult);
                                            }
                                            if (AirPriceResult.IndexOf("FareRule") != -1 && AirPriceResult.IndexOf("FareRuleLong") != -1)
                                            {
                                                ArayFareRule.Add(AirPriceResult);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceFilter_GDS-GetSeprateDataFromPriceResponse", "air_uapi", PriceResponse, SearchID, ex.Message);
            }
        }
    }
}
