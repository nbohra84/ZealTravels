using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class NoticeCompany
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public string? Link { get; set; }

    public bool? Status { get; set; }

    public DateTime? EventTime { get; set; }

    public int? IPriority { get; set; }
}
