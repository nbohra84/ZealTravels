using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirOfflineSupplier
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Supplier { get; set; }
}
