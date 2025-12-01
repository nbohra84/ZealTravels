using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursInvoiceDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public string? CommVatCommissiontype { get; set; }

    public decimal? CommVatCommission { get; set; }

    public string? CommVatVattype { get; set; }

    public decimal? CommVatVat { get; set; }

    public string? Exclusions { get; set; }

    public string? Inclusions { get; set; }

    public string? Locations { get; set; }

    public string? Policy { get; set; }

    public string? Tnc { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public string? ContactMobile { get; set; }

    public string? ContactAddress { get; set; }

    public string? ContactWebsite { get; set; }

    public string? TourDescription { get; set; }

    public DateTime? EventTime { get; set; }
}
