using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SpicejetSsr
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Ssrtype { get; set; }

    public string? Sector { get; set; }

    public string? Description { get; set; }

    public bool? Is48Hour { get; set; }

    public bool? Is24Hour { get; set; }

    public bool? Is6Hour { get; set; }

    public bool? IsBoeing { get; set; }

    public bool? IsQ400 { get; set; }

    public bool? T03011130 { get; set; }

    public bool? T11311500 { get; set; }

    public bool? T15011900 { get; set; }

    public bool? T19012300 { get; set; }

    public bool? T23010300 { get; set; }

    public int? Amount { get; set; }
}
