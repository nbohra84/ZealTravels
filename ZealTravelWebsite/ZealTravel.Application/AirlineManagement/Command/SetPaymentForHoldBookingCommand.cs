using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AirlineManagement.Command
{
    public class SetPaymentForHoldBookingCommand
    {
        public string BookingRef { get; set; }
        public string Remarks {  get; set; }
        public string PaymentType { get; set; }
        public string SearchId { get; set; }
    }
}
