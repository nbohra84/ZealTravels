using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Backoffice.Web.Models
{
    public class AirSheetReports
    {
        public int PaxNo { get; set; } = 0;
        public string Staff { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public int BookingRef { get; set; } = 0;
        public string CarrierCode { get; set; } = string.Empty;
        public string FlightNo { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string RecordLocator { get; set; } = string.Empty;
        public string TicketNo { get; set; } = string.Empty;
        public int Adt { get; set; } = 0; // Adult
        public int Chd { get; set; } = 0; // Child
        public int Inf { get; set; } = 0; // Infant
        public string PassengerType { get; set; } = string.Empty;
        public string Passenger { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public string DOJ { get; set; } = string.Empty; // Date of Journey

        public decimal BasicFare { get; set; } = 0;
        public decimal YQ { get; set; } = 0; // Fuel Surcharge
        public decimal TxnFee { get; set; } = 0; // Transaction Fee
        public decimal Taxes { get; set; } = 0;
        public int Meal { get; set; } = 0;
        public int Baggage { get; set; } = 0;
        public decimal ServiceCharge { get; set; } = 0;
        public decimal TDS { get; set; } = 0; // Tax Deducted at Source
        public decimal Commission { get; set; } = 0;
        public decimal NetPaidByAgent { get; set; } = 0;
        public string Supplier { get; set; } = string.Empty;

        public int Rowid { get; set; } = 0;
        public string IsPartial { get; set; } = string.Empty;
        public string RefundDate { get; set; } = string.Empty;
        public string PaxName { get; set; } = string.Empty;
        public int AccountID { get; set; } = 0;
        public string AgentName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string PnrTktNumber { get; set; } = string.Empty;
        public string TravelDate { get; set; } = string.Empty;
        public string ClientPenalty { get; set; } = string.Empty;
        public string SupplierPenalty { get; set; } = string.Empty;
        public string RefundBy { get; set; } = string.Empty;
        public decimal Amt { get; set; } = 0;
        public string DrNo { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Check { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;
        public string RefundType { get; set; } = string.Empty;
        public decimal Basic { get; set; } = 0;
        public decimal Yq { get; set; } = 0;
        public decimal Gst { get; set; } = 0;
        public decimal ServiceFee { get; set; } = 0;
        public decimal Seat { get; set; } = 0;
        public decimal Tds { get; set; } = 0;
        public decimal OurCharges { get; set; } = 0;
        public decimal AirlineCharges { get; set; } = 0;
        public decimal NetRefundAmount { get; set; } = 0;
        public string? DepartureDate_D { get; set; }
    }
}
