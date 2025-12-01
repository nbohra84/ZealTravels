using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HotelPassengerDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaxSegmentId { get; set; }

    public int? RoomNo { get; set; }

    public int? PaxId { get; set; }

    public string? PaxType { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public int? Age { get; set; }

    public string? PassportExpDate { get; set; }

    public string? PassportIssueDate { get; set; }

    public string? PassportNo { get; set; }

    public string? Mobile { get; set; }

    public string? GstcompanyAddress { get; set; }

    public string? GstcompanyContactNumber { get; set; }

    public string? GstcompanyEmail { get; set; }

    public string? GstcompanyName { get; set; }

    public string? Gstnumber { get; set; }
}
