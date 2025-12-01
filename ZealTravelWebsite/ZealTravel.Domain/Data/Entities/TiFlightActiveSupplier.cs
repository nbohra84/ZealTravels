using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class TiFlightActiveSupplier
{
    public int Id { get; set; }

    public int? Productsupplierid { get; set; }

    public string? Code { get; set; }

    public int? Supplierid { get; set; }

    public string? Productcode { get; set; }

    public string? Suppliertimeout { get; set; }

    public string? CompanyNationality { get; set; }

    public bool? Status { get; set; }
}
