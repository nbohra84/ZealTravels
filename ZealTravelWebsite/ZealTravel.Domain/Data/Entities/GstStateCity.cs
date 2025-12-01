using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GstStateCity
{
    public int Id { get; set; }

    public string? StateCode { get; set; }

    public string? City { get; set; }
}
