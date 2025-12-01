using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelCountryList
{
    public int Id { get; set; }

    public string CountryCode { get; set; } = null!;

    public string? CountryName { get; set; }
}
