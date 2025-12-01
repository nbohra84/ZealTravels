using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgAddDepartureCity
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? City { get; set; }

    public string? CountryName { get; set; }

    public string? Countrycode { get; set; }
}
