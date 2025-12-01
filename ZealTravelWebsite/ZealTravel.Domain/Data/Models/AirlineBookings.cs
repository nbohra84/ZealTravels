using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models
{
    public class AirlineBookings
    {
        public string CompanyID { get; set; }
        public int BookingRef { get; set; }
        public int Pax_SegmentID { get; set; }
        public string Title { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Pax_MobileNo { get; set; }
        public string? Pax_Email { get; set; }
        public string? DOB { get; set; }
        public string? PaxType { get; set; }
        public string? TicketNo { get; set; }
        public string SupplierID_D { get; set; }
        public string SupplierID_A { get; set; }
        public string Sector { get; set; }
        public string Trip { get; set; }
        public string Origin{ get; set; }
        public string Destination{ get; set; }
        public string? CarrierCode_D { get; set; }
        public string? CarrierCode_A { get; set; }
        public string? Airline_PNR_D { get; set; }
        public string? Airline_PNR_A { get; set; }
        public string? DepartureDate_D { get; set; }
        public string? DepartureDate_A { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsRejected { get; set; }
        public bool IsCancelRequested { get; set; }
        public string? CancelDetail { get; set; }
        public decimal TotalFare { get; set; }
        public string? PriceType_D { get; set; }
        public string? PriceType_A { get; set; }
        public decimal? TotalCommission { get; set; }
        public string? BookingStatus { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime EventTime { get; set; }
        public string? CompanyName { get; set; }
        public int AccountID { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Pan_No { get; set; }
        public string UserType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public bool IsPaymentHold_A { get; set; }
        public bool IsPaymentHold_D { get; set; }
        public string IsPaymentHold { get; set; }

    }
}
