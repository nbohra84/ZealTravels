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

namespace ZealTravel.Infrastructure.UAPI
{
    class GetAvailabilityResponse2
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string Adt;
        private string Chd;
        private string Inf;

        private string Origin;
        private string Destination;

        private string SearchID;
        private string CompanyID;
        private string PSQRequest;
        private string UApiResponse;

        public GetAvailabilityResponse2(string NetworkUserName, string NetworkPassword, string TargetBranch, string Adt, string Chd, string Inf, string Origin, string Destination)
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
        public DataTable SetAvailabilityResponse(string SearchID, string CompanyID, string Sector, bool IsRT, bool IsMC, string PSQRequest, string UApiResponse, string FltType, string AdtBTR, string ChdBTR, string InfBTR, bool UAPISME = false)
        {
            DataTable dtBound = DBCommon.Schema.SchemaFlights;

            try
            {
                this.SearchID = SearchID;
                this.CompanyID = CompanyID;
                this.PSQRequest = PSQRequest;
                this.UApiResponse = UApiResponse;

                Console.WriteLine($"[SetAvailabilityResponse] Starting - SearchID: {SearchID}, Response length: {UApiResponse?.Length ?? 0}");
                
                XmlDocument xDocResponse = new XmlDocument();
                xDocResponse.LoadXml(UApiResponse);
                XmlNode ddd = xDocResponse.LastChild.LastChild.FirstChild;
                
                Console.WriteLine($"[SetAvailabilityResponse] XML loaded successfully, Root node: {ddd?.Name ?? "null"}");

                string FlightDetailsList = "";
                string AirSegmentList = "";
                string FareInfoList = "";
                string RouteList = "";
                string AirPricePointList = "";
                string BrandList = "";

                foreach (XmlNode childNode in ddd.ChildNodes)
                {
                    string ssss = childNode.OuterXml;
                    if (childNode.OuterXml.IndexOf("FlightDetailsList") != -1)
                    {
                        FlightDetailsList = childNode.OuterXml;
                    }
                    else if (childNode.OuterXml.IndexOf("AirSegmentList") != -1)
                    {
                        AirSegmentList = childNode.OuterXml;
                    }
                    else if (childNode.OuterXml.IndexOf("FareInfoList") != -1)
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
                }

                Console.WriteLine($"[SetAvailabilityResponse] Parsed sections - FlightDetailsList: {FlightDetailsList?.Length ?? 0}, AirSegmentList: {AirSegmentList?.Length ?? 0}, AirPricePointList: {AirPricePointList?.Length ?? 0}");

                DataSet dsFlightDetailsList = new DataSet();
                DataSet dsAirSegmentList = new DataSet();
                DataSet dsFareInfoList = new DataSet();
                DataSet dsRouteList = new DataSet();
                DataSet dsBrandList = new DataSet();
                
                try
                {
                    if (!string.IsNullOrEmpty(FlightDetailsList))
                        dsFlightDetailsList.ReadXml(new System.IO.StringReader(FlightDetailsList));
                    if (!string.IsNullOrEmpty(AirSegmentList))
                        dsAirSegmentList.ReadXml(new System.IO.StringReader(AirSegmentList));
                    if (!string.IsNullOrEmpty(FareInfoList))
                        dsFareInfoList.ReadXml(new System.IO.StringReader(FareInfoList));
                    if (!string.IsNullOrEmpty(RouteList))
                        dsRouteList.ReadXml(new System.IO.StringReader(RouteList));
                    if (!string.IsNullOrEmpty(BrandList))
                        dsBrandList.ReadXml(new System.IO.StringReader(BrandList));
                    
                    Console.WriteLine($"[SetAvailabilityResponse] DataSets loaded - FlightDetails: {dsFlightDetailsList.Tables.Count}, AirSegment: {dsAirSegmentList.Tables.Count}, FareInfo: {dsFareInfoList.Tables.Count}, Route: {dsRouteList.Tables.Count}, Brand: {dsBrandList.Tables.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SetAvailabilityResponse] ERROR loading DataSets: {ex.Message}");
                    Console.WriteLine($"[SetAvailabilityResponse] StackTrace: {ex.StackTrace}");
                }

                string ORGG = "";
                string DESS = "";

                Console.WriteLine($"[SetAvailabilityResponse] Calling GetFlightReferences with AirPricePointList length: {AirPricePointList?.Length ?? 0}");
                DataTable dtFlights = GetFlightReferences(AirPricePointList);
                Console.WriteLine($"[SetAvailabilityResponse] GetFlightReferences returned - Rows: {dtFlights?.Rows?.Count ?? 0}, AirPricePointList length: {AirPricePointList?.Length ?? 0}");
                
                if (dtFlights != null && dtFlights.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFlights.Rows)
                    {
                        string SegmentRef = dr["AdtSegmentRef"].ToString();
                        if (SegmentRef.Length > 0)
                        {
                            string[] SplitSegmentRef = SegmentRef.Split('?');
                            for (int i = 0; i < SplitSegmentRef.Length; i++)
                            {
                                DataRow[] drAirSegemnt = dsAirSegmentList.Tables["AirSegment"].Select("Key='" + SplitSegmentRef[i].ToString().Trim() + "'");

                                int k22 = drAirSegemnt.Length;
                                if (k22 > 1)
                                {

                                }

                                if (drAirSegemnt.Length > 0)
                                {
                                    DataRow drAdd = dtBound.NewRow();

                                    string[] SplitClass = dr["AdtBookingCode"].ToString().Split('?');
                                    drAdd["ClassOfService"] = SplitClass[i].ToString().Trim();

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

                                    drAdd["AirlineID"] = TargetBranch;


                                    drAdd["Connection_O"] = dr["Connection_O"].ToString();
                                    drAdd["Connection_I"] = dr["Connection_I"].ToString();
                                    drAdd["AdtBTR"] = AdtBTR;
                                    drAdd["ChdBTR"] = ChdBTR;
                                    drAdd["InfBTR"] = InfBTR;

                                    drAdd["Sector"] = Sector;
                                    drAdd["FltType"] = FltType;

                                    drAdd["BookingCode"] = dr["AdtBookingCode"].ToString();
                                    //drAdd["ClassOfService"] = dr["AdtBookingCode"].ToString();
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
                                    drAdd["SegmentRef"] = drAirSegemnt.CopyToDataTable().Rows[0]["Key"].ToString();

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

                                    if(FltType.Equals("O"))
                                    {
                                        if (UAPISME)
                                        {
                                            drAdd["Refid"] = 100 + Convert.ToInt32(dr["refid"].ToString());
                                        }
                                        else
                                        {
                                            drAdd["Refid"] = 3000 + Convert.ToInt32(dr["refid"].ToString());
                                        }
                                    }
                                    else
                                    {

                                        if (UAPISME)
                                        {
                                            drAdd["Refid"] = 18000 + Convert.ToInt32(dr["refid"].ToString());
                                        }

                                        else
                                        {
                                            drAdd["Refid"] = 13000 + Convert.ToInt32(dr["refid"].ToString());
                                        }
                                    }
                                    

                                    drAdd["CancellationFee"] = dr["CancellationFee"].ToString();
                                    drAdd["DateChangeFee"] = dr["DateChangeFee"].ToString();
                                    drAdd["PriceType"] = ".PUB";
                                    //--------------------------------------------------------------------------------------------------------------------

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
                                            drAdd["TravelTime"] = drFlightDetails.CopyToDataTable().Rows[0]["TravelTime"].ToString();
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
                                            drAdd["CodeshareInfoOperatingCarrier"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString();
                                            if(drCodeshareInfo.CopyToDataTable().Columns.Contains("OperatingFlightNumber"))
                                            {
                                                drAdd["CodeshareInfoOperatingFlightNumber"] = drCodeshareInfo.CopyToDataTable().Rows[0]["OperatingFlightNumber"].ToString();
                                            }
                                            
                                            drAdd["CodeshareInfo"] = drCodeshareInfo.CopyToDataTable().Rows[0]["CodeshareInfo_Text"].ToString();
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
                                            //for AI SME price type
                                            if (dsFareInfoList.Tables["AccountCode"] != null)
                                            {
                                                DataRow[] drAccountCode = dsFareInfoList.Tables["AccountCode"].Select("FareInfo_Id='" + drFare.CopyToDataTable().Rows[0]["FareInfo_Id"].ToString() + "'");
                                                if (drAccountCode.Length > 0)
                                                {
                                                    string accountCode = drAccountCode[0]["Code"].ToString();
                                                    drAdd["PriceType"] = "." + accountCode;
                                                }
                                            }
                                            if (drFare.CopyToDataTable().Columns.Contains("FareFamily") && drAdd["PriceType"].ToString().Trim().Length.Equals(0))
                                            {
                                                drAdd["PriceType"] = "." + drFare.CopyToDataTable().Rows[0]["FareFamily"].ToString();
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
                                    dtBound.Rows.Add(drAdd);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SetAvailabilityResponse] EXCEPTION - SearchID: {SearchID}, Error: {ex.Message}");
                Console.WriteLine($"[SetAvailabilityResponse] StackTrace: {ex.StackTrace}");
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetAvailabilityResponse2" + "air_uapi", PSQRequest, UApiResponse, SearchID, ex.Message + "," + ex.StackTrace);
            }
            Console.WriteLine($"[SetAvailabilityResponse] Returning DataTable - Rows: {dtBound?.Rows?.Count ?? 0}, SearchID: {SearchID}");
            return dtBound;
        }
        private DataTable GetFlightReferences(string AirPricePointList)
        {
            DataTable dtFlights = Schemas.SchemaApiFlights;
            DataTable dtFlightReference = Schemas.SchemaApiFlightsReference;
            DataTable dtBound = DBCommon.Schema.SchemaFlights;

            try
            {
                Console.WriteLine($"[GetFlightReferences] Starting - AirPricePointList length: {AirPricePointList?.Length ?? 0}");
                
                if (string.IsNullOrEmpty(AirPricePointList))
                {
                    Console.WriteLine($"[GetFlightReferences] ERROR: AirPricePointList is null or empty!");
                    return dtFlights;
                }

                XmlDocument xDocAirPricePointList = new XmlDocument();
                xDocAirPricePointList.LoadXml(AirPricePointList);
                XmlNodeList XmlNodeListAirPricePointList = xDocAirPricePointList.LastChild.ChildNodes;
                
                Console.WriteLine($"[GetFlightReferences] XML loaded successfully, AirPricePoint nodes count: {XmlNodeListAirPricePointList?.Count ?? 0}");

              
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

                            foreach (DataRow dr in dsPaxFare.Tables["Option"].Rows)
                            {
                                DataRow[] drSelect = dsPaxFare.Tables["BookingInfo"].Select("Option_Id='" + dr["Option_Id"].ToString() + "'");
                                if (drSelect.Length > 0)
                                {
                                    foreach (DataRow drr in drSelect.CopyToDataTable().Rows)
                                    {
                                        DataRow drAdd = dtFlightReference.NewRow();
                                        if (dsPaxFare.Tables["Connection"] != null)
                                        {
                                            DataRow[] drSegmentIndex1 = dsPaxFare.Tables["Connection"].Select("Option_Id='" + dr["Option_Id"].ToString() + "'");
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

                                        drAdd["fltid"] = Convert.ToInt32(drr["Option_Id"].ToString()) + fltid;
                                        drAdd["AirPricePointKey"] = AirPricePointKey;

                                        if (dsPaxFare.Tables["PassengerType"] != null && dsPaxFare.Tables["PassengerType"].Rows[0]["Code"].ToString().Equals("ADT"))
                                        {
                                            drAdd["AirPricingInfoKey"] = "AA-" + AirPriceInfoKey;

                                            string Taxes = "";
                                            Taxes += "TotalPrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString() + "?";
                                            if (dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
                                            {
                                                Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString() + "?";
                                            }
                                            else
                                            {
                                                Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ApproximateBasePrice"].ToString() + "?";
                                            }

                                            Taxes += "Taxes:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Taxes"].ToString() + "?";
                                            foreach (DataRow drTaxes in dsPaxFare.Tables["TaxInfo"].Rows)
                                            {
                                                Taxes += drTaxes["Category"].ToString() + ":" + drTaxes["Amount"].ToString() + "?";
                                            }
                                            drAdd["AdtTaxes"] = Taxes;

                                            //string Extra = "";
                                            //Extra += "LatestTicketingTime:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["LatestTicketingTime"].ToString() + "?";
                                            //Extra += "PricingMethod:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["PricingMethod"].ToString() + "?";
                                            //Extra += "Refundable:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Refundable"].ToString() + "?";
                                            //Extra += "ETicketability:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ETicketability"].ToString() + "?";
                                            //Extra += "PlatingCarrier:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["PlatingCarrier"].ToString() + "?";
                                            //Extra += "ProviderCode:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ProviderCode"].ToString() + "?";
                                            //Extra += "Cat35Indicator:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Cat35Indicator"].ToString() + "?";
                                            //drAdd["Extra"] = Extra;


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
                                            if (dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
                                            {
                                                Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString() + "?";
                                            }
                                            else
                                            {
                                                Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ApproximateBasePrice"].ToString() + "?";
                                            }
                                            Taxes += "Taxes:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["Taxes"].ToString() + "?";
                                            foreach (DataRow drTaxes in dsPaxFare.Tables["TaxInfo"].Rows)
                                            {
                                                Taxes += drTaxes["Category"].ToString() + ":" + drTaxes["Amount"].ToString() + "?";
                                            }
                                            drAdd["ChdTaxes"] = Taxes;
                                        }
                                        else if (dsPaxFare.Tables["PassengerType"] != null && dsPaxFare.Tables["PassengerType"].Rows[0]["Code"].ToString().Equals("INF"))
                                        {
                                            drAdd["AirPricingInfoKey"] = "II-" + AirPriceInfoKey;

                                            string Taxes = "";
                                            Taxes += "TotalPrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString() + "?";
                                            if (dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
                                            {
                                                Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["BasePrice"].ToString() + "?";
                                            }
                                            else
                                            {
                                                Taxes += "BasePrice:" + dsPaxFare.Tables["AirPricingInfo"].Rows[0]["ApproximateBasePrice"].ToString() + "?";
                                            }
                                           
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

                        if (dtFlightReference != null && dtFlightReference.Rows.Count > 0)
                        {
                            ArrayList ArayFlts = DBCommon.CommonFunction.DataTable2ArrayList(dtFlightReference, "fltid", true);
                            if (ArayFlts.Count > 0)
                            {
                                for (int i = 0; i < ArayFlts.Count; i++)
                                {
                                    refid++;
                                    string ConnO = string.Empty;
                                    string strfltid = "";
                                    string strAirPricePointKey = string.Empty;
                                    string strAdtAirPricingInfoKey = string.Empty;
                                    string strChdAirPricingInfoKey = string.Empty;
                                    string strInfAirPricingInfoKey = string.Empty;

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
                                                ConnO = dr["Connection_O"].ToString();
                                                dc = dr["DateChangeFee"].ToString();
                                                cf = dr["CancellationFee"].ToString();

                                                strAdtAirPricingInfoKey = dr["AirPricingInfoKey"].ToString();
                                                strAdtSegmentRef = strAdtSegmentRef + dr["SegmentRef"].ToString() + "?";
                                                strAdtFareInfoRef = strAdtFareInfoRef + dr["FareInfoRef"].ToString() + "?";
                                                strAdtOptionKey = dr["OptionKey"].ToString();
                                                strAdtBookingCode = strAdtBookingCode + dr["BookingCode"].ToString() + "?";
                                                strAdtTaxes = dr["AdtTaxes"].ToString();
                                                Extra = dr["Extra"].ToString();
                                            }
                                            if (dr["AirPricingInfoKey"].ToString().IndexOf("CC-") != -1)
                                            {
                                                strChdAirPricingInfoKey = dr["AirPricingInfoKey"].ToString();
                                                //strAdtSegmentRef = strAdtSegmentRef + dr["SegmentRef"].ToString() + "?";
                                                strChdFareInfoRef = strChdFareInfoRef + dr["FareInfoRef"].ToString() + "?";
                                                strChdOptionKey = dr["OptionKey"].ToString();
                                                //strAdtBookingCode = strAdtBookingCode + dr["BookingCode"].ToString() + "?";
                                                strChdTaxes = dr["ChdTaxes"].ToString();
                                            }
                                            if (dr["AirPricingInfoKey"].ToString().IndexOf("II-") != -1)
                                            {
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
                                            strAdtSegmentRef = strAdtSegmentRef.Substring(0, strAdtSegmentRef.Length - 1);
                                            strAdtFareInfoRef = strAdtFareInfoRef.Substring(0, strAdtFareInfoRef.Length - 1);
                                            strAdtBookingCode = strAdtBookingCode.Substring(0, strAdtBookingCode.Length - 1);
                                        }
                                        if (strChdFareInfoRef.Length > 0)
                                        {
                                            strChdFareInfoRef = strChdFareInfoRef.Substring(0, strChdFareInfoRef.Length - 1);
                                        }
                                        if (strInfFareInfoRef.Length > 0)
                                        {
                                            strInfFareInfoRef = strInfFareInfoRef.Substring(0, strInfFareInfoRef.Length - 1);
                                        }

                                        DataRow drAdd = dtFlights.NewRow();
                                        drAdd["Connection_O"] = ConnO;
                                        drAdd["DateChangeFee"] = dc;
                                        drAdd["CancellationFee"] = cf;

                                        drAdd["Extra"] = Extra;
                                        drAdd["refid"] = strfltid;
                                        drAdd["AirPricePointKey"] = strAirPricePointKey;

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

                Console.WriteLine($"[GetFlightReferences] Processed - dtFlights rows: {dtFlights?.Rows?.Count ?? 0}, dtFlightReference rows: {dtFlightReference?.Rows?.Count ?? 0}");
                
                if (dtFlights != null && dtFlights.Rows.Count > 0)
                {
                    Console.WriteLine($"[GetFlightReferences] SUCCESS: Found {dtFlights.Rows.Count} flight references");
                }
                else
                {
                    Console.WriteLine($"[GetFlightReferences] WARNING: No flight references found. AirPricePointList preview: {(AirPricePointList?.Length > 500 ? AirPricePointList.Substring(0, 500) : AirPricePointList)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetFlightReferences] EXCEPTION - Error: {ex.Message}");
                Console.WriteLine($"[GetFlightReferences] StackTrace: {ex.StackTrace}");
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetFlightReference" + "air_uapi", PSQRequest, UApiResponse, SearchID, ex.Message + "," + ex.StackTrace);
            }

            Console.WriteLine($"[GetFlightReferences] Returning - Rows: {dtFlights?.Rows?.Count ?? 0}");
            return dtFlights;
        }
    }
}
