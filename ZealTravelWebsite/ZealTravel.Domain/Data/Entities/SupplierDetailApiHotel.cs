using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierDetailApiHotel
{
    public int Id { get; set; }

    public string? SupplierId { get; set; }

    public string? UserId { get; set; }

    public string? Password { get; set; }
}
