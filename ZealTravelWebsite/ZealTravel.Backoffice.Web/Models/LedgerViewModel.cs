using iText.Signatures.Validation.V1.Report;

namespace ZealTravel.Backoffice.Web.Models
{
    public class LedgerViewModel
    {
        public List<LedgerReportListResponse> ReportItems { get; set; } = new List<LedgerReportListResponse>();
    }

}
