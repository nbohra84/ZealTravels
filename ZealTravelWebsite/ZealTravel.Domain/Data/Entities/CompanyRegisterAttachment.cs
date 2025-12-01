using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRegisterAttachment
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Pan { get; set; }

    public string? Photo { get; set; }

    public string? Dl { get; set; }

    public string? AadharCard { get; set; }

    public string? Bill { get; set; }

    public string? Passport { get; set; }

    public string? VoterId { get; set; }
}
