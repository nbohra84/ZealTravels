using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRegisterCorporateUsersLimit
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Limit { get; set; }
}
