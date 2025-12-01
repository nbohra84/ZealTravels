using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.BookingManagement
{
    public interface IBookingManagementService
    {

        Task<BookingDetail> GetBooking(Int32 bookingRef);
        Task<BookingDetail> GetAirBooking(Int32 bookingRef);
        Task<List<FlightRefrenceData>> GetBookingRef(DateTime startDate, DateTime endDate);
        Task<bool> GetBookingPnrStatus(int bookingRef);
        Task<bool> GetBookingTicketStatus(int bookingRef);
        Task<bool> IsWalletCfee(string companyID, string bookingType, string sector);
        Task<bool> IsWalletCfee(string CompanyID, string BookingType);
        Task<DataTable> GetCfee(string CompanyID, string BookingType, string Sector);
        Task<Int32> GetBookingRefFlightDetailAirline(string SearchID, string SearchCriteria, string CompanyID, string MakerID, bool IsQueue, bool IsOffline, CompanyFlightFareDetailAirline obj, bool IsWallet_Cfee);
        Task<bool> SetCustomerFareDetailAirline(string CompanyID, Int32 BookingRef, Decimal CompanyDebit, Decimal CustomerDebit, Decimal CompanyCredit, Decimal CustomerCredit, Int32 Markup, Decimal CompanyTds);
        Task<bool> SetFlightSegmentRuleDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataTable dtBound);
        Task<bool> SetFlightSegmentDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataTable dtBound, bool IsCombi, bool IsRTfare);
        Task<bool> SetFareDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, string Conn, Decimal TotalMeal, Decimal TotalBaggage, Decimal TotalSeat, CompanyFlightFareDetailAirline obj, bool IsSingleFare);
        Task<bool> SetFareDetailSegmentAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, string Conn, string PaxType, Decimal dMeal, Decimal dSeat, Decimal dBaggage, DataRow drSelect, bool IsSingleFare);
        Task<bool> SetPaxDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataTable dtPassenger);
        Task<bool> BookingStatus(Int32 BookingRef, bool Status);
        Task<bool> SetBookingAirlineLogForPG(bool IsCombi, bool IsRT, bool IsMC, string SearchID, string CompanyID, Int32 BookingRef, string AvailabilityResponse, string PassengerResponse, string RefID_O, string RefID_I, int PaymentID, string PaymentType, string Currency, double CurrencyValue);
        Task<bool> UpdatePNR(Int32 BookingRef, string StaffID, string FltType, string Airline_PNR, string GDS_PNR, string SupplierID, string? UniversalRecordLocatorCode = "");
        Task<bool> UpdateTicketNumber(Int32 bookingRef, ArrayList ticketNumbers, bool isModify);
        Task<bool> IsTicketAutoMode(string Companyid, string CarrierCode, string Sector, int BookingRef);
        Task<string> GetActiveFormOfPayment();
        Task<DataRow> GetCCDetails(string CarrierCode);
    }

}
