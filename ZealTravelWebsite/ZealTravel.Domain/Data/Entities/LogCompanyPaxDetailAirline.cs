using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LogCompanyPaxDetailAirline
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaxSegmentId { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? TicketNo { get; set; }

    public string? MobileNo { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Ffn { get; set; }

    public string? TourCode { get; set; }

    public string? Dob { get; set; }

    public string? PaxType { get; set; }

    public string? PpNumber { get; set; }

    public string? PpissueDate { get; set; }

    public string? PpexpirayDate { get; set; }

    public string? Nationality { get; set; }

    public DateTime? EventTime { get; set; }

    public string? UpdateType { get; set; }

    public DateTime? UpdateTime { get; set; }
}
