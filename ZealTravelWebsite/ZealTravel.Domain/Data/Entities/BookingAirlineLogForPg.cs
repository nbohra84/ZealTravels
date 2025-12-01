using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class BookingAirlineLogForPg
{
    public int Id { get; set; }

    public int? PaymentId { get; set; }

    public string? UpdatedBy { get; set; }

    public string? CompanyId { get; set; }

    public int BookingRef { get; set; }

    public string? AvailabilityResponse { get; set; }

    public string? PassengerResponse { get; set; }

    public string? RefIdO { get; set; }

    public string? RefIdI { get; set; }

    public string? SearchId { get; set; }

    public bool? IsCombi { get; set; }

    public bool? IsRt { get; set; }

    public bool? IsMc { get; set; }

    public string? PaymentType { get; set; }

    public string? Currency { get; set; }

    public decimal? CurrencyValue { get; set; }

    public DateTime? EventTime { get; set; }
}
