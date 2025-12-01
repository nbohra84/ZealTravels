using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class RoleModuleManagement
{
    public int Id { get; set; }

    public int? ModuleId { get; set; }

    public string? ModuleName { get; set; }
}
