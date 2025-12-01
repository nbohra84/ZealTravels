using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyBalanceTransactionDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public decimal? AvailableBalance { get; set; }

    public decimal? TemporaryBalance { get; set; }

    public DateTime? TemporaryBalanceTime { get; set; }

    public decimal? OutstandingBalance { get; set; }

    public string? EventId { get; set; }

    public bool? CreditUser { get; set; }

    public DateOnly? CreditTime { get; set; }

    public decimal? CreditBalance { get; set; }

    public decimal? CreditAmount { get; set; }

    public DateTime? EventTime { get; set; }
}
