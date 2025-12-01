using System;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.AgencyManagement;
using DocumentFormat.OpenXml.Bibliography;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using ZealTravel.Domain.Interfaces.Spicejet;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    public class AirHoldBookingPaymentService : IAirHoldBookingPaymentService
    {
        private readonly ICompanyFlightDetailAirlinesService _companyFlightDetailAirlinesService;
        private readonly ICompanyFareDetailAirlineService _companyFareDetailAirlineService;
        private readonly IUAPIServices _uapiService;
        private readonly IAgencyService _agencyService;
        private readonly ISpicejetService _spicejetService;

        public AirHoldBookingPaymentService(
            ICompanyFlightDetailAirlinesService companyFlightDetailAirlinesService,
            ICompanyFareDetailAirlineService companyFareDetailAirlineService,
            IUAPIServices uapiService,
            IAgencyService agencyService,
            ISpicejetService spicejetService)
        {
            _companyFlightDetailAirlinesService = companyFlightDetailAirlinesService;
            _uapiService = uapiService;
            _agencyService = agencyService;
            _spicejetService = spicejetService;
            _companyFareDetailAirlineService = companyFareDetailAirlineService;
        }

        public async Task<string> SetHoldBookingBookingPaymentAsync(string bookingRef, string remarks, string paymentType, string SearchID)
        {
            if (!int.TryParse(bookingRef, out var bookingRefInt))
                throw new Exception("Invalid Booking Reference.");

            var booking = await _companyFlightDetailAirlinesService.FindAsync(x => x.BookingRef == bookingRefInt);
            if (booking == null)
                throw new Exception("Booking not found.");

            decimal bookingTotal = GetBookingTotal(booking);
            var isAvailableBalance = await _agencyService.VerifyTicketBalance(booking.CompanyId, bookingTotal);
            if (!isAvailableBalance)
                throw new Exception("Agency Doesn't have Enough Balance.");

            string depCode = booking.UniversalLocatorCodeD;
            string arrCode = booking.UniversalLocatorCodeA;
            string supplierD = booking.SupplierIdD;
            string supplierA = booking.SupplierIdA;
            string airPNRD = booking.AirlinePnrD;
            string airPNRA = booking.AirlinePnrA;
            string carrierCodeD = booking.CarrierCodeD;
            string carrierCodeA = booking.CarrierCodeA;
            int Paxcount = (int)booking.Adt + (int)booking.Chd;
            bool isRoundTrip = booking.Trip == "R" && booking.Sector == "D";


            
            decimal spiceJetBookingTotalFareD = 0;
            decimal spiceJetBookingTotalFareA = 0;
            if (carrierCodeD == "SG" || carrierCodeA == "SG")
            {
                // Outbound fare
                var fareD = await _companyFareDetailAirlineService.FindAsync(x => x.BookingRef == bookingRefInt && x.Conn == "O") ?? throw new Exception("SpiceJet outbound fare not found");
                spiceJetBookingTotalFareD = fareD.TotalFare.GetValueOrDefault();

                // Inbound fare only for round trips
                if (isRoundTrip)
                {
                    var fareA = await _companyFareDetailAirlineService.FindAsync(x => x.BookingRef == bookingRefInt && x.Conn == "I") ?? throw new Exception("SpiceJet inbound fare not found");
                    spiceJetBookingTotalFareA = fareA.TotalFare.GetValueOrDefault();
                }
            }
           

            if (carrierCodeD == "6E" || carrierCodeA == "6E" || carrierCodeD == "AI" || carrierCodeA == "AI")
            {
                // Validate universal locators
                if (string.IsNullOrWhiteSpace(depCode) || (isRoundTrip && string.IsNullOrWhiteSpace(arrCode)))
                {
                    var msg = isRoundTrip
                    ? "Both Universal Locator Codes (Departure & Arrival) are required for round trips."
                    : "Universal Locator Code (Departure) is required for one-way trips.";
                    throw new Exception(msg);
                }

            }

            if (carrierCodeD == "SG" || carrierCodeA == "SG" )
            {
                // Validate universal locators
                if (string.IsNullOrWhiteSpace(airPNRD) || (isRoundTrip && string.IsNullOrWhiteSpace(airPNRA)))
                {
                    var msg = isRoundTrip
                    ? "Both PNR Codes (Departure & Arrival) are required for round trips."
                    : "PNR Code (Departure) is required for one-way trips.";
                    throw new Exception(msg);

                }
            }

            // Outbound logic
            bool outSuccess;
            bool balanceUpdate;
            bool outDBUpdate;

            if (supplierD == "P3822701" || supplierD == "P7151745")
            {
                // GDS outbound
                const string supplierType = "GDS";
                outSuccess = await _uapiService.GetAirCommitForHoldBookingAsync(depCode, supplierD, supplierType, SearchID, bookingRefInt, booking.CompanyId,"O");

            }
            else if (supplierD == "IGS2528")
            {
                // LCC outbound
                const string supplierType = "LCC";
                outSuccess = await _uapiService.GetAirCommit6EForHoldBookingAsync(depCode, supplierD, supplierType, SearchID, bookingRefInt, booking.CompanyId,"O");
            }
            else if (supplierD == "MAAXT98402" || supplierD == "CPNMAA0030" || supplierD == "MAAXTA8402" || supplierD == "APITESTID")
            {
                outSuccess = await _spicejetService.GetAirCommitForHoldBookingAsync(supplierD, "NetworkPassword", SearchID, bookingRefInt, booking.CompanyId, spiceJetBookingTotalFareD, airPNRD, Paxcount,"O");

            }
            else
            {
                throw new Exception("Unsupported supplier for hold operation.");
            }

            if (outSuccess)
            {
                balanceUpdate = await _agencyService.SetGetCompanyAmountTransaction(SearchID, "SearchCriteria", booking.CompanyId, (decimal)booking.TotalFare!, "D", bookingRefInt, booking.StaffId, "Book-AIR-PAYMENTHOLD_O");

                outDBUpdate = await _agencyService.SetTransactionDetail(SearchID, "SearchCriteria", booking.CompanyId, bookingRefInt, (decimal)booking.TotalFare!, 0, paymentType, "", booking.StaffId, "Book-AIR-PAYMENTHOLD_O", true, false);
            }

            // Inbound logic for round-trip
            if (isRoundTrip)
            {
                bool inSuccess;
                if (supplierA == "P3822701" || supplierA == "P7151745")
                {
                    // GDS inbound
                    const string supplierType = "GDS";
                    inSuccess = await _uapiService.GetAirCommitForHoldBookingAsync(arrCode, supplierA, supplierType, SearchID, bookingRefInt, booking.CompanyId,"I");
                }
                else if (supplierA == "IGS2528")
                {
                    // LCC inbound
                    const string supplierType = "LCC";
                    inSuccess = await _uapiService.GetAirCommit6EForHoldBookingAsync(arrCode, supplierA, supplierType, SearchID, bookingRefInt, booking.CompanyId,"I");
                }
                else if (supplierA == "MAAXT98402" || supplierA == "CPNMAA0030" || supplierA == "MAAXTA8402" || supplierA == "APITESTID")
                {
                    inSuccess = await _spicejetService.GetAirCommitForHoldBookingAsync(supplierA, "NetworkPassword", SearchID, bookingRefInt, booking.CompanyId, spiceJetBookingTotalFareA, airPNRA, Paxcount,"I");
                }
                else
                {
                    throw new Exception("Unsupported inbound supplier for hold operation.");
                }

                if (inSuccess)
                {
                    balanceUpdate = await _agencyService.SetGetCompanyAmountTransaction(SearchID, "SearchCriteria", booking.CompanyId, (decimal)booking.TotalFare!, "D", bookingRefInt, booking.StaffId, "Book-AIR-PAYMENTHOLD_I");

                    outDBUpdate = await _agencyService.SetTransactionDetail(SearchID, "SearchCriteria", booking.CompanyId, bookingRefInt, (decimal)booking.TotalFare!, 0, paymentType, "", booking.StaffId, "Book-AIR-PAYMENTHOLD_I", true, false);
                }

                // Round-trip result
                if (outSuccess && inSuccess)
                    return "Payment has been successfully processed for this ticket.";
                throw new Exception("Failed to send Payment Hold.");
            }

            // One-way result
            if (outSuccess)
                return "Payment has been successfully processed for this ticket.";
            throw new Exception("Failed to send Payment Hold.");

        }

        /// <summary>
        /// Calculates the booking total as TotalFare - TotalCommission + TotalBaggage + TotalSeat + TotalMeal.
        /// </summary>
        private decimal GetBookingTotal(CompanyFlightDetailAirline booking)
        {
            decimal fare = booking.TotalFare.GetValueOrDefault();
            decimal commission = booking.TotalCommission.GetValueOrDefault();
            decimal baggage = booking.TotalBaggage.GetValueOrDefault();
            decimal seat = booking.TotalSeat.GetValueOrDefault();
            decimal meal = booking.TotalMeal.GetValueOrDefault();

            return fare - commission + baggage + seat + meal;
        }
    }
}
