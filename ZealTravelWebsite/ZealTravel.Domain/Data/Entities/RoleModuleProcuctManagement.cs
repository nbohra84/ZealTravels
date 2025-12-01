using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class RoleModuleProcuctManagement
{
    public int Id { get; set; }

    public int? ModuleId { get; set; }

    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? PageName { get; set; }

    public bool? Status { get; set; }

    public bool? IsB2c { get; set; }
}
