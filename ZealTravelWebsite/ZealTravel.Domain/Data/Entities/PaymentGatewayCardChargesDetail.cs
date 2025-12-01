using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PaymentGatewayCardChargesDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? MerchantCode { get; set; }

    public string? CardType { get; set; }

    public decimal? Charges { get; set; }

    public bool? Fixed { get; set; }

    public bool? Percnt { get; set; }

    public bool? Status { get; set; }

    public bool? B2c { get; set; }

    public bool? B2b { get; set; }

    public bool? D2b { get; set; }

    public bool? B2b2b { get; set; }

    public bool? B2b2c { get; set; }
}
