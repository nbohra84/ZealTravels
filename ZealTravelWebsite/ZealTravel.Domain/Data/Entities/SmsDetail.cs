using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SmsDetail
{
    public int Id { get; set; }

    public string? SmsUrl { get; set; }

    public string? BalanceUrl { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? SourceId { get; set; }
}
