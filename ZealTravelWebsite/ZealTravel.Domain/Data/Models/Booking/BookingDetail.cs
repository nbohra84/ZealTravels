using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class BookingDetail
    {
        public List<CompanyFlightDetailAirlines> CompanyFlightDetailAirlines { get; set; }
        public List<CompanyFlightSegmentDetailAirlines> CompanyFlightSegmentDetailAirlines { get; set; }
        public List<CompanyFareDetailAirlines> CompanyFareDetailAirlines { get; set; }
        public List<CompanyFareDetailSegmentAirlines> CompanyFareDetailSegmentAirlines { get; set; }
        public List<CompanyPaxDetailAirlines> CompanyPaxDetailAirlines { get; set; }
        public List<CompanyPaxSegmentDetailAirlines> CompanyPaxSegmentDetailAirlines { get; set; }
        public List<CompanyFlightDetails> CompanyFlightDetails { get; set; }
        public List<CompanyFlightSegmentRuleDetailAirlines> CompanyFlightSegmentRuleDetailAirlines { get; set; }
        public List<CompanyFlightGSTDetails> CompanyFlightGSTDetails { get; set; }
        public List<CompanyTransactionDetails> CompanyTransactionDetails { get; set; }
        public List<FlightOwnSegmentsPnrs> FlightOwnSegmentsPnrs { get; set; }
        public List<CompanyFlightOwnSegmentsPnrs> CompanyFlightOwnSegmentsPnrs { get; set; }
        public List<PaymentGatewayLoggers> PaymentGatewayLoggers { get; set; }
        public List<BookingAirlineLogForPGs> BookingAirlineLogForPGs { get; set; }
        //public int BookingRef { get; set; }
    }
}
