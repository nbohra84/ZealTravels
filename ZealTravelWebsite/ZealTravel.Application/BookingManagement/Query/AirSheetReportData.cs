using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models.Booking;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class AirSheetReportData
    {
        public List<CompanyFlightDetailsData> CompanyFlightDetails { get; set; }

        public List<CompanyFlightDetailAirlinesData> CompanyFlightDetailAirlines { get; set; }
        public List<CompanyFlightSegmentDetailAirlinesData> CompanyFlightSegmentDetailAirlines { get; set; }
        public List<CompanyFareDetailAirlinesData> CompanyFareDetailAirlines { get; set; }
        public List<CompanyFareDetailSegmentAirlinesData> CompanyFareDetailSegmentAirlines { get; set; }
        public List<CompanyPaxDetailAirlinesData> CompanyPaxDetailAirlines { get; set; }
        public List<CompanyPaxSegmentDetailAirlinesData> CompanyPaxSegmentDetailAirlines { get; set; }
    }
}
