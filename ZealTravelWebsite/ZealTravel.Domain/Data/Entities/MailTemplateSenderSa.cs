using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class MailTemplateSenderSa
{
    public int Id { get; set; }

    public string CompanyId { get; set; } = null!;

    public string? Header { get; set; }

    public string? Footer { get; set; }

    public string? CcAddress { get; set; }

    public string? BccAddress { get; set; }

    public string SmsSenderId { get; set; } = null!;
}
