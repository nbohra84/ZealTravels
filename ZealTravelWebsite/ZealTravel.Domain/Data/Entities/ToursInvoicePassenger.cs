using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursInvoicePassenger
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? PaxType { get; set; }

    public string? Title { get; set; }

    public string? PassengerName { get; set; }

    public int? Dob { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? PpNumber { get; set; }

    public string? PpIssueDate { get; set; }

    public string? PpExpiraryDate { get; set; }

    public string? Nationality { get; set; }

    public DateTime? EventTime { get; set; }
}
