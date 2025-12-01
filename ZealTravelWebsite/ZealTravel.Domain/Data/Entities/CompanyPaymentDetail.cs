using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyPaymentDetail
{
    public int Id { get; set; }

    public int? PaymentRef { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? StaffId { get; set; }

    public string? ReferenceNo { get; set; }

    public string? PaymentType { get; set; }

    public int? PaymentId { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Surcharge { get; set; }

    public string? Remark { get; set; }

    public string? BankName { get; set; }

    public string? AccountNo { get; set; }

    public string? ChequeNo { get; set; }

    public DateOnly? IssueDate { get; set; }

    public string? UtrNo { get; set; }

    public bool StatusBegin { get; set; }

    public DateTime? EventTime { get; set; }

    public int StatusEnd { get; set; }

    public string RemarkEnd { get; set; } = null!;

    public string? EntryBy { get; set; }

    public DateTime? EventTimeEnd { get; set; }

    public bool? IsBilled { get; set; }
}
