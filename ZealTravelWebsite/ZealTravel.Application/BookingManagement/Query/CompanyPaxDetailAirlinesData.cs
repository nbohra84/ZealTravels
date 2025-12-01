using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class CompanyPaxDetailAirlinesData
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public int? Pax_SegmentId { get; set; }

        public string? Title { get; set; }

        public string? First_Name { get; set; }

        public string? Middle_Name { get; set; }

        public string? Last_Name { get; set; }

        public string? TicketNo { get; set; }

        public string? MobileNo { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Ffn { get; set; }

        public string? TourCode { get; set; }

        public string? Dob { get; set; }

        public string? PaxType { get; set; }

        public string? PpNumber { get; set; }

        public string? PpissueDate { get; set; }

        public string? PpexpirayDate { get; set; }

        public string? Nationality { get; set; }

        public DateTime? EventTime { get; set; }


        public int? CPaxSegmentId { get; set; }


        public string? CanceledBy { get; set; }

        public string? CancellationType { get; set; }

        public string? CancellationReason { get; set; }

        public string? Remark { get; set; }

        public string? FltType { get; set; }

        public bool? IsPartial { get; set; }

        public string? Origin { get; set; }

        public string? Destination { get; set; }

        public string? Pnr { get; set; }

        public string? CarrierCode { get; set; }
        public string? Outbound { get; set; }
        public string? Inbound { get; set; }

        public bool? Status { get; set; }

        public string? Ip { get; set; }
    }
}
