using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class OpinionLog
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? CompanyId { get; set; }

    public int? Rating { get; set; }

    public string? Review { get; set; }

    public string? TitleReview { get; set; }

    public bool? Status { get; set; }

    public DateTime? Eventtime { get; set; }
}
