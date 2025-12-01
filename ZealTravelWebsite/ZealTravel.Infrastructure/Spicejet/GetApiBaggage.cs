using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiBaggage
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiBaggage(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public Decimal GetRoundWayBaggage( DataTable dtOutbound, DataTable dtInbound, DataTable dtPassenger)
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
                objSellRequestData.SellBy = svc_booking.SellBy.SSR;
                objSellRequestData.SellBySpecified = true;

                svc_booking.SellSSR sellssr = new svc_booking.SellSSR();
                sellssr.SSRRequest = new svc_booking.SSRRequest();

                DataTable dtBaggagesOutbound = new DataTable();
                DataRow[] result = dtPassenger.Select("BaggageChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                if (result.Length > 0)
                {
                    dtBaggagesOutbound = result.CopyToDataTable();
                }

                DataTable dtBaggagesInbound = new DataTable();
                result = dtPassenger.Select("BaggageChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                if (result.Length > 0)
                {
                    dtBaggagesInbound = result.CopyToDataTable();
                }

                if (dtBaggagesOutbound.Rows.Count > 0 && dtBaggagesInbound.Rows.Count > 0)
                {
                    //DataRow dr = dtOutbound.Rows[0];
                    int NoOfSegments = dtOutbound.Rows.Count + dtInbound.Rows.Count;
                    sellssr.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[NoOfSegments];

                    int id = 0;
                    for (int p = 0; p < dtOutbound.Rows.Count; p++)
                    {
                        DataRow drr = dtOutbound.Rows[p];
                        sellssr.SSRRequest.SegmentSSRRequests[id] = new svc_booking.SegmentSSRRequest();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator = new svc_booking.FlightDesignator();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator.CarrierCode = drr["CarrierCode"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator.FlightNumber = drr["FlightNumber"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator.OpSuffix = " ";
                        sellssr.SSRRequest.SegmentSSRRequests[id].ArrivalStation = drr["ArrivalStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].DepartureStation = drr["DepartureStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].STD = Convert.ToDateTime(drr["DepDate"].ToString());
                        sellssr.SSRRequest.SegmentSSRRequests[id].STDSpecified = true;
                        sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs = new svc_booking.PaxSSR[dtBaggagesOutbound.Rows.Count];

                        for (short i = 0; i < dtBaggagesOutbound.Rows.Count; i++)
                        {
                            DataRow Pxdr = dtBaggagesOutbound.Rows[i];
                            short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i] = new svc_booking.PaxSSR();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].State = svc_booking.MessageState.New;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].StateSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].ActionStatusCode = "NN";
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRCode = Pxdr["BaggageCode_O"].ToString().Trim();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRNumber = i;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRNumberSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRValue = 0;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRValueSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].ArrivalStation = drr["ArrivalStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].DepartureStation = drr["DepartureStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].PassengerNumber = k;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].PassengerNumberSpecified = true;
                        }
                        id++;
                    }

                    //DataRow drIn = dtInbound.Rows[0];
                    for (int p = 0; p < dtInbound.Rows.Count; p++)
                    {
                        DataRow drr = dtInbound.Rows[p];
                        sellssr.SSRRequest.SegmentSSRRequests[id] = new svc_booking.SegmentSSRRequest();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator = new svc_booking.FlightDesignator();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator.CarrierCode = drr["CarrierCode"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator.FlightNumber = drr["FlightNumber"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].FlightDesignator.OpSuffix = " ";
                        sellssr.SSRRequest.SegmentSSRRequests[id].ArrivalStation = drr["ArrivalStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].DepartureStation = drr["DepartureStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[id].STD = Convert.ToDateTime(drr["DepDate"].ToString());
                        sellssr.SSRRequest.SegmentSSRRequests[id].STDSpecified = true;
                        sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs = new svc_booking.PaxSSR[dtBaggagesInbound.Rows.Count];

                        for (short i = 0; i < dtBaggagesInbound.Rows.Count; i++)
                        {
                            DataRow Pxdr = dtBaggagesInbound.Rows[i];
                            short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i] = new svc_booking.PaxSSR();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].State = svc_booking.MessageState.New;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].StateSpecified = true;
                           sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].ActionStatusCode = "NN";
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRCode = Pxdr["BaggageCode_I"].ToString().Trim();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRNumber = i;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRNumberSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRValue = 0;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].SSRValueSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].ArrivalStation = drr["ArrivalStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].DepartureStation = drr["DepartureStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].PassengerNumber = k;
                            sellssr.SSRRequest.SegmentSSRRequests[id].PaxSSRs[i].PassengerNumberSpecified = true;
                        }
                        id++;
                    }
                }
                else if (dtBaggagesOutbound.Rows.Count > 0 && dtBaggagesInbound.Rows.Count == 0)
                {
                    //DataRow dr = dtOutbound.Rows[0];
                    for (int w = 0; w < dtOutbound.Rows.Count; w++)
                    {
                        DataRow drr = dtOutbound.Rows[w];
                        sellssr.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtOutbound.Rows.Count];
                        sellssr.SSRRequest.SegmentSSRRequests[w] = new svc_booking.SegmentSSRRequest();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator = new svc_booking.FlightDesignator();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator.CarrierCode = drr["CarrierCode"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator.FlightNumber = drr["FlightNumber"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator.OpSuffix = " ";
                        sellssr.SSRRequest.SegmentSSRRequests[w].ArrivalStation = drr["ArrivalStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].DepartureStation = drr["DepartureStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].STD = Convert.ToDateTime(drr["DepDate"].ToString());
                        sellssr.SSRRequest.SegmentSSRRequests[w].STDSpecified = true;
                        sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs = new svc_booking.PaxSSR[dtBaggagesOutbound.Rows.Count];

                        for (short i = 0; i < dtBaggagesOutbound.Rows.Count; i++)
                        {
                            DataRow Pxdr = dtBaggagesOutbound.Rows[i];
                            short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i] = new svc_booking.PaxSSR();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].State = svc_booking.MessageState.New;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].StateSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].ActionStatusCode = "NN";
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRCode = Pxdr["BaggageCode_O"].ToString().Trim();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRNumber = i;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRNumberSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRValue = 0;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRValueSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].ArrivalStation = drr["ArrivalStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].DepartureStation = drr["DepartureStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].PassengerNumber = k;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].PassengerNumberSpecified = true;
                        }
                    }
                }
                else if (dtBaggagesOutbound.Rows.Count == 0 && dtBaggagesInbound.Rows.Count > 0)
                {
                    //DataRow drIn = dtInbound.Rows[0];
                    for (int w = 0; w < dtInbound.Rows.Count; w++)
                    {
                        DataRow drr = dtInbound.Rows[w];
                        sellssr.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtInbound.Rows.Count];
                        sellssr.SSRRequest.SegmentSSRRequests[w] = new svc_booking.SegmentSSRRequest();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator = new svc_booking.FlightDesignator();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator.CarrierCode = drr["CarrierCode"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator.FlightNumber = drr["FlightNumber"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].FlightDesignator.OpSuffix = " ";
                        sellssr.SSRRequest.SegmentSSRRequests[w].ArrivalStation = drr["ArrivalStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].DepartureStation = drr["DepartureStation"].ToString();
                        sellssr.SSRRequest.SegmentSSRRequests[w].STD = Convert.ToDateTime(drr["DepDate"].ToString());
                        sellssr.SSRRequest.SegmentSSRRequests[w].STDSpecified = true;
                        sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs = new svc_booking.PaxSSR[dtBaggagesInbound.Rows.Count];

                        for (short i = 0; i < dtBaggagesInbound.Rows.Count; i++)
                        {
                            DataRow Pxdr = dtBaggagesInbound.Rows[i];
                            short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i] = new svc_booking.PaxSSR();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].State = svc_booking.MessageState.New;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].StateSpecified=true;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].ActionStatusCode = "NN";
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRCode = Pxdr["BaggageCode_I"].ToString().Trim();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRNumber = i;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRNumberSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRValue = 0;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].SSRValueSpecified = true;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].ArrivalStation = drr["ArrivalStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].DepartureStation = drr["DepartureStation"].ToString();
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].PassengerNumber = k;
                            sellssr.SSRRequest.SegmentSSRRequests[w].PaxSSRs[i].PassengerNumberSpecified = true;
                        }
                    }
                }

                sellssr.SSRRequest.CurrencyCode = "INR";
                sellssr.SSRRequest.CancelFirstSSR = false;
                sellssr.SSRRequest.CancelFirstSSRSpecified = true;
                sellssr.SSRRequest.SSRFeeForceWaiveOnSell = false;
                objSellRequestData.SellSSR = sellssr;


                svc_booking.SellRequest objSellRequest = new svc_booking.SellRequest();
                objSellRequest.SellRequestData = objSellRequestData;
                objSellRequest.Signature = Signature;
                objSellRequest.ContractVersion = ContractVersion;

                GetApiRequest = GetCommonFunctions.Serialize(objSellRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.SellResponse objSellResponse = objIBookingManager.Sell(objSellRequest);
                if (objSellResponse != null && objSellResponse.BookingUpdateResponseData != null && objSellResponse.BookingUpdateResponseData.Success != null && objSellResponse.BookingUpdateResponseData.Success.PNRAmount != null)
                {
                    iTotalCost = (objSellResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost);
                }

                GetApiResponse = GetCommonFunctions.Serialize(objSellResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetRoundWayBaggage", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetRoundWayBaggage-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return iTotalCost;
        }
        public Decimal GetOneWayBaggage(DataTable dtBound, DataTable dtPassenger)
        {
            Decimal dTotalCost = 0;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                string FltType = dtBound.Rows[0]["FltType"].ToString().Trim();
                int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                DataTable dtBaggeges = new DataTable();
                DataRow[] result = dtPassenger.Select("BaggageChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                if (FltType.Equals("I"))
                {
                    result = dtPassenger.Select("BaggageChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                }

                if (result.Length > 0)
                {
                    dtBaggeges = result.CopyToDataTable();
                }

                if (dtBaggeges.Rows.Count > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                    objSellRequestData.SellBy = svc_booking.SellBy.SSR;
                    objSellRequestData.SellBySpecified = true;


                    svc_booking.SellSSR objSellSSR = new svc_booking.SellSSR();
                    objSellSSR.SSRRequest = new svc_booking.SSRRequest();

                    DataRow dr = dtBound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtBound.Rows.Count];

                    for (int n = 0; n < dtBound.Rows.Count; n++)
                    {
                        DataRow drr = dtBound.Rows[n];
                        objSellSSR.SSRRequest.SegmentSSRRequests[n] = new svc_booking.SegmentSSRRequest();
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].FlightDesignator = new svc_booking.FlightDesignator();
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].FlightDesignator.CarrierCode = drr["CarrierCode"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].FlightDesignator.FlightNumber = drr["FlightNumber"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].FlightDesignator.OpSuffix = " ";
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].ArrivalStation = drr["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].DepartureStation = drr["DepartureStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].STD = Convert.ToDateTime(drr["DepDate"].ToString());
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].STDSpecified = true;
                        objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs = new svc_booking.PaxSSR[dtBaggeges.Rows.Count];

                        for (short i = 0; i < dtBaggeges.Rows.Count; i++)
                        {
                            DataRow Pxdr = dtBaggeges.Rows[i];
                            short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i] = new svc_booking.PaxSSR();
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].State = svc_booking.MessageState.New;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].StateSpecified = true;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].ActionStatusCode = "NN";

                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].SSRCode = Pxdr["BaggageCode_O"].ToString().Trim();
                       
                            if (FltType.Equals("I"))
                            {
                                objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].SSRCode = Pxdr["BaggageCode_I"].ToString().Trim();
                            }

                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].SSRNumber = i;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].SSRNumberSpecified = true;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].SSRValue = 0;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].SSRValueSpecified = true;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].ArrivalStation = drr["ArrivalStation"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].DepartureStation = drr["DepartureStation"].ToString();
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].PassengerNumber = k;
                            objSellSSR.SSRRequest.SegmentSSRRequests[n].PaxSSRs[i].PassengerNumberSpecified = true;
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
                        dTotalCost = objSellResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                    }

                    GetApiResponse = GetCommonFunctions.Serialize(objSellResponse);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetOneWayBaggage", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace + "," + ex.StackTrace);
            }
            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetOneWayBaggage-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);

            return dTotalCost;
        }
    }
}
