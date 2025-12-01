using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class CompanyFlightSegmentDetailAirlines
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public int? Flight_SegmentId { get; set; }

        public string? CarrierCode { get; set; }

        public string? Airline_Pnr { get; set; }

        public string? Gds_Pnr { get; set; }

        public string? Origin { get; set; }
        public string? sOrigin { get; set; }

        public string? Destination { get; set; }
        public string? sDestination { get; set; }

        public string? DepartureStation { get; set; }
        public string? sDepartureStation { get; set; }

        public string? ArrivalStation { get; set; }
        public string? sArrivalStation { get; set; }

        public string? DepartureDate { get; set; }

        public string? ArrivalDate { get; set; }

        public string? DepartureTime { get; set; }

        public string? ArrivalTime { get; set; }

        public string? FlightNumber { get; set; }

        public string? ClassOfService { get; set; }

        public int? Stops { get; set; }

        public int? Via { get; set; }

        public string? ViaName { get; set; }

        public string? DepartureTerminal { get; set; }
        public string? DepartureStationAirport { get; set; }
        public string? ArrivalStationAirport { get; set; }

        public string? ArrivalTerminal { get; set; }

        public int? JourneyTime { get; set; }

        public int? Duration { get; set; }

        public string? ProductClass { get; set; }

        public string? FareBasisCode { get; set; }

        public string? Cabin { get; set; }

        public string? RefundType { get; set; }

        public string? FareType { get; set; }

        public string? ConnOrder { get; set; }

        public string? BaggageDetail { get; set; }

        public string? FareRule { get; set; }

        public string? RuleTarrif { get; set; }

        public DateTime? EventTime { get; set; }

        public string? CarrierName { get; set; }

        public bool? IsLcc { get; set; }

        public string? Iatacode { get; set; }

        public string? AirlineContact { get; set; }
    }
}
