using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgTheme
{
    public int Id { get; set; }

    public int? Themesid { get; set; }

    public string Themes { get; set; } = null!;
}
