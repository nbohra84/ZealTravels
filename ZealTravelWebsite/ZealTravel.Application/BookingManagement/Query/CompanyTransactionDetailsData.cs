using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class CompanyTransactionDetailsData
    {
        public string? CompanyId { get; set; }
        public int? BookingRef { get; set; }
        public int? PaxSegmentId { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentId { get; set; }
        public string? Remark { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? Status { get; set; }
        public string? SurchargeAmount { get; set; }
        public string? TransactionAmount { get; set; }

    }
}
