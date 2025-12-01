using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgPayment
{
    public int Id { get; set; }

    public int? QueryRef { get; set; }

    public string? PaymentBank { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? PaymentType { get; set; }

    public string? PaymentDocument { get; set; }

    public string? PaymentRemark { get; set; }

    public string? PaymentRef { get; set; }

    public string? StaffId { get; set; }

    public DateTime? EventTime { get; set; }
}
