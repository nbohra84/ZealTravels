using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgWorkingOn
{
    public int Id { get; set; }

    public int? WorkingId { get; set; }

    public string? WorkingName { get; set; }
}
