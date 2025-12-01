using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyBalanceTransactionHistoryDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? TransType { get; set; }

    public decimal? TransactionAmount { get; set; }

    public decimal? BeforeTransactionAvailableBalance { get; set; }

    public decimal? AfterTransactionAvailableBalance { get; set; }

    public decimal? BeforeTransactionTemporaryBalance { get; set; }

    public decimal? AfterTransactionTemporaryBalance { get; set; }

    public decimal? OutStandingBalance { get; set; }

    public string? UpdatedBy { get; set; }

    public string? Remark { get; set; }

    public string? EventId { get; set; }

    public DateTime EventTime { get; set; }
}
