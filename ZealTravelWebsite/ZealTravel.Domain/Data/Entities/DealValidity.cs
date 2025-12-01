using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DealValidity
{
    public int Id { get; set; }

    public string? CarrierCode { get; set; }

    public string? Validity { get; set; }

    public string? Remarks { get; set; }
}
