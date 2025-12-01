using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class BookingDetailData
    {
        public List<CompanyFlightDetailAirlinesData> CompanyFlightDetailAirlines { get; set; }
        public List<CompanyFlightSegmentDetailAirlinesData> CompanyFlightSegmentDetailAirlines { get; set; }
        public List<CompanyFareDetailAirlinesData> CompanyFareDetailAirlines { get; set; }
        public List<CompanyFareDetailSegmentAirlinesData> CompanyFareDetailSegmentAirlines { get; set; }
        public List<CompanyPaxDetailAirlinesData> CompanyPaxDetailAirlines { get; set; }
        public List<CompanyPaxSegmentDetailAirlinesData> CompanyPaxSegmentDetailAirlines { get; set; }
        public List<CompanyFlightDetailsData> CompanyFlightDetails { get; set; }
        public List<CompanyFlightSegmentRuleDetailAirlinesData> CompanyFlightSegmentRuleDetailAirlines { get; set; }
        public List<CompanyFlightGSTDetailsData> CompanyFlightGSTDetails { get; set; }
        public List<CompanyTransactionDetailsData> CompanyTransactionDetails { get; set; }
        public List<FlightOwnSegmentsPnrsData> FlightOwnSegmentsPnrs { get; set; }
        public List<CompanyFlightOwnSegmentsPnrsData> CompanyFlightOwnSegmentsPnrs { get; set; }
        public List<PaymentGatewayLoggersData> PaymentGatewayLoggers { get; set; }
        public List<BookingAirlineLogForPGsData> BookingAirlineLogForPGs { get; set; }
    }
}
