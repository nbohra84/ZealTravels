using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DmcCountryCityList
{
    public int Id { get; set; }

    public string? CountryName { get; set; }

    public string? CityName { get; set; }
}
