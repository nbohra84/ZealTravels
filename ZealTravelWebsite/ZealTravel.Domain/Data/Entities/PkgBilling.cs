using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgBilling
{
    public int Id { get; set; }

    public int QueryRef { get; set; }

    public int? VendorHotelId { get; set; }

    public string? Hotel { get; set; }

    public decimal? VendorHotelAmout { get; set; }

    public bool? HotelGst { get; set; }

    public decimal? CustomerHotelAmout { get; set; }

    public int? VendorFlightId { get; set; }

    public string? Flight { get; set; }

    public decimal? VendorFlightAmout { get; set; }

    public bool? FlightGst { get; set; }

    public decimal? CustomerFlightAmout { get; set; }

    public string? FlightNameNumberOutbound { get; set; }

    public string? FlightNameNumberInbound { get; set; }

    public int? VendorPassportId { get; set; }

    public string? Passport { get; set; }

    public decimal? VendorPassportAmout { get; set; }

    public bool? PassportGst { get; set; }

    public decimal? CustomerPassportAmout { get; set; }

    public int? VendorVisaId { get; set; }

    public string? Visa { get; set; }

    public decimal? VendorVisaAmout { get; set; }

    public bool? VisaGst { get; set; }

    public decimal? CustomerVisaAmout { get; set; }

    public int? VendorInsuranceId { get; set; }

    public string? Insurance { get; set; }

    public decimal? VendorInsuranceAmout { get; set; }

    public bool? InsuranceGst { get; set; }

    public decimal? CustomerInsuranceAmout { get; set; }

    public int? VendorSightSeenId { get; set; }

    public string? SightSeen { get; set; }

    public decimal? VendorSightSeenAmout { get; set; }

    public bool? SightSeenGst { get; set; }

    public decimal? CustomerSightSeenAmout { get; set; }

    public int? VendorTransferringId { get; set; }

    public string? Transferring { get; set; }

    public decimal? VendorTransferringAmout { get; set; }

    public bool? TransferringGst { get; set; }

    public decimal? CustomerTransferringAmout { get; set; }

    public string? ExtraDetail { get; set; }

    public decimal? VendorExtra { get; set; }

    public bool? ExtraGst { get; set; }

    public decimal CustomerExtra { get; set; }

    public string? Remark { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? EventTime { get; set; }
}
