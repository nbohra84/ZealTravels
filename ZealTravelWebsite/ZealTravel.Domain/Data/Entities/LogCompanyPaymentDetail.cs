using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LogCompanyPaymentDetail
{
    public int Id { get; set; }

    public int? PaymentRef { get; set; }

    public string? CompanyId { get; set; }

    public string StaffId { get; set; } = null!;

    public string ReferenceNo { get; set; } = null!;

    public string? PaymentType { get; set; }

    public int? PaymentId { get; set; }

    public int? Amount { get; set; }

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

    public DateTime? UpdateTime { get; set; }

    public string? UpdateType { get; set; }
}
