using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierDetail
{
    public int Id { get; set; }

    public string? Product { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierId { get; set; }

    public string? UserId { get; set; }

    public string? Password { get; set; }

    public bool? OwnSupplier { get; set; }

    public DateTime EventTime { get; set; }
}
