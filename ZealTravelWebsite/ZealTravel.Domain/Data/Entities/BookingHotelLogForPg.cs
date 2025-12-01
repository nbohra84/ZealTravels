using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BookingHotelLogForPg
{
    public int Id { get; set; }

    public string? SearchId { get; set; }

    public string? CompanyId { get; set; }

    public string? StaffId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaymentId { get; set; }

    public string? PaymentType { get; set; }

    public string? Paxdetail { get; set; }

    public string? Hoteldata { get; set; }

    public string? Hotelinfo { get; set; }

    public string? Hotelblock { get; set; }

    public string? HotelSearchRequest { get; set; }

    public string? ResultIndex { get; set; }

    public string? RoomRef { get; set; }

    public string? Currency { get; set; }

    public decimal? CurrencyValue { get; set; }

    public DateTime? EventTime { get; set; }
}
