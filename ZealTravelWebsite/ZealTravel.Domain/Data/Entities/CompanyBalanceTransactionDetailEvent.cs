using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyBalanceTransactionDetailEvent
{
    public int Id { get; set; }

    public string EventId { get; set; } = null!;

    public string? EventName { get; set; }

    public bool? Status { get; set; }

    public string? EventType { get; set; }
}
