using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgAddKeyword
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? Keywords { get; set; }
}
