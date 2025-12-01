using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgQueryReply
{
    public int Id { get; set; }

    public int? QueryRef { get; set; }

    public int? ReplyRef { get; set; }

    public string? Reply { get; set; }

    public int? Status { get; set; }

    public string? WorkingOn { get; set; }

    public string? StaffId { get; set; }

    public string? Vendor { get; set; }

    public DateTime? EventTime { get; set; }
}
