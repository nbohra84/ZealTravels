using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BankDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? BankLogoCode { get; set; }

    public string? BankCode { get; set; }

    public string? BankName { get; set; }

    public string? BranchName { get; set; }

    public string AccountNo { get; set; } = null!;

    public bool? Status { get; set; }

    public bool? B2b { get; set; }

    public bool? D2b { get; set; }

    public bool? B2c { get; set; }

    public bool? B2b2b { get; set; }

    public bool? B2b2c { get; set; }

    public string BankLogo { get; set; } = null!;
}
