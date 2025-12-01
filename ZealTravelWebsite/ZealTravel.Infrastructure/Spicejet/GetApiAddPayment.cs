using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.Helpers;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiAddPayment
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------


        public GetApiAddPayment(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public bool GetPaymentStatus(string Supplierid, string Signature, Decimal TotalApiFare)
        {
            bool PaymentStatus = false;
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            if (ConfigurationHelper.GetSetting("ASPNETCORE_ENVIRONMENT").ToLower() != "production")
            {
                Supplierid = "APITESTID";  
            }

                try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.AddPaymentToBookingRequest objAddPaymentToBookingRequest = new svc_booking.AddPaymentToBookingRequest();
                svc_booking.AddPaymentToBookingRequestData objAddPaymentToBookingRequestData = new svc_booking.AddPaymentToBookingRequestData();

                objAddPaymentToBookingRequestData.MessageState = svc_booking.MessageState.New;
                objAddPaymentToBookingRequestData.MessageStateSpecified = true;

                objAddPaymentToBookingRequestData.WaiveFee = false;
                objAddPaymentToBookingRequestData.WaiveFeeSpecified = true;

                objAddPaymentToBookingRequestData.ReferenceType = svc_booking.PaymentReferenceType.Default;
                objAddPaymentToBookingRequestData.ReferenceTypeSpecified = true;

                objAddPaymentToBookingRequestData.PaymentMethodType = svc_booking.RequestPaymentMethodType.AgencyAccount;
                objAddPaymentToBookingRequestData.PaymentMethodTypeSpecified = true;

                objAddPaymentToBookingRequestData.PaymentMethodCode = "AG";

                objAddPaymentToBookingRequestData.QuotedCurrencyCode = "INR";

                objAddPaymentToBookingRequestData.QuotedAmount = TotalApiFare;
                objAddPaymentToBookingRequestData.QuotedAmountSpecified = true;

                objAddPaymentToBookingRequestData.Status = svc_booking.BookingPaymentStatus.New;
                objAddPaymentToBookingRequestData.StatusSpecified = true;

                objAddPaymentToBookingRequestData.AccountNumberID = 0;
                objAddPaymentToBookingRequestData.AccountNumberIDSpecified = true;

                objAddPaymentToBookingRequestData.AccountNumber = Supplierid;
                objAddPaymentToBookingRequestData.AccountNumberIDSpecified = true;

                objAddPaymentToBookingRequestData.Expiration = DateTime.Parse("0001-01-01T00:00:00");
                objAddPaymentToBookingRequestData.ExpirationSpecified = true;

                objAddPaymentToBookingRequestData.ParentPaymentID = 0;
                objAddPaymentToBookingRequestData.ParentPaymentIDSpecified = true;

                objAddPaymentToBookingRequestData.Installments = 0;
                objAddPaymentToBookingRequestData.InstallmentsSpecified = true;

                objAddPaymentToBookingRequestData.Deposit = false;
                objAddPaymentToBookingRequestData.DepositSpecified = true;

                objAddPaymentToBookingRequestData.AgencyAccount = new svc_booking.AgencyAccount();
                objAddPaymentToBookingRequestData.AgencyAccount.AccountID = 0;
                objAddPaymentToBookingRequestData.AgencyAccount.AccountIDSpecified = true;

                objAddPaymentToBookingRequestData.AgencyAccount.AccountTransactionID = 0;
                objAddPaymentToBookingRequestData.AgencyAccount.AccountTransactionIDSpecified = true;

                objAddPaymentToBookingRequest.addPaymentToBookingReqData = objAddPaymentToBookingRequestData;
                objAddPaymentToBookingRequest.ContractVersion = ContractVersion;
                objAddPaymentToBookingRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objAddPaymentToBookingRequest);

                svc_booking.AddPaymentToBookingResponse objAddPaymentToBookingResponse = new svc_booking.AddPaymentToBookingResponse();
                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                objAddPaymentToBookingResponse = objIBookingManager.AddPaymentToBooking(objAddPaymentToBookingRequest);
                PaymentStatus = objAddPaymentToBookingResponse.BookingPaymentResponse.ValidationPayment.Payment.PaymentAddedToState;
                if (PaymentStatus.Equals(false))
                {
                    errorMessage = objAddPaymentToBookingResponse.BookingPaymentResponse.ValidationPayment.PaymentValidationErrors[0].ErrorDescription.ToString();
                }

                GetApiResponse = GetCommonFunctions.Serialize(objAddPaymentToBookingResponse);              
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetPaymentStatus", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetPaymentStatus-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return PaymentStatus;
        }
    }
}
