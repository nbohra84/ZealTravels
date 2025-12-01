using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.BookingManagement;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetBookingAsHold
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        public string Searchid;
        public string Companyid;
        public Int32 BookingRef;
        private IBookingManagementService _bookingService;
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        public GetBookingAsHold(string Searchid, string Companyid, int BookingRef, IBookingManagementService bookingService)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.BookingRef = BookingRef;
            this._bookingService = bookingService;
        }
        //================================================================================================================================================
        public async Task<bool> GetCommitOneWayAsHold(DataTable dtBound, DataTable dtPassengerInfo, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string RecordLocator = string.Empty;
            string Supplierid = string.Empty;
            string Signature = string.Empty;
            errorMessage = "";
            bool status = false;

            try
            {
                string PriceType = dtBound.Rows[0]["PriceType"].ToString().Trim();
                Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                string FltType = dtBound.Rows[0]["FltType"].ToString().Trim();

                int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                Decimal GetApiTotalFare = 0;

                Decimal GetTotalFare = GetCommonFunctions.CalculateTotalFare(dtBound.Rows[0]);
                Decimal GeMealCharges = GetCommonFunctions.CalculateMealCharges(dtPassengerInfo, FltType);
                Decimal GetBaggageCharges = GetCommonFunctions.CalculateBaggageCharges(dtPassengerInfo, FltType);

                //----------------------------------------------------------------------------------------------------------------------

                GetApiUpdateContacts objBooking = new GetApiUpdateContacts(Searchid, Companyid, BookingRef);
                bool IsGstUpdated = objBooking.GetUpdateContacts(dtBound, dtPassengerInfo, dtCompanyInfo, dtGstInfo);

                //----------------------------------------------------------------------------------------------------------------------

                GetApiSell objSell = new GetApiSell(Searchid);
                GetApiTotalFare = objSell.GetSellOneWay(dtBound);

                if (Convert.ToInt32(dtBound.Rows[0]["Inf"].ToString()) > 0 && GetApiTotalFare > 0)
                {
                    GetApiTotalFare = objSell.GetSellOneWayInfant(dtBound);
                }

                //----------------------------------------------------------------------------------------------------------------------

                if (GeMealCharges > 0)
                {
                    GetApiMeal objGetApiMeal = new GetApiMeal(Searchid, Companyid, BookingRef);
                    GetApiTotalFare = objGetApiMeal.GetOneWayMeal(dtBound, dtPassengerInfo);
                    if (GetApiTotalFare.Equals(0))
                    {
                        errorMessage += "Failed at Meals";
                    }
                }

                //----------------------------------------------------------------------------------------------------------------------

                if (GetBaggageCharges > 0)
                {
                    GetApiBaggage objGetApiBaggage = new GetApiBaggage(Searchid, Companyid, BookingRef);
                    GetApiTotalFare = objGetApiBaggage.GetOneWayBaggage(dtBound, dtPassengerInfo);
                    if (GetApiTotalFare.Equals(0))
                    {
                        errorMessage = " | Failed at Baggages";
                    }
                }

                //----------------------------------------------------------------------------------------------------------------------

                GetApiUpdatePassenger objGetApiUpdatePassenger = new GetApiUpdatePassenger(Searchid, Companyid, BookingRef);
                GetApiTotalFare = objGetApiUpdatePassenger.GetUpdatePassenger(dtBound, dtPassengerInfo);
                if (GetApiTotalFare.Equals(0))
                {
                    errorMessage = " | Failed at Passengers";
                }

                //----------------------------------------------------------------------------------------------------------------------

                GetApiLogin objGetApiLogin = new GetApiLogin();
                if ((GetTotalFare + GeMealCharges + GetBaggageCharges) >= GetApiTotalFare)
                {
                    if (objGetApiLogin.GetAvailableBalance(Searchid, Signature, Supplierid) >= GetApiTotalFare)
                    {
                        string PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);
                        if (PromotionCode != null && PromotionCode.Length > 0)
                        {
                            GetApiApplyPromotionRequest objApplyPromotionRequest = new GetApiApplyPromotionRequest(Searchid, Companyid, BookingRef);
                            GetApiTotalFare = objApplyPromotionRequest.GetApplyPromotionStatus(Supplierid, Signature, PromotionCode);
                        }

                        //GetApiAddPayment objbps = new GetApiAddPayment(Searchid, Companyid, BookingRef);
                        bool PaymentStatus = true; //commited for the hold objbps.GetPaymentStatus(Supplierid, Signature, GetApiTotalFare);

                        //dbCommon.Logger.WriteLogg(Companyid, BookingRef, "GetPaymentStatus-air_spicejet-Hold", "PNR", GetApiTotalFare + Environment.NewLine + "Manually ignore the payment process because Holding journey", Supplierid, Searchid);

                        if (PaymentStatus.Equals(true))
                        {
                            GetApiBookingCommitRequest objBookingCommitRequest = new GetApiBookingCommitRequest(Searchid, Companyid, BookingRef);
                            RecordLocator = objBookingCommitRequest.GetCommit(dtBound);
                        }
                        else
                        {
                            errorMessage = " | Failed at Payment Status";
                        }
                        
                    }
                    else
                    {
                        errorMessage = " | Failed at Payment not available";
                    }
                }
                else
                {
                    errorMessage = " | Failed at Passenger Update ApiFare:" + GetApiTotalFare.ToString() + " and OurFare:" + GetTotalFare.ToString();
                }
                if (RecordLocator.Length > 0)
                {
                    status = await _bookingService.UpdatePNR(BookingRef, "AD-101", FltType, RecordLocator, RecordLocator, Supplierid);
                    //dbCommon.dbCallCenter.UpdatePNR(BookingRef, "AD-101", FltType, RecordLocator, RecordLocator, Supplierid);
                }

                //objGetApiLogin.OffSignature(Searchid, Signature, ContractVersion);

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(Companyid, BookingRef, "GetHoldOneWay", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            return status;
        }
        public async Task<bool> GetCommitRTAsHold(DataTable dtOutbound, DataTable dtInbound, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo)
        {
            string RecordLocator = string.Empty;
            string Supplierid = string.Empty;
            string Signature = string.Empty;
            bool status = false;

            try
            {
                string PriceType = dtOutbound.Rows[0]["PriceType"].ToString().Trim();
                Supplierid = dtOutbound.Rows[0]["AirlineID"].ToString().Trim();
                Signature = dtOutbound.Rows[0]["Api_SessionID"].ToString().Trim();

                int Adt = int.Parse(dtOutbound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtOutbound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtOutbound.Rows[0]["Inf"].ToString());
                int iPaxCount = Chd + Adt;

                Decimal GetApiTotalFare = 0;
                Decimal GetTotalFare = GetCommonFunctions.CalculateTotalFare(dtOutbound.Rows[0]);

                Decimal GeMealCharges = GetCommonFunctions.CalculateMealCharges(dtPassenger, "O");
                GeMealCharges += GetCommonFunctions.CalculateMealCharges(dtPassenger, "I");

                Decimal GetBaggageCharges = GetCommonFunctions.CalculateBaggageCharges(dtPassenger, "O");
                GetBaggageCharges += GetCommonFunctions.CalculateBaggageCharges(dtPassenger, "I");


                //----------------------------------------------------------------------------------------------------------------------

                GetApiUpdateContacts objBooking = new GetApiUpdateContacts(Searchid, Companyid, BookingRef);
                bool IsGstUpdated = objBooking.GetUpdateContacts(dtOutbound, dtPassenger, dtCompanyInfo, dtGstInfo);

                //----------------------------------------------------------------------------------------------------------------------

                GetApiSell objSell = new GetApiSell(Searchid);
                GetApiTotalFare = objSell.GetSellRoundWay(dtOutbound, dtInbound);

                if (Convert.ToInt32(dtOutbound.Rows[0]["Inf"].ToString()) > 0 && GetApiTotalFare > 0)
                {
                    GetApiTotalFare = objSell.GetSellRoundWayInfant(dtOutbound, dtInbound);
                }

                //----------------------------------------------------------------------------------------------------------------------

                if (GeMealCharges > 0)
                {
                    GetApiMeal objGetApiMeal = new GetApiMeal(Searchid, Companyid, BookingRef);
                    GetApiTotalFare = objGetApiMeal.GetRoundWayMeal(dtOutbound, dtInbound, dtPassenger);
                    if (GetApiTotalFare.Equals(0))
                    {
                        errorMessage = "Failed at Meals";
                    }
                }

                //----------------------------------------------------------------------------------------------------------------------

                if (GetBaggageCharges > 0)
                {
                    GetApiBaggage objGetApiBaggage = new GetApiBaggage(Searchid, Companyid, BookingRef);
                    GetApiTotalFare = objGetApiBaggage.GetRoundWayBaggage(dtOutbound, dtInbound, dtPassenger);
                    if (GetApiTotalFare.Equals(0))
                    {
                        errorMessage = "Failed at Baggages";
                    }
                }

                //----------------------------------------------------------------------------------------------------------------------

                GetApiUpdatePassenger objGetApiUpdatePassenger = new GetApiUpdatePassenger(Searchid, Companyid, BookingRef);
                GetApiTotalFare = objGetApiUpdatePassenger.GetUpdatePassenger(dtOutbound, dtPassenger);

                if (GetApiTotalFare.Equals(0))
                {
                    errorMessage = "Failed at Passengers";
                }

                //----------------------------------------------------------------------------------------------------------------------

                if ((GetTotalFare + GeMealCharges + GetBaggageCharges) >= GetApiTotalFare)
                {
                    GetApiLogin objGetApiLogin = new GetApiLogin();
                    if (objGetApiLogin.GetAvailableBalance(Searchid, Signature, Supplierid) >= GetApiTotalFare)
                    {
                        string PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);
                        if (PromotionCode != null && PromotionCode.Length > 0)
                        {
                            GetApiApplyPromotionRequest objApplyPromotionRequest = new GetApiApplyPromotionRequest(Searchid, Companyid, BookingRef);
                            GetApiTotalFare = objApplyPromotionRequest.GetApplyPromotionStatus(Supplierid, Signature, PromotionCode);
                        }

                        //GetApiAddPayment objbps = new GetApiAddPayment(Searchid, Companyid, BookingRef);
                        bool PaymentStatus = true;    // commited for holding  objbps.GetPaymentStatus(Supplierid, Signature, GetApiTotalFare);
                        if (PaymentStatus.Equals(true))
                        {
                            GetApiBookingCommitRequest objBookingCommitRequest = new GetApiBookingCommitRequest(Searchid, Companyid, BookingRef);
                            RecordLocator = objBookingCommitRequest.GetCommit(dtOutbound);
                        }
                        else
                        {
                            errorMessage = "Failed at Payment Status";
                        }
                    }
                    else
                    {
                        errorMessage = "Failed at Payment not available";
                    }

                    if (RecordLocator.Length > 0)
                    {

                        status = await _bookingService.UpdatePNR(BookingRef, "AD-101", "O", RecordLocator, RecordLocator, Supplierid);

                    }

                    if (RecordLocator.Length > 0)
                    {
                        
                        status = await _bookingService.UpdatePNR(BookingRef, "AD-101", "I", RecordLocator, RecordLocator, Supplierid);
                    }

                    
                    //objGetApiLogin.OffSignature(Searchid, Signature, ContractVersion);
                }
                else
                {
                    errorMessage = "Failed at Fare not matched with api";
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(Companyid, BookingRef, "GetHoldRT", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            return status;
        }
    }
}
