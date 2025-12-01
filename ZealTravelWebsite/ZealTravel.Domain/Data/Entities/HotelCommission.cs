using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelCommission
{
    public int Id { get; set; }

    public decimal? Commission { get; set; }

    public decimal? ServiceCharge { get; set; }

    public bool? PnrStatus { get; set; }
}
