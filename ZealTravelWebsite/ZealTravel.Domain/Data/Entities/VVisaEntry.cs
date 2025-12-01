using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaEntry
{
    public int Id { get; set; }

    public int? Visaid { get; set; }

    public string? FromVisaCountry { get; set; }

    public string? FromVisaState { get; set; }

    public string? ToVisaCountry { get; set; }

    public string? VisaCategory { get; set; }

    public DateTime? Eventtime { get; set; }
}
