using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BankCodeList
{
    public int Id { get; set; }

    public string? BankName { get; set; }

    public string? BankCode { get; set; }
}
