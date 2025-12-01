using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelCommissionSa
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public decimal? Commission { get; set; }
}
