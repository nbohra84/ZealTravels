using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class GstState
{
    public int Id { get; set; }

    public string? State { get; set; }

    public string? GstCode { get; set; }

    public string? StateCode { get; set; }

    public bool? Ut { get; set; }
}
