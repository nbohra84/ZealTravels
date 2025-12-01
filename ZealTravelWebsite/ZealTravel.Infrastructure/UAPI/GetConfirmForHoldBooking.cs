using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Configuration;
using RtfPipe.Tokens;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Infrastructure.Spicejet;

namespace ZealTravel.Infrastructure.UAPI
{
    public class GetConfirmForHoldBooking : IGetConfirmForHoldBooking
    {
        private IBookingManagementService _bookingService;
        private readonly IConfiguration _config;
        public GetConfirmForHoldBooking(IBookingManagementService bookingService, IConfiguration config)
        {
            _bookingService = bookingService;
            _config = config;
        }

        public async Task<bool> GetConfirmForHoldBookingResponse(string universalRecordLocatorCode,string targetBranch,string NetworkUserName,string NetworkPassword,string SearchID,int BookingRef,string CompanyID, string fltType)
        {
            bool returnstatus = false;
            var objApiBookings = new GetApiBookings();
            var response = objApiBookings.GetBookingDataByRecordLocator(universalRecordLocatorCode,targetBranch,NetworkUserName,NetworkPassword,SearchID);

            if (string.IsNullOrWhiteSpace(response))
                return false;

            // 2) Extract before‐payment identifiers from CommonUapi
            CommonUapi.RetrieveBookingIdentifiers(response,out var extractedPNR,out var version,out var providerLocatorCode,out var reservationLocatorCode,out var airPricingInfoKey, out var carrierCode);

            // 3) Do the Hold+Pay
            var setter = new SetPaymentforHoldBooking(_bookingService,_config);
            var holdResponse = await setter.SetUAPIHoldTicketPayment(extractedPNR,targetBranch,NetworkUserName,NetworkPassword,SearchID,providerLocatorCode,reservationLocatorCode,airPricingInfoKey,version, carrierCode,BookingRef,CompanyID,fltType);

            

            // 4) Extract after‐payment identifiers from CommonUapi
            CommonUapi.RetrieveBookingIdentifiersAfterPayment(holdResponse,out var firstCarrier,out var hkSegmentKey,out var supplierLocatorCode,out var airReservationLocatorCode,out var status);
            
            if (string.IsNullOrEmpty(firstCarrier))
                return false;

            // 5) Only ticket if non‐6E and HK segment
            if (string.Equals(status, "HK", StringComparison.OrdinalIgnoreCase))
            {
                var createTkt = new GetAirCreateTicketing(NetworkUserName,NetworkPassword,targetBranch,NetworkUserName,NetworkPassword,SearchID,CompanyID,BookingRef,_bookingService);

                returnstatus = await createTkt.CreateTicket(supplierLocatorCode,supplierLocatorCode,airReservationLocatorCode,firstCarrier);
            }
            else
            {
                returnstatus = true;
            }

            return returnstatus;
        }

        public async Task<bool> Get6EConfirmForHoldBookingResponse(string universalRecordLocatorCode,string targetBranch,string networkUserName,string networkPassword, string UserName, string Password, string searchID,int BookingRef,string CompanyID, string fltType)
        {
            
            var objApiBookings = new GetApiBookings();
            var response = objApiBookings.Get6eBookingDataByRecordLocator(universalRecordLocatorCode,targetBranch,networkUserName,networkPassword,searchID);

            if (string.IsNullOrWhiteSpace(response))
                return false;

            // 2) Extract before‐payment identifiers from CommonUapi
            CommonUapi.RetrieveBookingIdentifiers(response,out var extractedPNR,out var version,out var providerLocatorCode,out var reservationLocatorCode,out var airPricingInfoKey,out var carrierCode);

            // 3) Do the Hold+Pay
            var setter = new SetPaymentforHoldBooking(_bookingService,_config);
            var holdResponse = await setter.Set6eHoldTicketPayment(extractedPNR,targetBranch,networkUserName,networkPassword, UserName, Password, searchID,providerLocatorCode,reservationLocatorCode,airPricingInfoKey,version, carrierCode,BookingRef,CompanyID,fltType);

            

            // 4) Extract after‐payment identifiers from CommonUapi
            CommonUapi.RetrieveBookingIdentifiersAfterPayment(holdResponse,out var firstCarrier,out var hkSegmentKey,out var supplierLocatorCode,out var airReservationLocatorCode,out var status);

            if (string.IsNullOrEmpty(firstCarrier))
                return false;

            

            return true;
        }
    }
}
