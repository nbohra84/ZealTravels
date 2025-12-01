using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for GetAvailabilityResponseQP
/// </summary>
public class GetAvailabilityResponseQP
{
    private string NetworkUserName;
    private string NetworkPassword;
    private string TargetBranch;
    private string Adt;
    private string Chd;
    private string Inf;

    private string Origin;
    private string Destination;

    public GetAvailabilityResponseQP()
    {

    }

    /*public GetAvailabilityResponseQP(string NetworkUserName, string NetworkPassword, string TargetBranch, string Adt, string Chd, string Inf, string Origin, string Destination)
    {
        this.NetworkUserName = NetworkUserName;
        this.NetworkPassword = NetworkPassword;
        this.TargetBranch = TargetBranch;
        this.Adt = Adt;
        this.Chd = Chd;
        this.Inf = Inf;
        this.Origin = Origin;
        this.Destination = Destination;
    }
    public DataTable SetAvailabilityResponse(string SearchID, string CompanyID, string Sector, bool IsRT, bool IsMC, string PSQRequest, string QpResponse, string FltType, string AdtBTR, string ChdBTR, string InfBTR)
    {
        DataTable dtBound = DBCommon.Schema.SchemaFlights;

        try
        {
            XmlDocument xDocResponse = new XmlDocument();
            xDocResponse.LoadXml(QpResponse);
            XmlNode ddd = xDocResponse.LastChild.LastChild.FirstChild;

            string FlightDetailsList = "";
            string AirSegmentList = "";
            string FareInfoList = "";
            string RouteList = "";
            string AirPricePointList = "";
            string BrandList = "";
            string HostTokenList = "";
            foreach (XmlNode childNode in ddd.ChildNodes)
            {
                string ssss = childNode.OuterXml;
                if (childNode.OuterXml.IndexOf("FlightDetailsList") != -1)
                {
                    FlightDetailsList = childNode.OuterXml;
                }
                else if (childNode.OuterXml.IndexOf("segments") != -1)  //AirSegmentList
                {
                    AirSegmentList = childNode.OuterXml;
                }
                else if (childNode.OuterXml.IndexOf("fares") != -1)    //FareInfoList
                {
                    FareInfoList = childNode.OuterXml;
                }
                else if (childNode.OuterXml.IndexOf("RouteList") != -1)
                {
                    RouteList = childNode.OuterXml;
                }
                else if (childNode.OuterXml.IndexOf("AirPricePointList") != -1)
                {
                    AirPricePointList = childNode.OuterXml;
                }
                else if (childNode.OuterXml.IndexOf("BrandList") != -1)
                {
                    BrandList = childNode.OuterXml;
                }
                else if (childNode.OuterXml.IndexOf("HostTokenList") != -1)
                {
                    HostTokenList = childNode.OuterXml;
                }
            }

            DataSet dsFlightDetailsList = new DataSet();
            DataSet dsAirSegmentList = new DataSet();
            DataSet dsFareInfoList = new DataSet();
            DataSet dsRouteList = new DataSet();
            DataSet dsBrandList = new DataSet();
            DataSet dsHostTokenList = new DataSet();
            dsFlightDetailsList.ReadXml(new System.IO.StringReader(FlightDetailsList));
            dsAirSegmentList.ReadXml(new System.IO.StringReader(AirSegmentList));
            dsFareInfoList.ReadXml(new System.IO.StringReader(FareInfoList));
            dsRouteList.ReadXml(new System.IO.StringReader(RouteList));
            if (BrandList != null && BrandList.Length > 0 && BrandList.IndexOf("BrandID") != -1)
            {
                dsBrandList.ReadXml(new System.IO.StringReader(BrandList));
            }
            dsHostTokenList.ReadXml(new System.IO.StringReader(HostTokenList));

            string Token = "";
            foreach (DataRow dr in dsHostTokenList.Tables["HostToken"].Rows)
            {
                Token += dr["Key"].ToString() + "@" + dr["HostToken_Text"].ToString() + " ? ";
            }


            string ORGG = "";
            string DESS = "";

            DataTable dtFlights = GetFlightReferences(AirPricePointList);
            if (dtFlights != null && dtFlights.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFlights.Rows)
                {
                    string SegmentRef = dr["AdtSegmentRef"].ToString();
                    string AdtHostTokenRef = dr["AdtHostTokenRef"].ToString();
                    if (SegmentRef.Length > 0)
                    {
                        string[] SplitSegmentRef = SegmentRef.Split('?');
                        string[] SplitHostSegmentRef = AdtHostTokenRef.Split('?');
                        for (int i = 0; i < SplitSegmentRef.Length; i++)
                        {
                            string Group = "0";
                            string SegementRef = SplitSegmentRef[i].ToString().Trim();
                            string HostSegmentRef = SplitHostSegmentRef[i].ToString().Trim();
                            if (SegementRef.IndexOf("outbound") != -1)
                            {
                                FltType = "O";
                                Group = "0";
                                SegementRef = SegementRef.Replace("outbound", "").Trim();
                                HostSegmentRef = HostSegmentRef.Replace("outbound", "").Trim();
                            }
                            else if (SegementRef.IndexOf("inbound") != -1)
                            {
                                FltType = "I";
                                Group = "1";
                                SegementRef = SegementRef.Replace("inbound", "").Trim();
                                HostSegmentRef = HostSegmentRef.Replace("inbound", "").Trim();
                            }


                            DataRow[] drAirSegemnt = dsAirSegmentList.Tables["AirSegment"].Select("Key='" + SegementRef + "' And Group='" + Group + "'");

                            int k22 = drAirSegemnt.Length;
                            if (k22 > 1)
                            {

                            }

                            if (drAirSegemnt.Length > 0)
                            {
                                string[] SplitClass = dr["AdtBookingCode"].ToString().Split('?');

                                DataRow drAdd = dtBound.NewRow();
                                drAdd["ClassOfService"] = SplitClass[i].ToString().Trim();

                                drAdd["AdtHTR"] = HostSegmentRef;
                                drAdd["ChdHTR"] = dr["ChdHostTokenRef"].ToString();
                                drAdd["InfHTR"] = dr["InfHostTokenRef"].ToString();
                                drAdd["HostTokenRef"] = Token;


                                drAdd["AdtBTR"] = AdtBTR;
                                drAdd["ChdBTR"] = ChdBTR;
                                drAdd["InfBTR"] = InfBTR;
                                drAdd["AirlineID"] = "QPMAA8752B"; //TargetBranch;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;

                                if (FltType.Equals("O"))
                                {
                                    drAdd["Origin"] = Origin;
                                    drAdd["Destination"] = Destination;
                                }
                                else
                                {
                                    drAdd["Origin"] = Destination;
                                    drAdd["Destination"] = Origin;
                                }

                                drAdd["Sector"] = Sector;
                                drAdd["FltType"] = FltType;

                                drAdd["Connection_O"] = dr["Connection_O"].ToString();
                                drAdd["Connection_I"] = dr["Connection_I"].ToString();

                                drAdd["BookingCode"] = dr["AdtBookingCode"].ToString();
                                drAdd["SeatsAvailable"] = dr["BookingCount"].ToString();
                                drAdd["CabinClass"] = dr["CabinClass"].ToString();
                                //LatestTicketingTime:2021-12-27T23:59:00.000+05:30?PricingMethod:Guaranteed?Refundable:true?ETicketability:Yes?PlatingCarrier:AI?ProviderCode:1G?Cat35Indicator:false?

                                string Extra = dr["Extra"].ToString();
                                string[] SplitExtra = dr["Extra"].ToString().Split('?');
                                if (SplitExtra.Length > 6)
                                {
                                    drAdd["LatestTicketingTime"] = SplitExtra[0].ToString().Replace("LatestTicketingTime:", "").Trim();
                                    drAdd["PricingMethod"] = SplitExtra[1].ToString().Replace("PricingMethod:", "").Trim();
                                    drAdd["Refundable"] = SplitExtra[2].ToString().Replace("Refundable:", "").Trim();
                                    drAdd["ETicketability"] = SplitExtra[3].ToString().Replace("ETicketability:", "").Trim();
                                    drAdd["PlatingCarrier"] = SplitExtra[4].ToString().Replace("PlatingCarrier:", "").Trim();
                                    drAdd["ProviderCode"] = SplitExtra[5].ToString().Replace("ProviderCode:", "").Trim();
                                    drAdd["Cat35Indicator"] = SplitExtra[6].ToString().Replace("Cat35Indicator:", "").Trim();
                                }

                                drAdd["AirPricePointKey"] = dr["AirPricePointKey"].ToString();


                                drAdd["AdtAirPricingInfoKey"] = dr["AdtAirPricingInfoKey"].ToString();
                                drAdd["ChdAirPricingInfoKey"] = dr["ChdAirPricingInfoKey"].ToString();
                                drAdd["InfAirPricingInfoKey"] = dr["InfAirPricingInfoKey"].ToString();

                                drAdd["AdtFareInfoRef"] = dr["AdtFareInfoRef"].ToString();
                                drAdd["ChdFareInfoRef"] = dr["ChdFareInfoRef"].ToString();
                                drAdd["InfFareInfoRef"] = dr["InfFareInfoRef"].ToString();

                                drAdd["AdtOptionKey"] = dr["AdtOptionKey"].ToString();
                                drAdd["ChdOptionKey"] = dr["ChdOptionKey"].ToString();
                                drAdd["InfOptionKey"] = dr["InfOptionKey"].ToString();

                                drAdd["AdtTaxes"] = dr["AdtTaxes"].ToString();
                                drAdd["ChdTaxes"] = dr["ChdTaxes"].ToString();
                                drAdd["InfTaxes"] = dr["InfTaxes"].ToString();


                                drAdd["Refid"] = 5000 + Convert.ToInt32(dr["refid"].ToString());    //== 4000 for Uaoi

                                drAdd["CancellationFee"] = dr["CancellationFee"].ToString();
                                drAdd["DateChangeFee"] = dr["DateChangeFee"].ToString();

                                //--------------------------------------------------------------------------------------------------------------------
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("NumberOfStops") && drAirSegemnt.CopyToDataTable().Rows[0]["NumberOfStops"].ToString().Length > 0)
                                {
                                    dr["Via"] = Convert.ToInt32(drAirSegemnt.CopyToDataTable().Rows[0]["NumberOfStops"].ToString().Trim());
                                }
                                drAdd["SegmentRef"] = drAirSegemnt.CopyToDataTable().Rows[0]["Key"].ToString();
                                drAdd["Group"] = drAirSegemnt.CopyToDataTable().Rows[0]["Group"].ToString();
                                drAdd["CarrierCode"] = drAirSegemnt.CopyToDataTable().Rows[0]["Carrier"].ToString();
                                drAdd["FlightNumber"] = drAirSegemnt.CopyToDataTable().Rows[0]["FlightNumber"].ToString();
                                drAdd["DepartureStation"] = drAirSegemnt.CopyToDataTable().Rows[0]["Origin"].ToString();
                                drAdd["ArrivalStation"] = drAirSegemnt.CopyToDataTable().Rows[0]["Destination"].ToString();

                                drAdd["DepDate"] = drAirSegemnt.CopyToDataTable().Rows[0]["DepartureTime"].ToString();
                                drAdd["ArrDate"] = drAirSegemnt.CopyToDataTable().Rows[0]["ArrivalTime"].ToString();
                                drAdd["DepTime"] = drAirSegemnt.CopyToDataTable().Rows[0]["DepartureTime"].ToString();
                                drAdd["ArrTime"] = drAirSegemnt.CopyToDataTable().Rows[0]["ArrivalTime"].ToString();

                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("FlightTime"))
                                {
                                    drAdd["FlightTime"] = drAirSegemnt.CopyToDataTable().Rows[0]["FlightTime"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("Distance"))
                                {
                                    drAdd["Distance"] = drAirSegemnt.CopyToDataTable().Rows[0]["Distance"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("ETicketability"))
                                {
                                    drAdd["ETicketability"] = drAirSegemnt.CopyToDataTable().Rows[0]["ETicketability"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("Equipment"))
                                {
                                    drAdd["EquipmentType"] = drAirSegemnt.CopyToDataTable().Rows[0]["Equipment"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("ChangeOfPlane"))
                                {
                                    drAdd["ChangeOfPlane"] = drAirSegemnt.CopyToDataTable().Rows[0]["ChangeOfPlane"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("ParticipantLevel"))
                                {
                                    drAdd["ParticipantLevel"] = drAirSegemnt.CopyToDataTable().Rows[0]["ParticipantLevel"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("LinkAvailability"))
                                {
                                    drAdd["LinkAvailability"] = drAirSegemnt.CopyToDataTable().Rows[0]["LinkAvailability"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("PolledAvailabilityOption"))
                                {
                                    drAdd["PolledAvailabilityOption"] = drAirSegemnt.CopyToDataTable().Rows[0]["PolledAvailabilityOption"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("OptionalServicesIndicator"))
                                {
                                    drAdd["OptionalServicesIndicator"] = drAirSegemnt.CopyToDataTable().Rows[0]["OptionalServicesIndicator"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("AvailabilitySource"))
                                {
                                    drAdd["AvailabilitySource"] = drAirSegemnt.CopyToDataTable().Rows[0]["AvailabilitySource"].ToString();
                                }
                                if (drAirSegemnt.CopyToDataTable().Columns.Contains("AvailabilityDisplayType"))
                                {
                                    drAdd["AvailabilityDisplayType"] = drAirSegemnt.CopyToDataTable().Rows[0]["AvailabilityDisplayType"].ToString();
                                }
                                //drAdd["FlightTime"] = drAirSegemnt.CopyToDataTable().Rows[0]["FlightTime"].ToString();
                                //drAdd["Distance"] = drAirSegemnt.CopyToDataTable().Rows[0]["Distance"].ToString();
                                //drAdd["ETicketability"] = drAirSegemnt.CopyToDataTable().Rows[0]["ETicketability"].ToString();
                                //drAdd["EquipmentType"] = drAirSegemnt.CopyToDataTable().Rows[0]["Equipment"].ToString();
                                //drAdd["ChangeOfPlane"] = drAirSegemnt.CopyToDataTable().Rows[0]["ChangeOfPlane"].ToString();
                                //drAdd["ParticipantLevel"] = drAirSegemnt.CopyToDataTable().Rows[0]["ParticipantLevel"].ToString();
                                //drAdd["LinkAvailability"] = drAirSegemnt.CopyToDataTable().Rows[0]["LinkAvailability"].ToString();
                                //drAdd["PolledAvailabilityOption"] = drAirSegemnt.CopyToDataTable().Rows[0]["PolledAvailabilityOption"].ToString();
                                //drAdd["OptionalServicesIndicator"] = drAirSegemnt.CopyToDataTable().Rows[0]["OptionalServicesIndicator"].ToString();
                                //drAdd["AvailabilitySource"] = drAirSegemnt.CopyToDataTable().Rows[0]["AvailabilitySource"].ToString();
                                //drAdd["AvailabilityDisplayType"] = drAirSegemnt.CopyToDataTable().Rows[0]["AvailabilityDisplayType"].ToString();

                                DataRow[] drAirAvailInfo = dsAirSegmentList.Tables["AirAvailInfo"].Select("AirSegment_Id='" + drAirSegemnt.CopyToDataTable().Rows[0]["AirSegment_Id"].ToString() + "'");
                                if (drAirAvailInfo.Length > 0)
                                {
                                    drAdd["ProviderCode"] = drAirAvailInfo.CopyToDataTable().Rows[0]["ProviderCode"].ToString();
                                }

                                DataRow[] drFlightDetailsRef = dsAirSegmentList.Tables["FlightDetailsRef"].Select("AirSegment_Id='" + drAirSegemnt.CopyToDataTable().Rows[0]["AirSegment_Id"].ToString() + "'");
                                if (drFlightDetailsRef.Length > 0)
                                {
                                    drAdd["FlightDetailsRefKey"] = drFlightDetailsRef.CopyToDataTable().Rows[0]["Key"].ToString();

                                    DataRow[] drFlightDetails = dsFlightDetailsList.Tables["FlightDetails"].Select("Key='" + drFlightDetailsRef.CopyToDataTable().Rows[0]["Key"].ToString() + "'");
                                    if (drFlightDetails.Length > 0)
                                    {
                                        if (drFlightDetails.CopyToDataTable().Columns.Contains("TravelTime"))
                                        {
                                            drAdd["TravelTime"] = drFlightDetails.CopyToDataTable().Rows[0]["TravelTime"].ToString();
                                        }
                                        if (drFlightDetails.CopyToDataTable().Columns.Contains("OriginTerminal"))
                                        {
                                            drAdd["DepartureTerminal"] = drFlightDetails.CopyToDataTable().Rows[0]["OriginTerminal"].ToString();
                                        }
                                        if (drFlightDetails.CopyToDataTable().Columns.Contains("DestinationTerminal"))
                                        {
                                            drAdd["ArrivalTerminal"] = drFlightDetails.CopyToDataTable().Rows[0]["DestinationTerminal"].ToString();
                                        }
                                    }
                                }

                                if (dsAirSegmentList.Tables["CodeshareInfo"] != null)
                                {
                                    DataRow[] drCodeshareInfo = dsAirSegmentList.Tables["CodeshareInfo"].Select("AirSegment_Id='" + drAirSegemnt.CopyToDataTable().Rows[0]["AirSegment_Id"].ToString() + "'");
                                    if (drCodeshareInfo.Length > 0)
                                    {
                                        if (drCodeshareInfo.CopyToDataTable().Columns.Contains("OperatingCarrier"))
                                        {
                                            drAdd["CodeshareInfoOperatingCarrier"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString();
                                        }
                                        if (drCodeshareInfo.CopyToDataTable().Columns.Contains("OperatingFlightNumber"))
                                        {
                                            drAdd["CodeshareInfoOperatingFlightNumber"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingFlightNumber"].ToString();
                                        }
                                        if (drCodeshareInfo.CopyToDataTable().Columns.Contains("CodeshareInfo_Text"))
                                        {
                                            drAdd["CodeshareInfo"] = drCodeshareInfo.CopyToDataTable().Rows[0]["CodeshareInfo_Text"].ToString();
                                        }
                                    }
                                }

                                string[] SplitFareInfoRef = dr["AdtFareInfoRef"].ToString().Split('?');
                                for (int j = 0; j < SplitFareInfoRef.Length;)
                                {
                                    DataRow[] drFareRuleKey = dsFareInfoList.Tables["FareRuleKey"].Select("FareInfoRef='" + SplitFareInfoRef[j].ToString().Trim() + "'");
                                    if (drFareRuleKey.Length > 0)
                                    {
                                        drAdd["FareRuleInfo_Text"] = drFareRuleKey.CopyToDataTable().Rows[0]["FareRuleKey_Text"].ToString().Trim();
                                    }

                                    DataRow[] drFare = dsFareInfoList.Tables["FareInfo"].Select("Key='" + SplitFareInfoRef[j].ToString().Trim() + "'");
                                    if (drFare.Length > 0)
                                    {
                                        string PriceType = "";
                                        if (dsBrandList != null && dsBrandList.Tables.Count > 0)
                                        {
                                            PriceType = GetFareType(dsBrandList, dsFareInfoList.Tables["Brand"], drFare.CopyToDataTable().Rows[0]["FareInfo_Id"].ToString());
                                        }
                                        if (PriceType.Length.Equals(0) && drFare.CopyToDataTable().Columns.Contains("FareFamily"))
                                        {
                                            drAdd["PriceType"] = "." + drFare.CopyToDataTable().Rows[0]["FareFamily"].ToString();
                                        }
                                        else
                                        {
                                            drAdd["PriceType"] = "." + PriceType;
                                        }

                                        if (drAdd["FareBasisCode"].ToString().Trim().Length.Equals(0))
                                        {
                                            drAdd["FareBasisCode"] = drFare.CopyToDataTable().Rows[0]["FareBasis"].ToString();
                                        }

                                        if (drAdd["BaggageDetail"].ToString().Trim().Length.Equals(0))
                                        {
                                            string CabinBaggage = "7 KG";
                                            string Baggage = "15 KG";

                                            if (dsFareInfoList.Tables["BaggageAllowance"] != null)
                                            {
                                                DataRow[] drSelectBaggageInfo = dsFareInfoList.Tables["BaggageAllowance"].Select("FareInfo_Id='" + drFare.CopyToDataTable().Rows[0]["FareInfo_Id"].ToString() + "'");
                                                if (drSelectBaggageInfo.Length > 0)
                                                {
                                                    if (drSelectBaggageInfo.CopyToDataTable().Columns.Contains("NumberOfPieces") && drSelectBaggageInfo.CopyToDataTable().Rows[0]["NumberOfPieces"].ToString().Trim().Length > 0)
                                                    {
                                                        Baggage = drSelectBaggageInfo.CopyToDataTable().Rows[0]["NumberOfPieces"].ToString().Trim() + " " + "PCS";
                                                    }
                                                    else
                                                    {
                                                        DataRow[] drSelectBaggageWeightInfo = dsFareInfoList.Tables["MaxWeight"].Select("BaggageAllowance_Id='" + drSelectBaggageInfo.CopyToDataTable().Rows[0]["BaggageAllowance_Id"].ToString() + "'");
                                                        if (drSelectBaggageWeightInfo.Length > 0)
                                                        {
                                                            Baggage = drSelectBaggageWeightInfo.CopyToDataTable().Rows[0]["Value"].ToString().Trim() + " " + drSelectBaggageWeightInfo.CopyToDataTable().Rows[0]["Unit"].ToString().Trim();
                                                        }
                                                    }
                                                    drAdd["BaggageDetail"] = Baggage + "*" + CabinBaggage;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }

                                //DataRow[] drFareInfo = dsAirSegmentList.Tables["CodeshareInfo"].Select("AirSegment_Id='" + drAirSegemnt.CopyToDataTable().Rows[0]["AirSegment_Id"].ToString() + "'");
                                //if (drCodeshareInfo.Length > 0)
                                //{
                                //    drAdd["CodeshareInfoOperatingCarrier"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString();
                                //    drAdd["CodeshareInfoOperatingFlightNumber"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingFlightNumber"].ToString();
                                //    drAdd["CodeshareInfo"] = drCodeshareInfo.CopyToDataTable().Rows[0]["CodeshareInfo_Text"].ToString();
                                //}

                                //IN:INR101?K3:INR426?P2:INR236?YR:INR590?

                                if (dr["AdtTaxes"].ToString().IndexOf("TotalPrice") != -1)
                                {
                                    string[] SplitTaxes = dr["AdtTaxes"].ToString().Split('?');
                                    if (SplitTaxes.Length > 3)
                                    {
                                        int K3 = 0;
                                        int YR = 0;
                                        int YQ = 0;
                                        int WO = 0;
                                        int JN = 0;
                                        int IN = 0;
                                        int OT = 0;

                                        for (int k = 0; k < SplitTaxes.Length; k++)
                                        {
                                            if (SplitTaxes[k].ToString().IndexOf("BasePrice") != -1)
                                            {
                                                drAdd["AdtTotalBasic"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("BasePrice:", "").Trim().Replace("INR", "").Trim()));
                                                drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("BasePrice:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                            if (SplitTaxes[k].ToString().IndexOf("Taxes") != -1)
                                            {
                                                drAdd["AdtTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("Taxes:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                            if (SplitTaxes[k].ToString().IndexOf("TotalPrice") != -1)
                                            {
                                                drAdd["AdtTotalFare"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("TotalPrice:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                        }
                                        for (int k = 0; k < SplitTaxes.Length; k++)
                                        {
                                            if (SplitTaxes[k].ToString().Trim().Length > 1)
                                            {
                                                if (SplitTaxes[k].ToString().IndexOf("BasePrice") == -1 && SplitTaxes[k].ToString().IndexOf("Taxes") == -1 && SplitTaxes[k].ToString().IndexOf("TotalPrice") == -1)
                                                {
                                                    if (SplitTaxes[k].ToString().IndexOf("K3") != -1)
                                                    {
                                                        K3 += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("K3:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("YR") != -1)
                                                    {
                                                        YR += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("YR:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("YQ") != -1)
                                                    {
                                                        YQ += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("YQ:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("WO") != -1)
                                                    {
                                                        WO += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("WO:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("JN") != -1)
                                                    {
                                                        JN += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("JN:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("IN") != -1 && SplitTaxes[k].ToString().IndexOf("INR") == -1)
                                                    {
                                                        IN += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("IN:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("YM") != -1)
                                                    {
                                                        IN += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("YM:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else
                                                    {
                                                        string[] split = SplitTaxes[k].ToString().Split(':');
                                                        OT += Decimal.ToInt32(Convert.ToDecimal(split[1].ToString().Replace("INR", "").Trim()));
                                                    }
                                                }
                                            }
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
                                if (dr["ChdTaxes"].ToString().IndexOf("TotalPrice") != -1)
                                {
                                    string[] SplitTaxes = dr["ChdTaxes"].ToString().Split('?');
                                    if (SplitTaxes.Length > 3)
                                    {
                                        int K3 = 0;
                                        int YR = 0;
                                        int YQ = 0;
                                        int WO = 0;
                                        int JN = 0;
                                        int IN = 0;
                                        int OT = 0;

                                        for (int k = 0; k < SplitTaxes.Length; k++)
                                        {
                                            if (SplitTaxes[k].ToString().IndexOf("BasePrice") != -1)
                                            {
                                                drAdd["ChdTotalBasic"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("BasePrice:", "").Trim().Replace("INR", "").Trim()));
                                                drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("BasePrice:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                            if (SplitTaxes[k].ToString().IndexOf("Taxes") != -1)
                                            {
                                                drAdd["ChdTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("Taxes:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                            if (SplitTaxes[k].ToString().IndexOf("TotalPrice") != -1)
                                            {
                                                drAdd["ChdTotalFare"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("TotalPrice:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                        }
                                        for (int k = 0; k < SplitTaxes.Length; k++)
                                        {
                                            if (SplitTaxes[k].ToString().Trim().Length > 1)
                                            {
                                                if (SplitTaxes[k].ToString().IndexOf("BasePrice") == -1 && SplitTaxes[k].ToString().IndexOf("Taxes") == -1 && SplitTaxes[k].ToString().IndexOf("TotalPrice") == -1)
                                                {
                                                    if (SplitTaxes[k].ToString().IndexOf("K3") != -1)
                                                    {
                                                        K3 += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("K3:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("YR") != -1)
                                                    {
                                                        YR += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("YR:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("YQ") != -1)
                                                    {
                                                        YQ += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("YQ:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("WO") != -1)
                                                    {
                                                        WO += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("WO:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("JN") != -1)
                                                    {
                                                        JN += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("JN:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("IN") != -1 && SplitTaxes[k].ToString().IndexOf("INR") == -1)
                                                    {
                                                        IN += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("IN:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else if (SplitTaxes[k].ToString().IndexOf("YM") != -1)
                                                    {
                                                        IN += Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("YM:", "").Trim().Replace("INR", "").Trim()));
                                                    }
                                                    else
                                                    {
                                                        string[] split = SplitTaxes[k].ToString().Split(':');
                                                        OT += Decimal.ToInt32(Convert.ToDecimal(split[1].ToString().Replace("INR", "").Trim()));
                                                    }
                                                }
                                            }
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
                                if (dr["InfTaxes"].ToString().IndexOf("TotalPrice") != -1)
                                {
                                    string[] SplitTaxes = dr["InfTaxes"].ToString().Split('?');
                                    if (SplitTaxes.Length > 3)
                                    {
                                        for (int k = 0; k < SplitTaxes.Length; k++)
                                        {
                                            if (SplitTaxes[k].ToString().IndexOf("BasePrice") != -1)
                                            {
                                                drAdd["InfTotalBasic"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("BasePrice:", "").Trim().Replace("INR", "").Trim()));
                                                drAdd["Inf_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("BasePrice:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                            if (SplitTaxes[k].ToString().IndexOf("Taxes") != -1)
                                            {
                                                drAdd["InfTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("Taxes:", "").Trim().Replace("INR", "").Trim()));
                                                drAdd["Inf_Tax"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("Taxes:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                            if (SplitTaxes[k].ToString().IndexOf("TotalPrice") != -1)
                                            {
                                                drAdd["InfTotalFare"] = Decimal.ToInt32(Convert.ToDecimal(SplitTaxes[k].ToString().Replace("TotalPrice:", "").Trim().Replace("INR", "").Trim()));
                                            }
                                        }
                                    }
                                }

                                if (Convert.ToInt32(drAdd["InfTotalFare"].ToString()) > 0 && (Convert.ToInt32(drAdd["Inf_BASIC"].ToString()) + Convert.ToInt32(drAdd["Inf_Tax"].ToString())).Equals(0))
                                {
                                    drAdd["Inf_Tax"] = Convert.ToInt32(drAdd["InfTotalFare"].ToString());
                                    drAdd["InfTotalTax"] = Convert.ToInt32(drAdd["InfTotalFare"].ToString());
                                }

                                if (drAdd["PriceType"].ToString().ToUpper().IndexOf("Corp".ToUpper()) != -1 || drAdd["PriceType"].ToString().ToUpper().IndexOf("Flexible".ToUpper()) != -1)
                                {
                                    drAdd["RefundType"] = "N";
                                }
                                else
                                {
                                    drAdd["RefundType"] = "Y";
                                }

                                dtBound.Rows.Add(drAdd);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DBCommon.Logger.dbLogg(CompanyID, 0, "GetAvailabilityResponseQP" + "air_uapi", PSQRequest, QpResponse, SearchID, ex.Message + "," + ex.StackTrace);
        }
        return dtBound;
    }
    private DataTable GetFlightReferences(string AirPricePointList)
    {
        DataTable dtFlights = Schemas.SchemaApiFlights;

        XmlDocument xDocAirPricePointList = new XmlDocument();
        xDocAirPricePointList.LoadXml(AirPricePointList);
        XmlNodeList XmlNodeListAirPricePointList = xDocAirPricePointList.LastChild.ChildNodes;


        DataTable dtFlightReference = Schemas.SchemaApiFlightsReference;
        DataTable dtBound = DBCommon.Schema.SchemaFlights;

        int fltid = 0;
        int refid = 0;
        foreach (XmlNode AllFares in XmlNodeListAirPricePointList)
        {
            XmlDocument xDocFares = new XmlDocument();
            xDocFares.LoadXml(AllFares.OuterXml);
            string AirPricePointKey = AllFares.Attributes["Key"].Value;
            foreach (XmlNode Fares in xDocFares.ChildNodes)
            {
                foreach (XmlNode PaxFares in Fares.ChildNodes)
                {
                    fltid = 1;
                    string AirPriceInfoKey = PaxFares.Attributes["Key"].Value;
                    DataSet dsPaxFare = new DataSet();
                    dsPaxFare.ReadXml(new System.IO.StringReader(PaxFares.OuterXml));

                    if (dsPaxFare.Tables["FlightOption"] != null)
                    {
                        ArrayList ArCommon = DBCommon.CommonFunction.DataTable2ArrayList(dsPaxFare.Tables["FlightOption"], "FlightOption_Id", true);
                        //for (int d = 0; d < ArCommon.Count; d++)
                        //{
                        DataRow[] drFlt1 = dsPaxFare.Tables["Option"].Select("FlightOption_Id='" + ArCommon[0].ToString() + "'");
                        DataRow[] drFlt2 = dsPaxFare.Tables["Option"].Select("FlightOption_Id='" + ArCommon[1].ToString() + "'");
                        //}

                        if (drFlt1.Length > 0 && drFlt2.Length > 0)
                        {
                            foreach (DataRow drFlightOption1 in drFlt1.CopyToDataTable().Rows)
                            {
                                string OptionId1 = drFlightOption1["Option_Id"].ToString();
                                foreach (DataRow drFlightOption2 in drFlt2.CopyToDataTable().Rows)
                                {
                                    string OptionId2 = drFlightOption2["Option_Id"].ToString();
                                    fltid++;
                                    DataRow[] drOption = dsPaxFare.Tables["Option"].Select("Option_Id='" + OptionId1 + "' OR Option_Id='" + OptionId2 + "'  ");
                                    foreach (DataRow dr in drOption.CopyToDataTable().Rows)
                                    {
                                        DataRow[] drSelect = dsPaxFare.Tables["BookingInfo"].Select("Option_Id='" + dr["Option_Id"].ToString() + "'");
                                        if (drSelect.Length > 0)
                                        {
                                            foreach (DataRow drr in drSelect.CopyToDataTable().Rows)
                                            {
                                                DataRow drAdd = dtFlightReference.NewRow();

                                                if (dsPaxFare.Tables["Connection"] != null)
                                                {
                                                    DataRow[] drSegmentIndex1 = dsPaxFare.Tables["Connection"].Select("Option_Id='" + OptionId1 + "'");
                                                    if (drSegmentIndex1.Length > 0)
                                                    {
                                                        string segment = "";
                                                        foreach (DataRow drSeg in drSegmentIndex1.CopyToDataTable().Rows)
                                                        {
                                                            segment += drSeg["SegmentIndex"].ToString() + "?";
                                                        }
                                                        segment = segment.Substring(0, segment.Length - 1);
                                                        drAdd["Connection_O"] = segment;
                                                    }
                                                }

                                                if (dsPaxFare.Tables["Connection"] != null)
                                                {
                                                    DataRow[] drSegmentIndex2 = dsPaxFare.Tables["Connection"].Select("Option_Id='" + OptionId2 + "'");
                                                    if (drSegmentIndex2.Length > 0)
                                                    {
                                                        string segment = "";
                                                        foreach (DataRow drSeg in drSegmentIndex2.CopyToDataTable().Rows)
                                                        {
                                                            segment += drSeg["SegmentIndex"].ToString() + "?";
                                                        }
                                                        segment = segment.Substring(0, segment.Length - 1);
                                                        drAdd["Connection_I"] = segment;
                                                    }
                                                }


                                                drAdd["fltid"] = fltid;
                                                drAdd["AirPricePointKey"] = AirPricePointKey;

                                                drAdd["FltType"] = dsPaxFare.Tables["Option"].Select("Option_Id='" + drr["Option_Id"].ToString() + "'").CopyToDataTable().Rows[0]["FlightOption_Id"].ToString();

                                                if (dsPaxFare.Tables["PassengerType"] != null && dsPaxFare.Tables["PassengerType"].Rows[0]["Code"].ToString().Equals("ADT"))
                                                {
                                                    drAdd["AirPricingInfoKey"] = "AA-" + AirPriceInfoKey;

                                                    string Taxes = "";
                                                    Taxes += "TotalPrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString() + "?";
                                                    Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString() + "?";

                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("Taxes"))
                                                    {
                                                        Taxes += "Taxes:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Taxes"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Taxes += "Taxes:" + "INR0" + " ? ";
                                                    }

                                                    if (dsPaxFare.Tables["TaxInfo"] != null)
                                                    {
                                                        foreach (DataRow drTaxes in dsPaxFare.Tables["TaxInfo"].Rows)
                                                        {
                                                            Taxes += drTaxes["Category"].ToString() + ":" + drTaxes["Amount"].ToString() + "?";
                                                        }
                                                    }
                                                    drAdd["AdtTaxes"] = Taxes;

                                                    string Extra = "";


                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("LatestTicketingTime"))
                                                    {
                                                        Extra += "LatestTicketingTime:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["LatestTicketingTime"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Extra += "LatestTicketingTime:" + "" + "?";
                                                    }


                                                    Extra += "PricingMethod:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["PricingMethod"].ToString() + "?";

                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("Refundable"))
                                                    {
                                                        Extra += "Refundable:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Refundable"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Extra += "Refundable:" + "" + "?";
                                                    }

                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("ETicketability"))
                                                    {
                                                        Extra += "ETicketability:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ETicketability"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Extra += "ETicketability:" + "" + "?";
                                                    }


                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("PlatingCarrier"))
                                                    {
                                                        Extra += "PlatingCarrier:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["PlatingCarrier"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Extra += "PlatingCarrier:" + "" + "?";
                                                    }



                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("ProviderCode"))
                                                    {
                                                        Extra += "ProviderCode:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ProviderCode"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Extra += "ProviderCode:" + "" + "?";
                                                    }



                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("Cat35Indicator"))
                                                    {
                                                        Extra += "Cat35Indicator:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Cat35Indicator"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Extra += "Cat35Indicator:" + "" + "?";
                                                    }

                                                    //Extra += "LatestTicketingTime:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["LatestTicketingTime"].ToString() + "?";
                                                    //Extra += "PricingMethod:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["PricingMethod"].ToString() + "?";

                                                    //if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("Refundable"))
                                                    //{
                                                    //    Extra += "Refundable:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Refundable"].ToString() + "?";
                                                    //}
                                                    //else
                                                    //{
                                                    //    Extra += "Refundable:" + "false" + "?";
                                                    //}
                                                    //Extra += "ETicketability:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ETicketability"].ToString() + "?";
                                                    //Extra += "PlatingCarrier:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["PlatingCarrier"].ToString() + "?";
                                                    //Extra += "ProviderCode:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ProviderCode"].ToString() + "?";
                                                    //Extra += "Cat35Indicator:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Cat35Indicator"].ToString() + "?";

                                                    drAdd["Extra"] = Extra;


                                                    if (dsPaxFare.Tables["ChangePenalty"] != null)
                                                    {
                                                        string DateChangeFee = "";
                                                        DataRow[] drChangePenalty = dsPaxFare.Tables["ChangePenalty"].Select("AirPricingInfo_Id='" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["AirPricingInfo_Id"].ToString() + "'");
                                                        if (drChangePenalty.Length > 0)
                                                        {
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
                                                            drAdd["DateChangeFee"] = DateChangeFee;
                                                        }
                                                    }

                                                    if (dsPaxFare.Tables["CancelPenalty"] != null)
                                                    {
                                                        string DateChangeFee = "";
                                                        DataRow[] drChangePenalty = dsPaxFare.Tables["CancelPenalty"].Select("AirPricingInfo_Id='" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["AirPricingInfo_Id"].ToString() + "'");
                                                        if (drChangePenalty.Length > 0)
                                                        {
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
                                                            drAdd["CancellationFee"] = DateChangeFee;
                                                        }
                                                    }
                                                }
                                                else if (dsPaxFare.Tables["PassengerType"] != null && (dsPaxFare.Tables["PassengerType"].Rows[0]["Code"].ToString().Equals("CHD") || dsPaxFare.Tables["PassengerType"].Rows[0]["Code"].ToString().Equals("CNN")))
                                                {
                                                    drAdd["AirPricingInfoKey"] = "CC-" + AirPriceInfoKey;

                                                    string Taxes = "";
                                                    Taxes += "TotalPrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString() + "?";
                                                    Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString() + "?";

                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("Taxes"))
                                                    {
                                                        Taxes += "Taxes:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Taxes"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Taxes += "Taxes:" + "INR0" + " ? ";
                                                    }

                                                    if (dsPaxFare.Tables["TaxInfo"] != null)
                                                    {
                                                        foreach (DataRow drTaxes in dsPaxFare.Tables["TaxInfo"].Rows)
                                                        {
                                                            Taxes += drTaxes["Category"].ToString() + ":" + drTaxes["Amount"].ToString() + "?";
                                                        }
                                                    }
                                                    drAdd["ChdTaxes"] = Taxes;
                                                }
                                                else if (dsPaxFare.Tables["PassengerType"] != null && dsPaxFare.Tables["PassengerType"].Rows[0]["Code"].ToString().Equals("INF"))
                                                {
                                                    drAdd["AirPricingInfoKey"] = "II-" + AirPriceInfoKey;

                                                    string Taxes = "";
                                                    Taxes += "TotalPrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString() + "?";
                                                    Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString() + "?";

                                                    if (dsPaxFare.Tables["AirPricingInfo"].Columns.Contains("Taxes"))
                                                    {
                                                        Taxes += "Taxes:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Taxes"].ToString() + "?";
                                                    }
                                                    else
                                                    {
                                                        Taxes += "Taxes:" + "INR0" + " ? ";
                                                    }

                                                    if (dsPaxFare.Tables["TaxInfo"] != null)
                                                    {
                                                        foreach (DataRow drTaxes in dsPaxFare.Tables["TaxInfo"].Rows)
                                                        {
                                                            Taxes += drTaxes["Category"].ToString() + ":" + drTaxes["Amount"].ToString() + "?";
                                                        }
                                                    }
                                                    drAdd["InfTaxes"] = Taxes;
                                                }

                                                drAdd["SegmentRef"] = drr["SegmentRef"].ToString();
                                                drAdd["HostTokenRef"] = drr["HostTokenRef"].ToString();
                                                drAdd["FareInfoRef"] = drr["FareInfoRef"].ToString();
                                                drAdd["OptionKey"] = dr["Key"].ToString();


                                                if (drAdd["BookingCount"].ToString().Length.Equals(0))
                                                {
                                                    drAdd["BookingCount"] = drr["BookingCount"].ToString();
                                                }

                                                if (drAdd["BookingCode"].ToString().Length > (0))
                                                {
                                                    drAdd["BookingCode"] = drAdd["BookingCode"].ToString() + "?" + drr["BookingCode"].ToString();
                                                }
                                                else
                                                {
                                                    drAdd["BookingCode"] = drr["BookingCode"].ToString();
                                                }

                                                if (drAdd["CabinClass"].ToString().Length.Equals(0))
                                                {
                                                    drAdd["CabinClass"] = drr["CabinClass"].ToString();
                                                }

                                                dtFlightReference.Rows.Add(drAdd);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (dtFlightReference != null && dtFlightReference.Rows.Count > 0)
                {
                    ArrayList ArayFlts = DBCommon.CommonFunction.DataTable2ArrayList(dtFlightReference, "fltid", true);
                    if (ArayFlts.Count > 0)
                    {
                        for (int i = 0; i < ArayFlts.Count; i++)
                        {
                            refid++;
                            string ConnO = string.Empty;
                            string ConnI = string.Empty;

                            string strfltid = "";
                            string strAirPricePointKey = string.Empty;
                            string strAdtAirPricingInfoKey = string.Empty;
                            string strChdAirPricingInfoKey = string.Empty;
                            string strInfAirPricingInfoKey = string.Empty;


                            string strAdtHTR = string.Empty;
                            string strChdHTR = string.Empty;
                            string strInfHTR = string.Empty;

                            string strAdtSegmentRef = string.Empty;
                            string strChdSegmentRef = string.Empty;
                            string strInfSegmentRef = string.Empty;

                            string strAdtFareInfoRef = string.Empty;
                            string strChdFareInfoRef = string.Empty;
                            string strInfFareInfoRef = string.Empty;

                            string strAdtTaxes = string.Empty;
                            string strChdTaxes = string.Empty;
                            string strInfTaxes = string.Empty;

                            string strAdtOptionKey = string.Empty;
                            string strChdOptionKey = string.Empty;
                            string strInfOptionKey = string.Empty;
                            string strBookingCount = string.Empty;
                            string strAdtBookingCode = string.Empty;
                            string strChdBookingCode = string.Empty;
                            string strInfBookingCode = string.Empty;
                            string strCabinClass = string.Empty;
                            string Extra = "";

                            string dc = "";
                            string cf = "";

                            DataRow[] drSelect = dtFlightReference.Select("fltid='" + ArayFlts[i].ToString() + "'");
                            if (drSelect.Length > 0)
                            {
                                foreach (DataRow dr in drSelect.CopyToDataTable().Rows)
                                {
                                    strfltid = refid.ToString();
                                    strAirPricePointKey = dr["AirPricePointKey"].ToString();
                                    if (dr["AirPricingInfoKey"].ToString().IndexOf("AA-") != -1)
                                    {
                                        dc = dr["DateChangeFee"].ToString();
                                        cf = dr["CancellationFee"].ToString();

                                        ConnO = dr["Connection_O"].ToString();
                                        ConnI = dr["Connection_I"].ToString();

                                        strAdtAirPricingInfoKey = dr["AirPricingInfoKey"].ToString();

                                        if (dr["FltType"].ToString().Equals("0"))
                                        {
                                            strAdtSegmentRef = strAdtSegmentRef + dr["SegmentRef"].ToString().Trim() + "outbound" + "?";
                                        }
                                        else
                                        {
                                            strAdtSegmentRef = strAdtSegmentRef + dr["SegmentRef"].ToString().Trim() + "inbound" + "?";
                                        }
                                        if (dr["FltType"].ToString().Equals("0"))
                                        {
                                            strAdtHTR = strAdtHTR + dr["HostTokenRef"].ToString().Trim() + "outbound" + "?";
                                        }
                                        else
                                        {
                                            strAdtHTR = strAdtHTR + dr["HostTokenRef"].ToString().Trim() + "inbound" + "?";
                                        }

                                        strAdtFareInfoRef = strAdtFareInfoRef + dr["FareInfoRef"].ToString() + "?";
                                        strAdtOptionKey = dr["OptionKey"].ToString();
                                        strAdtBookingCode = strAdtBookingCode + dr["BookingCode"].ToString() + "?";
                                        strAdtTaxes = dr["AdtTaxes"].ToString();
                                        Extra = dr["Extra"].ToString();
                                    }
                                    if (dr["AirPricingInfoKey"].ToString().IndexOf("CC-") != -1)
                                    {
                                        if (dr["FltType"].ToString().Equals("0"))
                                        {
                                            strChdHTR = strChdHTR + dr["HostTokenRef"].ToString().Trim() + "outbound" + "?";
                                        }
                                        else
                                        {
                                            strChdHTR = strChdHTR + dr["HostTokenRef"].ToString().Trim() + "inbound" + "?";
                                        }

                                        strChdAirPricingInfoKey = dr["AirPricingInfoKey"].ToString();
                                        //strAdtSegmentRef = strAdtSegmentRef + dr["SegmentRef"].ToString() + "?";
                                        strChdFareInfoRef = strChdFareInfoRef + dr["FareInfoRef"].ToString() + "?";
                                        strChdOptionKey = dr["OptionKey"].ToString();
                                        //strAdtBookingCode = strAdtBookingCode + dr["BookingCode"].ToString() + "?";
                                        strChdTaxes = dr["ChdTaxes"].ToString();
                                    }
                                    if (dr["AirPricingInfoKey"].ToString().IndexOf("II-") != -1)
                                    {
                                        if (dr["FltType"].ToString().Equals("0"))
                                        {
                                            strInfHTR = strInfHTR + dr["HostTokenRef"].ToString().Trim() + "outbound" + "?";
                                        }
                                        else
                                        {
                                            strInfHTR = strInfHTR + dr["HostTokenRef"].ToString().Trim() + "inbound" + "?";
                                        }

                                        strInfAirPricingInfoKey = dr["AirPricingInfoKey"].ToString();
                                        //strAdtSegmentRef = strAdtSegmentRef + dr["SegmentRef"].ToString() + "?";
                                        strInfFareInfoRef = strInfFareInfoRef + dr["FareInfoRef"].ToString() + "?";
                                        strInfOptionKey = dr["OptionKey"].ToString();
                                        //strAdtBookingCode = strAdtBookingCode + dr["BookingCode"].ToString() + "?";
                                        strInfTaxes = dr["InfTaxes"].ToString();
                                    }
                                    if (strBookingCount.Length.Equals(0))
                                    {
                                        strBookingCount = dr["BookingCount"].ToString();
                                    }
                                    if (strCabinClass.Length.Equals(0))
                                    {
                                        strCabinClass = dr["CabinClass"].ToString();
                                    }
                                }

                                if (strAdtFareInfoRef.Length > 0)
                                {
                                    strAdtHTR = strAdtHTR.Substring(0, strAdtHTR.Length - 1);
                                    strAdtSegmentRef = strAdtSegmentRef.Substring(0, strAdtSegmentRef.Length - 1);
                                    strAdtFareInfoRef = strAdtFareInfoRef.Substring(0, strAdtFareInfoRef.Length - 1);
                                    strAdtBookingCode = strAdtBookingCode.Substring(0, strAdtBookingCode.Length - 1);
                                }
                                if (strChdFareInfoRef.Length > 0)
                                {
                                    strChdHTR = strChdHTR.Substring(0, strChdHTR.Length - 1);
                                    strChdFareInfoRef = strChdFareInfoRef.Substring(0, strChdFareInfoRef.Length - 1);
                                }
                                if (strInfFareInfoRef.Length > 0)
                                {
                                    strInfHTR = strInfHTR.Substring(0, strInfHTR.Length - 1);
                                    strInfFareInfoRef = strInfFareInfoRef.Substring(0, strInfFareInfoRef.Length - 1);
                                }

                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Connection_O"] = ConnO;
                                drAdd["Connection_I"] = ConnI;

                                drAdd["DateChangeFee"] = dc;


                                drAdd["CancellationFee"] = cf;

                                drAdd["Extra"] = Extra;
                                drAdd["refid"] = strfltid;
                                drAdd["AirPricePointKey"] = strAirPricePointKey;

                                drAdd["AdtHostTokenRef"] = strAdtHTR;
                                drAdd["ChdHostTokenRef"] = strChdHTR;
                                drAdd["InfHostTokenRef"] = strInfHTR;

                                drAdd["AdtAirPricingInfoKey"] = strAdtAirPricingInfoKey;
                                drAdd["ChdAirPricingInfoKey"] = strChdAirPricingInfoKey;
                                drAdd["InfAirPricingInfoKey"] = strInfAirPricingInfoKey;

                                drAdd["AdtSegmentRef"] = strAdtSegmentRef;
                                drAdd["ChdSegmentRef"] = strChdSegmentRef;
                                drAdd["InfSegmentRef"] = strInfSegmentRef;

                                drAdd["AdtTaxes"] = strAdtTaxes;
                                drAdd["ChdTaxes"] = strChdTaxes;
                                drAdd["InfTaxes"] = strInfTaxes;

                                drAdd["AdtFareInfoRef"] = strAdtFareInfoRef;
                                drAdd["ChdFareInfoRef"] = strChdFareInfoRef;
                                drAdd["InfFareInfoRef"] = strInfFareInfoRef;

                                drAdd["AdtOptionKey"] = strAdtOptionKey;
                                drAdd["ChdOptionKey"] = strChdOptionKey;
                                drAdd["InfOptionKey"] = strInfOptionKey;

                                drAdd["BookingCount"] = strBookingCount;

                                drAdd["AdtBookingCode"] = strAdtBookingCode;
                                drAdd["ChdBookingCode"] = strChdBookingCode;
                                drAdd["InfBookingCode"] = strInfBookingCode;

                                drAdd["CabinClass"] = strCabinClass;

                                drAdd["Extra"] = Extra;

                                dtFlights.Rows.Add(drAdd);


                                strAdtHTR = string.Empty;
                                strChdHTR = string.Empty;
                                strInfHTR = string.Empty;


                                strAdtTaxes = string.Empty;
                                strChdTaxes = string.Empty;
                                strInfTaxes = string.Empty;

                                strfltid = "";
                                strAirPricePointKey = string.Empty;
                                strAdtAirPricingInfoKey = string.Empty;
                                strChdAirPricingInfoKey = string.Empty;
                                strInfAirPricingInfoKey = string.Empty;
                                strAdtSegmentRef = string.Empty;
                                strChdSegmentRef = string.Empty;
                                strInfSegmentRef = string.Empty;
                                strAdtFareInfoRef = string.Empty;
                                strChdFareInfoRef = string.Empty;
                                strInfFareInfoRef = string.Empty;

                                strAdtOptionKey = string.Empty;
                                strChdOptionKey = string.Empty;
                                strInfOptionKey = string.Empty;
                                strBookingCount = string.Empty;
                                strAdtBookingCode = string.Empty;
                                strChdBookingCode = string.Empty;
                                strInfBookingCode = string.Empty;
                                strCabinClass = string.Empty;
                            }
                        }
                    }
                }

                dtFlightReference.Clear();
            }
        }

        if (dtFlights != null && dtFlights.Rows.Count > 0)
        {

        }

        return dtFlights;
    }

    public static DataTable SchemaApiFlt
    {
        get
        {
            DataTable Table = new DataTable();
            Table.TableName = "Flights";
            Table.Columns.Add("fltid1", typeof(Int32)).DefaultValue = 0;
            Table.Columns.Add("fltid2", typeof(Int32)).DefaultValue = 0;
            return Table;
        }
    }
    private string GetFareType(DataSet dsBrandList, DataTable dtBrand, string fareInfo_Id)
    {
        if (dsBrandList != null && dsBrandList.Tables.Count > 0)
        {
            DataRow[] drSelect = dtBrand.Select("fareInfo_Id='" + fareInfo_Id + "'");
            if (drSelect.Length.Equals(1))
            {
                DataRow[] drSelect2 = dsBrandList.Tables[0].Select("BrandID='" + drSelect.CopyToDataTable().Rows[0]["BrandID"].ToString() + "'");
                if (drSelect2.Length.Equals(1))
                {
                    return drSelect2.CopyToDataTable().Rows[0]["Name"].ToString();
                }
            }
        }
        return "";
    }
    */
}