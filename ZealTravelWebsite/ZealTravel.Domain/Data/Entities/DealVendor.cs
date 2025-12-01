using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DealVendor
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public string? VendorName { get; set; }

    public string? VendorDetail { get; set; }
}
