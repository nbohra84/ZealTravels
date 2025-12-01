using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class WhitelabelDetail
{
    public int Id { get; set; }

    public string CompanyId { get; set; } = null!;

    public string? Host { get; set; }

    public bool? B2b { get; set; }

    public bool? B2c { get; set; }

    public string? HostDirectory { get; set; }

    public bool? RunOwnGateway { get; set; }

    public bool? OwnPg { get; set; }

    public bool? AgencyPrintB2c { get; set; }

    public DateTime? EventTime { get; set; }

    public bool? IsWorking { get; set; }
}
