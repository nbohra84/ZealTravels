using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LoggerSearch
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? StaffId { get; set; }

    public string? SearchId { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public string? Place { get; set; }

    public string? Remark { get; set; }

    public string? Remark2 { get; set; }

    public string? Host { get; set; }

    public DateTime? EventTime { get; set; }
}
