using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgPassenger
{
    public int Id { get; set; }

    public int? QueryRef { get; set; }

    public string? PaxType { get; set; }

    public string? PassengerName { get; set; }

    public int? Age { get; set; }
}
