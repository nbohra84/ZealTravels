using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PaymentGatewayCardChargesWaiverLimit
{
    public int Id { get; set; }

    public string? CardType { get; set; }

    public long? Waiver { get; set; }
}
