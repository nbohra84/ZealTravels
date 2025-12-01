using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgExclusion
{
    public int Id { get; set; }

    public int? Exclusionid { get; set; }

    public string Exclusions { get; set; } = null!;
}
