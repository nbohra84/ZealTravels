using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HelpFileDescription
{
    public int Id { get; set; }

    public int? Pageid { get; set; }

    public int? Orderby { get; set; }

    public bool? IsHeader { get; set; }

    public string? Description { get; set; }

    public DateTime? Eventtime { get; set; }
}
