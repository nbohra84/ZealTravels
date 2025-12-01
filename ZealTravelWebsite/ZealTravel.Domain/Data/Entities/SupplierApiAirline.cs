using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierApiAirline
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? CarrierCode { get; set; }

    public bool? Status { get; set; }
}
