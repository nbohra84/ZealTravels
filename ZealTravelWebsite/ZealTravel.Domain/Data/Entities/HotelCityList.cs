using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelCityList
{
    public int Id { get; set; }

    public string? CountryCode { get; set; }

    public string? CityName { get; set; }

    public int? CityId { get; set; }

    public string? CityCode { get; set; }

    public int? IsActive { get; set; }
}
