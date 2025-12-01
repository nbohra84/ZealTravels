using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class MailTemplateSender
{
    public int Id { get; set; }

    public string MailType { get; set; } = null!;

    public string? Subject { get; set; }

    public string? MailContent { get; set; }

    public string? FromAddress { get; set; }

    public string? CcAddress { get; set; }

    public string? BccAddress { get; set; }

    public bool? Active { get; set; }

    public bool? Header { get; set; }

    public bool? Footer { get; set; }

    public DateTime EventTime { get; set; }
}
