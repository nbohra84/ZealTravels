using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CountryMaster
{
    public int CountryId { get; set; }

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }
}
