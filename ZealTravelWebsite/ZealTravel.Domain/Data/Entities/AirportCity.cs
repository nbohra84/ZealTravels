using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirportCity
{
    public string? AirportName { get; set; }

    public string? AirportCode { get; set; }

    public string? CityName { get; set; }

    public string? AltCityName { get; set; }

    public string? CityCode { get; set; }

    public string? CountryName { get; set; }

    public string? CountryCode { get; set; }

    public string? Nationality { get; set; }

    public string? Currency { get; set; }

    public bool? Orderlist { get; set; }
}
