using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class QueryGroupFare
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public string? ContactPerson { get; set; }

    public string? CompanyId { get; set; }

    public string? Sector { get; set; }

    public string? Trip { get; set; }

    public int? NoOfPassengers { get; set; }

    public string? DepartureStation { get; set; }

    public string? ArrivalStation { get; set; }

    public string? DepartureDate { get; set; }

    public string? ArrivalDate { get; set; }

    public string? MobileNo { get; set; }

    public string? Email { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EventTime { get; set; }

    public int? Status { get; set; }

    public string? RemarksResponse { get; set; }

    public DateTime? EventTimeResponse { get; set; }
}
