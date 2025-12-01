using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PaymentGatewayCardName
{
    public int Id { get; set; }

    public string? MerchantCode { get; set; }

    public string? CardType { get; set; }

    public string? CardName { get; set; }

    public decimal? Charges { get; set; }

    public bool? Fixed { get; set; }

    public bool? Percnt { get; set; }
}
