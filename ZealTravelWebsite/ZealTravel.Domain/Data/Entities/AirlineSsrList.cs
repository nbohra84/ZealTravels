using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlineSsrList
{
    public int Id { get; set; }

    public string? Sector { get; set; }

    public string? CarrierCode { get; set; }

    public string? ProductClass { get; set; }

    public string? SsrType { get; set; }

    public string? SsrCode { get; set; }

    public string? Description { get; set; }

    public string? AdditionalRule { get; set; }

    public int? Amount { get; set; }

    public string? Refundable { get; set; }

    public string? FeeCode { get; set; }
}
