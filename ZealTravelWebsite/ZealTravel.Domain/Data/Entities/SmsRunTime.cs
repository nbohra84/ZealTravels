using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SmsRunTime
{
    public int Id { get; set; }

    public int? Smsid { get; set; }

    public string? CompanyId { get; set; }

    public string? SmsText { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }
}
