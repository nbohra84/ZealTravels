using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyTransactionLedgerEvent
{
    public int Id { get; set; }

    public string? EventId { get; set; }

    public string? EventName { get; set; }
}
