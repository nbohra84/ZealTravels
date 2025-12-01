using System;
using System.Threading.Tasks;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.UniversalAPI.Spicejet;
using ZealTravel.Infrastructure.DBCommon;

namespace ZealTravel.Infrastructure.Spicejet
{
    public class GetConfirmForHoldBooking : IGetConfirmForHoldBooking
    {
        private readonly IBookingManagementService _bookingService;

        public GetConfirmForHoldBooking(IBookingManagementService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<bool> GetConfirmForHoldBookingResponse(string supplierId,string networkPassword,string searchID,int bookingRef,string companyID,decimal totalApiFare,string recordLocator,int paxCount, string FltType)
        {
            var getLogin = new GetApiLogin();
            var signature = getLogin.GetSignature(searchID, supplierId, networkPassword);
            string PassengerRequest = string.Empty;
            try
            {
                // 1) Fetch the “hold” booking
                var getBooking = new GetApiBookings(searchID, companyID, bookingRef);
                var bookingData = getBooking.GetBookingDataBySignature(supplierId, signature, recordLocator);

                // 2) Pre-payment validation
                CommonSpiceJet.RetrieveSpiceJetBookingIdentifiers(bookingData,out var status,out var balanceDue,out var journeysCount);

                var isValidHold = status.Equals("Hold", StringComparison.OrdinalIgnoreCase) && balanceDue > 0 && journeysCount > 0;

                if (!isValidHold)
                    return false;

                // 3) Attempt payment
                var getAddPayment = new GetApiAddPayment(searchID, companyID, bookingRef);
                var paymentSucceeded = getAddPayment.GetPaymentStatus(supplierId, signature, totalApiFare);
                if (!paymentSucceeded)
                    return false;

                // 4) Commit the hold
                var getCommit = new GetApiHoldBookingCommitRequest();
                var commitResponse = getCommit.GetCommit(supplierId, signature, paxCount, recordLocator);

                if (string.IsNullOrEmpty(commitResponse))
                    return false;

                // 5) Fetch the post-payment booking
                var afterPaymentBooking = new GetApiBookings(searchID, companyID, bookingRef);
                var afterPaymentData = afterPaymentBooking.GetBookingDataBySignature(supplierId, signature, recordLocator);

                // 6) Post-payment validation
                CommonSpiceJet.RetrieveSpiceJetPostPaymentIdentifiers(afterPaymentData,out var postStatus,out var postBalanceDue,out var postJourneysCount,out var postPaymentsCount);

                var isFullyPaid = postStatus.Equals("Confirmed", StringComparison.OrdinalIgnoreCase) && postBalanceDue == 0m && postJourneysCount > 0 && postPaymentsCount > 0;

                DBCommon.Logger.dbLoggAPI(searchID, companyID, bookingRef, FltType, "SearchCriteria", "PAYMENTFORHOLDBOOKING", bookingData, paymentSucceeded.ToString(), PassengerRequest);

                return isFullyPaid;
            }
            finally
            {
                // always clean up the session
                getLogin.OffSignature(searchID, signature);
            }
        }
    }
}
