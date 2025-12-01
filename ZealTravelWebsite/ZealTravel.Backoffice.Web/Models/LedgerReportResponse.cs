namespace ZealTravel.Backoffice.Web.Models
{
    public class LedgerReportResponse
    {

        public int Sno { get; set; }
        public string CompanyName { get; set; }
        public string ReferenceNo { get; set; }
        public string Cnote { get; set; }
        public DateTime Createdon { get; set; }
        public string? PaymentType { get; set; }
        public decimal Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? AvailableBalance { get; set; }
        public string? Pnr { get; set; }
        public string? Remark { get; set; }
        public string By { get; set; }
    }
}
