using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgStatus
{
    public int Id { get; set; }

    public int? StatusId { get; set; }

    public string? StatusName { get; set; }
}
