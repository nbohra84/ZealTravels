using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class NoticeStaff
{
    public int Id { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public string? Link { get; set; }

    public int? StaffType { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }

    public int? IPriority { get; set; }
}
