using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class StCityAirport
{
    public string? AirportCode { get; set; }

    public string? AirportName { get; set; }

    public string? CityName { get; set; }
}
