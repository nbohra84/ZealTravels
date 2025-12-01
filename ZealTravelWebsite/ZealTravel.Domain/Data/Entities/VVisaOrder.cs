using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaOrder
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? Orderid { get; set; }

    public string? PassportNo { get; set; }

    public string? PassportName { get; set; }

    public string? PassportAddress { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? LandLineNo { get; set; }

    public string? Gender { get; set; }

    public string? FilePath { get; set; }

    public string? VisaCategory { get; set; }

    public string? VisaChargeCategory { get; set; }

    public string? FromVisaState { get; set; }

    public string? ToVisaCountry { get; set; }

    public string? FromVisaCountry { get; set; }

    public DateTime? Eventtime { get; set; }
}
