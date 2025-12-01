using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DealDetail
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public string? CarrierCode { get; set; }

    public string? Iata { get; set; }

    public string? Economy { get; set; }

    public string? Business { get; set; }
}
