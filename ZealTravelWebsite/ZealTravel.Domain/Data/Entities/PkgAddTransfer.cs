using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgAddTransfer
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public int? Day { get; set; }

    public int? Segmentno { get; set; }

    public string? TransferType { get; set; }

    public string? Origin { get; set; }

    public string? Destiation { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? Class { get; set; }

    public string? Type { get; set; }

    public string? Custom { get; set; }
}
