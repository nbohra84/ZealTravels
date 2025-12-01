using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CurrencyDatum
{
    public int Id { get; set; }

    public string? Currency { get; set; }

    public decimal? Value { get; set; }
}
