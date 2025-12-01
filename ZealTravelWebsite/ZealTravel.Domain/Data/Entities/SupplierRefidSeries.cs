using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierRefidSeries
{
    public int Id { get; set; }

    public string? Supplier { get; set; }

    public string? Description { get; set; }

    public int? Refid { get; set; }

    public int? ToRefid { get; set; }
}
