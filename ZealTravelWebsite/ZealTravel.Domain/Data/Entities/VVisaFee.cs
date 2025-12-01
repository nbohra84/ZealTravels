using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaFee
{
    public int Id { get; set; }

    public int? Visaid { get; set; }

    public string? VisaCategory { get; set; }

    public string? VisaChargeCategory { get; set; }

    public decimal? Visafee { get; set; }

    public decimal? Vfsfee { get; set; }

    public decimal? Servicefee { get; set; }

    public string? Remark { get; set; }

    public DateTime? Eventtime { get; set; }
}
