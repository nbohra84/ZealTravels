using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PaymentGatewayManagement
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? PaymentGatewayName { get; set; }

    public string? MerchantCode { get; set; }

    public string? MerchantKey { get; set; }

    public string? MerchantMid { get; set; }

    public string? PaymentUrl { get; set; }

    public bool? IsFix { get; set; }

    public bool? IsDebit { get; set; }

    public bool? IsCredit { get; set; }

    public bool? IsNetbanking { get; set; }

    public bool? IsUpi { get; set; }
}
