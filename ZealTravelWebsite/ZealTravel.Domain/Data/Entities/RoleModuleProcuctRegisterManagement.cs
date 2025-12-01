using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class RoleModuleProcuctRegisterManagement
{
    public int Id { get; set; }

    public string? UserType { get; set; }

    public string? ProductId { get; set; }

    public bool? Show { get; set; }

    public bool? Edit { get; set; }
}
