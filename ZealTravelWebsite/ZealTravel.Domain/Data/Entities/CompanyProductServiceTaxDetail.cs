using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyProductServiceTaxDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public decimal? FlightServiceTax { get; set; }

    public decimal? HotelServiceTax { get; set; }
}
