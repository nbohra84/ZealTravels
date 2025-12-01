using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models
{
    public class LedgerReport
    {
        public int? PaxSegmentID { get; set; }
        public bool? IsAirlineOff { get; set; }
        public string? CompanyID { get; set; }
        public int? BookingRef { get; set; }
        public decimal? Balance { get; set; }
        public string? PaymentID { get; set; }
        public DateTime? EventTime { get; set; }
        public string? Trip { get; set; }
        public string? Airline_PNR_D { get; set; }
        public string? Airline_PNR_A { get; set; }
        public string? BookingStatus { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string? PaymentType { get; set; }
        public string? Remark { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CompanyName1 { get; set; }
        public string? CompanyName { get; set; }
        public int? AccountID { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Pan_No { get; set; }
        public string? UserType { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        public string? CPaymentType { get; set; }
    }

}

