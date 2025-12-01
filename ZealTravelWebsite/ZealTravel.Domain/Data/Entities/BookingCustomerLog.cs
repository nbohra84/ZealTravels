using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BookingCustomerLog
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public int? PaymentId { get; set; }

    public string? AdminId { get; set; }

    public string? TransactionType { get; set; }

    public string? Host { get; set; }

    public string? Ip { get; set; }

    public DateTime? EventTime { get; set; }
}
