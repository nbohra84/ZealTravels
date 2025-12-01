using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaChargeCategory
{
    public int Id { get; set; }

    public string? VisaChargeCategory { get; set; }

    public string? VisaChargeName { get; set; }
}
