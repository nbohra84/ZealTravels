using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgVendor
{
    public int Id { get; set; }

    public string? VendorType { get; set; }

    public string? Destination { get; set; }

    public string? VendorName { get; set; }

    public string? VendorDetail { get; set; }

    public string? VendorEmail { get; set; }

    public string? VendorMobile { get; set; }

    public string? VendorPhone { get; set; }

    public string? VendorAddress { get; set; }
}
