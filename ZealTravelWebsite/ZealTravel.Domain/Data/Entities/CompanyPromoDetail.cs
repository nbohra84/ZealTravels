using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyPromoDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Promocode { get; set; }

    public int? PromoDeal { get; set; }

    public bool? PromoStatus { get; set; }

    public DateTime? EventTime { get; set; }
}
