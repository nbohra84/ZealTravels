using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class SupplierDetailGalileoAirline
{
    public int Id { get; set; }

    public string? Hap { get; set; }

    public string? Userid { get; set; }

    public string? Password { get; set; }

    public string? SoapUrl { get; set; }

    public string? TargetBranch { get; set; }

    public string? Pcc { get; set; }

    public int? ImportQueue { get; set; }

    public int? TktdQueue { get; set; }

    public bool? TicketIfFareGaurantee { get; set; }

    public bool? Status { get; set; }
}
