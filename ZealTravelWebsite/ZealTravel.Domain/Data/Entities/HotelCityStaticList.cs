using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelCityStaticList
{
    public int Id { get; set; }

    public string? CountryCode { get; set; }

    public string? CityName { get; set; }

    public string? CityId { get; set; }
}
