using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SmtpDetail
{
    public int Id { get; set; }

    public string? SmtpType { get; set; }

    public string? SmtpServer { get; set; }

    public string? ServerPort { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool? Authentication { get; set; }
}
