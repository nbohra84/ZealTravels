using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Backoffice.Web.Models.Agency.Booking
{
    public class PrintPopupResponse
    {
        public string? ReportTableHTML { get; set; }
        public string BookingRef { get; set; }
        public string Pax_SegmentId { get; set; }
        public string? SalesTable { get; set; }
        public string? Hdntktdetails { get; set; }
        public string HdnhostName { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public bool ShowLogo { get; set; } = true;
        public bool HideFare { get; set; } = false;
        public bool IsSingleFare { get; set; } = false;
        public string? CompanyID { get; set; }
        public string? PNR { get; set; }
        public string? TicketNumber { get; set; }
        public string? PassengerName { get; set; }
        public string SingleFare { get; set; }
        public string? BookingReference { get; set; }
    }
}
