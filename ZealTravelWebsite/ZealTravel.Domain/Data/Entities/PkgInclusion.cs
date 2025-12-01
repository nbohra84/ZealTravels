using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgInclusion
{
    public int Id { get; set; }

    public int? Inclusionid { get; set; }

    public string Inclusions { get; set; } = null!;
}
