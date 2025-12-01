using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgAddPolicy
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? Payment { get; set; }

    public string? Cancellation { get; set; }

    public string? DestinationInfo { get; set; }

    public string? GroupDepartureInfo { get; set; }
}
