using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaRule
{
    public int Id { get; set; }

    public int? Visaid { get; set; }

    public string? VisaCategory { get; set; }

    public string? Rules { get; set; }

    public DateTime? Eventtime { get; set; }
}
