using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRegisterCorporateUser
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public bool? IsItinerary { get; set; }

    public bool? IsMaker { get; set; }
}
