using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class UserType
{
    public int Id { get; set; }

    public string? UserType1 { get; set; }

    public string? CompanyId { get; set; }

    public string? UserDescription { get; set; }
}
