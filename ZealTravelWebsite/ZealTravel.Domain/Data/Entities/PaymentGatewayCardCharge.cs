using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PaymentGatewayCardCharge
{
    public int Id { get; set; }

    public string? CardType { get; set; }

    public int? StartAmount { get; set; }

    public int? EndAmount { get; set; }

    public decimal? Charges { get; set; }
}
