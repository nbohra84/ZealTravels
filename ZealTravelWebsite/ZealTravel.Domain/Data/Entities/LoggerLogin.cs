using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LoggerLogin
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Pwd { get; set; }

    public DateTime? InEvent { get; set; }

    public DateTime? OutEvent { get; set; }

    public string? Ip { get; set; }

    public string? Host { get; set; }

    public bool? LoginStatus { get; set; }
}
