using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SmsTemplate
{
    public int Id { get; set; }

    public string? EventName { get; set; }

    public string? ProductName { get; set; }

    public string? UserType { get; set; }

    public string? SmsMessage { get; set; }

    public bool? Status { get; set; }

    public string? SenderId { get; set; }

    public string? Tempid { get; set; }

    public string? Entityid { get; set; }
}
