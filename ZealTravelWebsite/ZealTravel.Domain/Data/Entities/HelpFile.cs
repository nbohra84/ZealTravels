using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HelpFile
{
    public int Id { get; set; }

    public string Pagename { get; set; } = null!;

    public string? Heading { get; set; }

    public int? Pageid { get; set; }

    public DateTime? Eventtime { get; set; }
}
