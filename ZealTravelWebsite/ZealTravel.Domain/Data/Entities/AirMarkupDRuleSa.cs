using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirMarkupDRuleSa
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public string? CarrierCode { get; set; }

    public int? NormalMarkup { get; set; }

    public int? SpecialMarkup { get; set; }
}
