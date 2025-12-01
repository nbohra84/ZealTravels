using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiMeal
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiMeal(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public Decimal GetRoundWayMeal(DataTable dtOutbound, DataTable dtInbound, DataTable dtPassenger)
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

                DataTable dtOutboundMealData = new DataTable();
                DataRow[] drResults = dtPassenger.Select("MealChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                if (drResults.Length > 0)
                {
                    dtOutboundMealData = drResults.CopyToDataTable();
                }

                DataTable dtInboundMealData = new DataTable();
                drResults = dtPassenger.Select("MealChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                if (drResults.Length > 0)
                {
                    dtInboundMealData = drResults.CopyToDataTable();
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                objSellRequestData.SellBy = svc_booking.SellBy.SSR;
                objSellRequestData.SellBySpecified = true;

                svc_booking.SellSSR objSellSSR = new svc_booking.SellSSR();
                objSellSSR.SSRRequest = new svc_booking.SSRRequest();

                if (dtOutboundMealData.Rows.Count > 0 && dtInboundMealData.Rows.Count > 0)
                {
                    DataRow dr = dtOutbound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[2];
                    objSellSSR.SSRRequest.SegmentSSRRequests[0] = new svc_booking.SegmentSSRRequest();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new svc_booking.FlightDesignator();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation = dr["ArrivalStation"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation = dr["DepartureStation"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STD = Convert.ToDateTime(dr["DepDate"].ToString());
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new svc_booking.PaxSSR[dtOutboundMealData.Rows.Count];

                    for (short i = 0; i < dtOutboundMealData.Rows.Count; i++)
                    {
                        DataRow Pxdr = dtOutboundMealData.Rows[i];
                        short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new svc_booking.PaxSSR();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = svc_booking.MessageState.New;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].StateSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = Pxdr["MealCode_O"].ToString().Trim();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumberSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValue = 0;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValueSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = k;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;
                    }

                    DataRow drIn = dtInbound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests[1] = new svc_booking.SegmentSSRRequest();
                    objSellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator = new svc_booking.FlightDesignator();

                    objSellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.CarrierCode = drIn["CarrierCode"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.FlightNumber = drIn["FlightNumber"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[1].FlightDesignator.OpSuffix = " ";
                    objSellSSR.SSRRequest.SegmentSSRRequests[1].ArrivalStation = drIn["ArrivalStation"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[1].DepartureStation = drIn["DepartureStation"].ToString();

                    objSellSSR.SSRRequest.SegmentSSRRequests[1].STD = Convert.ToDateTime(drIn["DepDate"].ToString());
                    objSellSSR.SSRRequest.SegmentSSRRequests[1].STDSpecified = true;

                    objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs = new svc_booking.PaxSSR[dtInboundMealData.Rows.Count];

                    for (short i = 0; i < dtInboundMealData.Rows.Count; i++)
                    {
                        DataRow Pxdr = dtInboundMealData.Rows[i];
                        short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i] = new svc_booking.PaxSSR();

                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].State = svc_booking.MessageState.New;
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].StateSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ActionStatusCode = "NN";
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRCode = Pxdr["MealCode_I"].ToString().Trim();

                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRNumberSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRValue = 0;
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].SSRValueSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].ArrivalStation = drIn["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].DepartureStation = drIn["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumber = k;
                        objSellSSR.SSRRequest.SegmentSSRRequests[1].PaxSSRs[i].PassengerNumberSpecified = true;
                    }
                }
                else if (dtOutboundMealData.Rows.Count > 0 && dtInboundMealData.Rows.Count == 0)
                {
                    DataRow dr = dtOutbound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[1];
                    objSellSSR.SSRRequest.SegmentSSRRequests[0] = new svc_booking.SegmentSSRRequest();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new svc_booking.FlightDesignator();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation = dr["ArrivalStation"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation = dr["DepartureStation"].ToString();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STD = Convert.ToDateTime(dr["DepDate"].ToString());
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new svc_booking.PaxSSR[dtOutboundMealData.Rows.Count];

                    for (short i = 0; i < dtOutboundMealData.Rows.Count; i++)
                    {
                        DataRow Pxdr = dtOutboundMealData.Rows[i];
                        short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new svc_booking.PaxSSR();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = svc_booking.MessageState.New;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].StateSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = Pxdr["MealCode_O"].ToString().Trim();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumberSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValue = 0;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValueSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = k;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;
                    }
                }
                else if (dtOutboundMealData.Rows.Count == 0 && dtInboundMealData.Rows.Count > 0)
                {
                    DataRow drIn = dtInbound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[1];
                    objSellSSR.SSRRequest.SegmentSSRRequests[0] = new svc_booking.SegmentSSRRequest();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new svc_booking.FlightDesignator();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = drIn["CarrierCode"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = drIn["FlightNumber"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation = drIn["ArrivalStation"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation = drIn["DepartureStation"].ToString();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STD = Convert.ToDateTime(drIn["DepDate"].ToString());
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new svc_booking.PaxSSR[dtInboundMealData.Rows.Count];

                    for (short i = 0; i < dtInboundMealData.Rows.Count; i++)
                    {
                        DataRow Pxdr = dtInboundMealData.Rows[i];
                        short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new svc_booking.PaxSSR();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = svc_booking.MessageState.New;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].StateSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = Pxdr["MealCode_I"].ToString().Trim();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumberSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValue = 0;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValueSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = drIn["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = drIn["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = k;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;
                    }
                }

                objSellSSR.SSRRequest.CurrencyCode = "INR";

                objSellSSR.SSRRequest.CancelFirstSSR = false;
                objSellSSR.SSRRequest.CancelFirstSSR = true;

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
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetRoundWayMeal", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetRoundWayMeal-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);

            return iTotalCost;
        }
        public Decimal GetOneWayMeal(DataTable dtBound, DataTable dtPassenger)
        {
            Decimal iTotalCost = 0;

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

                DataTable dtMealData = new DataTable();
                DataRow[] result;
                if (FltType.Equals("I"))
                {
                    result = dtPassenger.Select("MealChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                }
                else
                {
                    result = dtPassenger.Select("MealChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                }

                if (result.Length > 0)
                {
                    dtMealData = result.CopyToDataTable();
                }

                if (dtMealData.Rows.Count > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_booking.SellRequestData objSellRequestData = new svc_booking.SellRequestData();
                    objSellRequestData.SellBy = svc_booking.SellBy.SSR;
                    objSellRequestData.SellBySpecified = true;

                    svc_booking.SellSSR objSellSSR = new svc_booking.SellSSR();
                    objSellSSR.SSRRequest = new svc_booking.SSRRequest();

                    DataRow dr = dtBound.Rows[0];
                    objSellSSR.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[1];
                    objSellSSR.SSRRequest.SegmentSSRRequests[0] = new svc_booking.SegmentSSRRequest();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new svc_booking.FlightDesignator();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].ArrivalStation = dr["ArrivalStation"].ToString();
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].DepartureStation = dr["DepartureStation"].ToString();

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STD = Convert.ToDateTime(dr["DepDate"].ToString());
                    objSellSSR.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;

                    objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new svc_booking.PaxSSR[dtMealData.Rows.Count];

                    for (short i = 0; i < dtMealData.Rows.Count; i++)
                    {
                        DataRow Pxdr = dtMealData.Rows[i];
                        short k = (short)(int.Parse(Pxdr["Passengerid"].ToString()) - 1);
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new svc_booking.PaxSSR();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = svc_booking.MessageState.New;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].StateSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = Pxdr["MealCode_O"].ToString().Trim();
                        if (FltType.Equals("I"))
                        {
                            objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = Pxdr["MealCode_I"].ToString().Trim();
                        }

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = i;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumberSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValue = 0;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValueSpecified = true;

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();

                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = k;
                        objSellSSR.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetOneWayMeal", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetOneWayMeal-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return iTotalCost;
        }
    }
}
