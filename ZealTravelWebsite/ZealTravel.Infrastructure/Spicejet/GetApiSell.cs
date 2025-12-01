using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    public class GetApiSell
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiSell(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        public GetApiSell(string Searchid)
        {
            this.Searchid = Searchid;
        }
        //================================================================================================================================================
        public Decimal GetSellOneWay(DataTable dtBound)
        {
            Decimal iTotalCost = 0;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                objSellRequestData.SellBy = svc_booking.SellBy.JourneyBySellKey;
                objSellRequestData.SellBySpecified = true;

                objSellRequestData.SellJourneyByKeyRequest = new svc_booking.SellJourneyByKeyRequest();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new svc_booking.SellJourneyByKeyRequestData();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new svc_booking.SellKeyList[1];
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new svc_booking.SellKeyList();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = dtBound.Rows[0]["JourneySellKey"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = dtBound.Rows[0]["FareSellKey"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType = new svc_booking.PaxPriceType[iPaxCount];

                for (int i = 0; i < Adt; i++)
                {
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[i] = new svc_booking.PaxPriceType();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[i].PaxType = "ADT"; ;
                }
                for (int i = 0; i < Chd; i++)
                {
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[Adt + i] = new svc_booking.PaxPriceType();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[Adt + i].PaxType = "CHD";
                }

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.CurrencyCode = "INR";

                svc_booking.PointOfSale objPointOfSale = new svc_booking.PointOfSale();
                objPointOfSale.State = svc_booking.MessageState.New;
                objPointOfSale.StateSpecified = true;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS = objPointOfSale;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.AgentCode = "AG";
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.OrganizationCode = dtBound.Rows[0]["AirlineID"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.DomainCode = "WWW"; ;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.LocationCode = string.Empty;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCount = Convert.ToInt16(iPaxCount);
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCountSpecified = true;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.LoyaltyFilterSpecified = true;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.IsAllotmentMarketFare = false;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.IsAllotmentMarketFareSpecified = true;

                svc_booking.SellRequest objSellRequest = new svc_booking.SellRequest();
                objSellRequest.SellRequestData = objSellRequestData;
                objSellRequest.ContractVersion = ContractVersion;
                objSellRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objSellRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.SellResponse objSellResponse = objIBookingManager.Sell(objSellRequest);
                if (objSellResponse != null && objSellResponse.BookingUpdateResponseData != null && objSellResponse.BookingUpdateResponseData.Success != null && objSellResponse.BookingUpdateResponseData.Success.PNRAmount != null)
                {
                    iTotalCost = objSellResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                }

                GetApiResponse = GetCommonFunctions.Serialize(objSellResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetSellOneWay", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetSellOneWay-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);

            return iTotalCost;
        }
        public Decimal GetSellOneWayInfant(DataTable dtBound)
        {
            Decimal iTotalCost = 0;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                bool bSameCls = false;
                if (dtBound.Rows.Count.Equals(2))
                {
                    if (dtBound.Rows[0]["FlightNumber"].ToString().Trim().Equals(dtBound.Rows[1]["FlightNumber"].ToString().Trim()))
                    {
                        bSameCls = true;
                    }
                }
                else if (dtBound.Rows.Count.Equals(3))
                {
                    string sFlightNum1 = dtBound.Rows[0]["FlightNumber"].ToString().Trim();
                    string sFlightNum2 = dtBound.Rows[1]["FlightNumber"].ToString().Trim();
                    string sFlightNum3 = dtBound.Rows[2]["FlightNumber"].ToString().Trim();

                    if (sFlightNum1.Equals(sFlightNum2) && sFlightNum1.Equals(sFlightNum3))
                    {
                        bSameCls = true;
                    }
                    else if (sFlightNum1.Equals(sFlightNum2) && sFlightNum1 != sFlightNum3)
                    {

                    }
                    else if (sFlightNum1 != sFlightNum2 && sFlightNum2.Equals(sFlightNum3))
                    {

                    }
                }
                else if (dtBound.Rows.Count.Equals(1))
                {
                    bSameCls = true;
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                objSellRequestData.SellBy = svc_booking.SellBy.SSR;
                objSellRequestData.SellBySpecified = true;

                objSellRequestData.SellJourneyByKeyRequest = new svc_booking.SellJourneyByKeyRequest();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new svc_booking.SellJourneyByKeyRequestData();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new svc_booking.SellKeyList[1];
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new svc_booking.SellKeyList();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = dtBound.Rows[0]["JourneySellKey"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = dtBound.Rows[0]["FareSellKey"].ToString();

                svc_booking.SellSSR objSellSSR = new svc_booking.SellSSR();
                objSellSSR.SSRRequest = new svc_booking.SSRRequest();
                if (bSameCls.Equals(true))
                {
                    DataRow dr = dtBound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[1];
                    objSellSSR.SSRRequest.SegmentSSRRequests[0] = new svc_booking.SegmentSSRRequest();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new svc_booking.FlightDesignator();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation = dr["Destination"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation = dr["Origin"].ToString();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STD = Convert.ToDateTime(dr["DepartureTime"].ToString());
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new svc_booking.PaxSSR[Inf];
                    for (short i = 0; i < Inf; i++)
                    {
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new svc_booking.PaxSSR();
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = svc_booking.MessageState.New;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].StateSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = "INFT";

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumberSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValue = 0;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValueSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dr["Destination"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dr["Origin"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;
                    }
                }
                else
                {
                    if (dtBound.Rows.Count.Equals(3))
                    {
                        string sFlightNum1 = dtBound.Rows[0]["FlightNumber"].ToString().Trim();
                        string sFlightNum2 = dtBound.Rows[1]["FlightNumber"].ToString().Trim();
                        string sFlightNum3 = dtBound.Rows[2]["FlightNumber"].ToString().Trim();

                        if (sFlightNum1.Equals(sFlightNum2) && sFlightNum1 != sFlightNum3)
                        {
                            objSellSSR.SSRRequest = new svc_booking.SSRRequest();
                            objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[2];

                            DataRow dr1 = dtBound.Rows[0];
                            DataRow dr2 = dtBound.Rows[1];
                            DataRow dr3 = dtBound.Rows[2];

                            for (int j = 0; j < 2; j++)
                            {
                                if (j.Equals(0))
                                {
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr1["CarrierCode"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr1["FlightNumber"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr2["ArrivalStation"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr1["DepartureStation"].ToString();

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr1["DepartureTime"].ToString());
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                    for (short i = 0; i < Inf; i++)
                                    {
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr2["ArrivalStation"].ToString();
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr1["DepartureStation"].ToString();
                                    }
                                }
                                else if (j.Equals(1))
                                {
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr3["CarrierCode"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr3["FlightNumber"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr3["ArrivalStation"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr3["DepartureStation"].ToString();

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr3["DepartureTime"].ToString());
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                    for (short i = 0; i < Inf; i++)
                                    {
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";


                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr3["ArrivalStation"].ToString();
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr3["DepartureStation"].ToString();
                                    }
                                }

                                objSellSSR.SSRRequest.CurrencyCode = "INR";
                                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSell = false;
                                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                            }
                        }
                        else if (sFlightNum1 != sFlightNum2 && sFlightNum2.Equals(sFlightNum3))
                        {
                            objSellSSR.SSRRequest = new svc_booking.SSRRequest();
                            objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[2];

                            DataRow dr1 = dtBound.Rows[0];
                            DataRow dr2 = dtBound.Rows[1];
                            DataRow dr3 = dtBound.Rows[2];

                            for (int j = 0; j < 2; j++)
                            {
                                if (j.Equals(0))
                                {
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr1["CarrierCode"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr1["FlightNumber"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr1["ArrivalStation"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr1["DepartureStation"].ToString();

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr1["DepartureTime"].ToString());
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                    for (short i = 0; i < Inf; i++)
                                    {
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr1["ArrivalStation"].ToString();
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr1["DepartureStation"].ToString();
                                    }
                                }
                                else if (j.Equals(1))
                                {
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr2["CarrierCode"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr2["FlightNumber"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr3["ArrivalStation"].ToString();
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr2["DepartureStation"].ToString();

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr2["DepartureTime"].ToString());
                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                    objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                    for (short i = 0; i < Inf; i++)
                                    {
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr3["ArrivalStation"].ToString();
                                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr2["DepartureStation"].ToString();
                                    }
                                }

                                objSellSSR.SSRRequest.CurrencyCode = "INR";
                                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSell = false;
                                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                            }
                        }
                    }
                    else
                    {
                        objSellSSR.SSRRequest = new svc_booking.SSRRequest();
                        objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtBound.Rows.Count];

                        for (int j = 0; j < dtBound.Rows.Count; j++)
                        {
                            DataRow dr = dtBound.Rows[j];

                            objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr["ArrivalStation"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr["DepartureStation"].ToString();

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr["DepartureTime"].ToString());
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                            for (short i = 0; i < Inf; i++)
                            {
                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                                objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();
                            }

                            objSellSSR.SSRRequest.CurrencyCode = "INR";
                            objSellSSR.SSRRequest.SSRFeeForceWaiveOnSell = false;
                            objSellSSR.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                        }
                    }

                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtBound.Rows.Count];

                    for (int j = 0; j < dtBound.Rows.Count; j++)
                    {
                        DataRow dr = dtBound.Rows[j];

                        objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr["DepartureTime"].ToString());
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                        for (short i = 0; i < Inf; i++)
                        {
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;
                        }
                    }
                }

                objSellSSR.SSRRequest.CurrencyCode = "INR";
                objSellSSR.SSRRequest.CancelFirstSSR = false;
                objSellSSR.SSRRequest.CancelFirstSSRSpecified = true;

                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSell = false;
                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;

                objSellRequestData.SellSSR = objSellSSR;

                svc_booking.SellRequest objSellRequest = new svc_booking.SellRequest();
                objSellRequest.SellRequestData = objSellRequestData;
                objSellRequest.Signature = Signature;
                objSellRequest.ContractVersion = ContractVersion;

                GetApiRequest = GetCommonFunctions.Serialize(objSellRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.SellResponse objSellResponse = objIBookingManager.Sell(objSellRequest);
                if (objSellResponse != null && objSellResponse.BookingUpdateResponseData != null && objSellResponse.BookingUpdateResponseData.Success != null && objSellResponse.BookingUpdateResponseData.Success.PNRAmount != null)
                {
                    iTotalCost = objSellResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                }

                GetApiResponse = GetCommonFunctions.Serialize(objSellResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetSellOneWayInfant", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetSellOneWayInfant-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return iTotalCost;
        }
        private DataTable GetArrangeSegments(DataTable Dt)
        {
            DataTable dtBound = new DataTable();

            bool bSameCls = false;
            if (Dt.Rows.Count.Equals(2))
            {
                if (Dt.Rows[0]["FlightNumber"].ToString().Trim().Equals(Dt.Rows[1]["FlightNumber"].ToString().Trim()))
                {
                    bSameCls = true;
                }
            }
            else if (Dt.Rows.Count.Equals(3))
            {
                if (Dt.Rows[0]["FlightNumber"].ToString().Trim().Equals(Dt.Rows[1]["FlightNumber"].ToString().Trim()) && Dt.Rows[0]["FlightNumber"].ToString().Trim().Equals(Dt.Rows[2]["FlightNumber"].ToString().Trim()))
                {
                    bSameCls = true;
                }
            }
            if (Dt.Rows.Count.Equals(1))
            {
                bSameCls = true;
            }

            if (bSameCls.Equals(true))
            {
                dtBound = Dt.Clone();
                DataRow dr1 = Dt.Rows[0];
                dr1["DepartureStation"] = dr1["Origin"];
                dr1["ArrivalStation"] = dr1["Destination"];
                dtBound.ImportRow(dr1);
                dtBound.AcceptChanges();
            }
            else
            {
                if (Dt.Rows.Count.Equals(3))
                {
                    dtBound = Dt.Clone();
                    string sFlightNum1 = Dt.Rows[0]["FlightNumber"].ToString().Trim();
                    string sFlightNum2 = Dt.Rows[1]["FlightNumber"].ToString().Trim();
                    string sFlightNum3 = Dt.Rows[2]["FlightNumber"].ToString().Trim();

                    if (sFlightNum1.Equals(sFlightNum2) && sFlightNum1 != sFlightNum3)
                    {
                        DataRow dr1 = Dt.Rows[0];
                        DataRow dr2 = Dt.Rows[1];
                        DataRow dr3 = Dt.Rows[2];

                        for (int j = 0; j < 2; j++)
                        {
                            if (j.Equals(0))
                            {
                                dtBound.ImportRow(dr1);
                                dtBound.Rows[0]["ArrivalStation"] = dr2["ArrivalStation"];
                                dtBound.AcceptChanges();
                            }
                            else if (j.Equals(1))
                            {
                                dtBound.ImportRow(dr3);
                                dtBound.AcceptChanges();
                            }
                        }
                    }
                    else if (sFlightNum1 != sFlightNum2 && sFlightNum2.Equals(sFlightNum3))
                    {
                        DataRow dr1 = Dt.Rows[0];
                        DataRow dr2 = Dt.Rows[1];
                        DataRow dr3 = Dt.Rows[2];

                        for (int j = 0; j < 2; j++)
                        {
                            if (j.Equals(0))
                            {
                                dtBound.ImportRow(dr1);
                                dtBound.AcceptChanges();
                            }
                            else if (j.Equals(1))
                            {
                                dtBound.ImportRow(dr2);
                                dtBound.Rows[1]["ArrivalStation"] = dr3["ArrivalStation"];
                                dtBound.AcceptChanges();
                            }
                        }
                    }
                    else
                    {
                        dtBound = Dt;
                        dtBound.AcceptChanges();
                    }
                }
                else
                {
                    dtBound = Dt;
                    dtBound.AcceptChanges();
                }
            }
            return dtBound;
        }
        //================================================================================================================================================
        public Decimal GetSellRoundWay(DataTable dtOutbound, DataTable dtInbound)
        {
            Decimal iTotalCost = 0;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                Supplierid = dtOutbound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtOutbound.Rows[0]["Api_SessionID"].ToString().Trim();
                int Adt = int.Parse(dtOutbound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtOutbound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtOutbound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                objSellRequestData.SellBy = svc_booking.SellBy.JourneyBySellKey;
                objSellRequestData.SellBySpecified = true;

                objSellRequestData.SellJourneyByKeyRequest = new svc_booking.SellJourneyByKeyRequest();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new svc_booking.SellJourneyByKeyRequestData();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new svc_booking.SellKeyList[2];
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new svc_booking.SellKeyList();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = dtOutbound.Rows[0]["JourneySellKey"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = dtOutbound.Rows[0]["FareSellKey"].ToString();

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[1] = new svc_booking.SellKeyList();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[1].JourneySellKey = dtInbound.Rows[0]["JourneySellKey"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[1].FareSellKey = dtInbound.Rows[0]["FareSellKey"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType = new svc_booking.PaxPriceType[iPaxCount];

                for (int i = 0; i < Adt; i++)
                {
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[i] = new svc_booking.PaxPriceType();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[i].PaxType = "ADT"; ;
                }
                for (int i = 0; i < Chd; i++)
                {
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[Adt + i] = new svc_booking.PaxPriceType();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxPriceType[Adt + i].PaxType = "CHD";
                }

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.CurrencyCode = "INR";

                svc_booking.PointOfSale pst = new svc_booking.PointOfSale();
                pst.State = svc_booking.MessageState.New;
                pst.StateSpecified = true;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS = pst;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.AgentCode = "AG";
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.OrganizationCode = dtOutbound.Rows[0]["AirlineID"].ToString();
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.DomainCode = "WWW";
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.SourcePOS.LocationCode = string.Empty;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCount = Convert.ToInt16(iPaxCount);
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.PaxCountSpecified = true;


                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.LoyaltyFilterSpecified = true;

                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.IsAllotmentMarketFare = false;
                objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.IsAllotmentMarketFareSpecified = true;

                svc_booking.SellRequest objSellRequest = new svc_booking.SellRequest();
                objSellRequest.SellRequestData = objSellRequestData;
                objSellRequest.ContractVersion = ContractVersion;
                objSellRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objSellRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.SellResponse objSellRes = objIBookingManager.Sell(objSellRequest);
                if (objSellRes != null && objSellRes.BookingUpdateResponseData != null && objSellRes.BookingUpdateResponseData.Success != null && objSellRes.BookingUpdateResponseData.Success.PNRAmount != null)
                {
                    iTotalCost = objSellRes.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                }

                GetApiResponse = GetCommonFunctions.Serialize(objSellRes);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetSellRoundWay", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetSellRoundWay-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return iTotalCost;
        }
        public Decimal GetSellRoundWayInfant(DataTable dtOutbound, DataTable dtInbound)
        {
            Decimal iTotalCost = 0;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                int ContractVersion = 420;
                Supplierid = dtOutbound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtOutbound.Rows[0]["Api_SessionID"].ToString().Trim();
                int Adt = int.Parse(dtOutbound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtOutbound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtOutbound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                objSellRequestData.SellBy = svc_booking.SellBy.SSR;
                objSellRequestData.SellBySpecified = true;

                svc_booking.SellSSR objSellSSR = new svc_booking.SellSSR();
                objSellSSR.SSRRequest = new svc_booking.SSRRequest();

                if (Inf > 0)
                {
                    DataTable dtArranged = new DataTable();
                    dtArranged = dtOutbound.Clone();
                    dtArranged.Merge(GetArrangeSegments(dtOutbound));
                    dtArranged.Merge(GetArrangeSegments(dtInbound));
                    dtArranged.AcceptChanges();

                    objSellRequestData.SellJourneyByKeyRequest = new svc_booking.SellJourneyByKeyRequest();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData = new svc_booking.SellJourneyByKeyRequestData();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.ActionStatusCode = "NN";
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys = new svc_booking.SellKeyList[1];
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0] = new svc_booking.SellKeyList();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].JourneySellKey = dtOutbound.Rows[0]["JourneySellKey"].ToString();
                    objSellRequestData.SellJourneyByKeyRequest.SellJourneyByKeyRequestData.JourneySellKeys[0].FareSellKey = dtOutbound.Rows[0]["FareSellKey"].ToString();

                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtArranged.Rows.Count];

                    for (int j = 0; j < dtArranged.Rows.Count; j++)
                    {
                        DataRow dr = dtArranged.Rows[j];

                        objSellSSR.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr["DepartureTime"].ToString());
                        objSellSSR.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];
                        for (short i = 0; i < Inf; i++)
                        {
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();

                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                            objSellSSR.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;
                        }
                    }
                }


                objSellSSR.SSRRequest.CurrencyCode = "INR";

                objSellSSR.SSRRequest.CancelFirstSSR = false;
                objSellSSR.SSRRequest.CancelFirstSSRSpecified = true;

                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSell = false;
                objSellSSR.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;

                objSellRequestData.SellSSR = objSellSSR;

                svc_booking.SellRequest objSellRequest = new svc_booking.SellRequest();
                objSellRequest.SellRequestData = objSellRequestData;
                objSellRequest.Signature = Signature;
                objSellRequest.ContractVersion = ContractVersion;

                GetApiRequest = GetCommonFunctions.Serialize(objSellRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.SellResponse objSellResponse = objIBookingManager.Sell(objSellRequest);
                if (objSellResponse != null && objSellResponse.BookingUpdateResponseData != null && objSellResponse.BookingUpdateResponseData.Success != null && objSellResponse.BookingUpdateResponseData.Success.PNRAmount != null)
                {
                    iTotalCost = objSellResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                }

                GetApiResponse = GetCommonFunctions.Serialize(objSellResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetSellRoundWayInfant", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
          //  dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetSellRoundWayInfant-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return iTotalCost;
        }

        //================================================================================================================================================
        public void ClearSellFromApi(Int32 ContractVersion, string Signature)
        {
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
            svc_booking.ClearRequest objClearRequest = new svc_booking.ClearRequest();
            objClearRequest.ContractVersion = ContractVersion;
            objClearRequest.Signature = Signature;

            GetApiRequest = GetCommonFunctions.Serialize(objClearRequest);

            svc_booking.ClearResponse objClearResponse = objIBookingManager.Clear(objClearRequest);

            GetApiResponse = GetCommonFunctions.Serialize(objClearResponse);

            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "ClearSellFromApi-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, "", Searchid);
        }
    }
}
