using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HtmlFilght
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Source { get; set; }

    public string? Destination { get; set; }

    public string? Sector { get; set; }
}
