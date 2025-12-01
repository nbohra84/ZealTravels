using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierProductStatus
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? SupplierCode { get; set; }

    public string? Pcc { get; set; }

    public string? Product { get; set; }

    public bool? B2b { get; set; }

    public bool? B2c { get; set; }

    public bool? Rt { get; set; }

    public bool? Int { get; set; }

    public bool? MultiCity { get; set; }

    public bool? ImportPnr { get; set; }

    public bool? Pnr { get; set; }

    public bool? Ticketting { get; set; }
}
