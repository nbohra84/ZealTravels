using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRoleModuleProcuctRegisterOperationManagement
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? ProductId { get; set; }

    public bool? Edit { get; set; }

    public bool? Show { get; set; }

    public bool? ProductStatus { get; set; }
}
