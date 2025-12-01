using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models
{
    public class BalanceTransactionResponse
    {
        //public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyNameWithAccountId { get; set; }
        public decimal AvailableBalance { get; set; }
        //public List<BalanceTransactionReportResponse> BalanceTransationReports { get; set; }
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Not a valid integer")]
        public string TransactionAmount { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Not a valid integer")]
        public int BookingRef { get; set; }
        public string UpdatedBy { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string EventId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Remark { get; set; }

        public bool IsAirline { get; set; }
        public bool IsHotel { get; set; }
        public int PassengerId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string PaymentType { get; set; }
    }
}
