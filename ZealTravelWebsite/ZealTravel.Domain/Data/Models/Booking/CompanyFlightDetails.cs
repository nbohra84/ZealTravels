using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class CompanyFlightDetails
    {
        public int Id { get; set; }

        public string? SupplierId { get; set; }

        public string? SupplierIdD { get; set; }

        public string? SupplierIdA { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public string? Sector { get; set; }

        public string? Trip { get; set; }

        public string? Origin { get; set; }

        public string? Destination { get; set; }

        public string? CarrierCodeD { get; set; }

        public string? CarrierCodeA { get; set; }

        public string? AirlinePnrD { get; set; }

        public string? AirlinePnrA { get; set; }

        public string? GdsPnrD { get; set; }

        public string? GdsPnrA { get; set; }

        public string? DepartureDateD { get; set; }

        public string? DepartureDateA { get; set; }

        public bool? IsImport { get; set; }

        public string? MakerId { get; set; }

        public string? StaffId { get; set; }

        public bool? IsOffline { get; set; }

        public bool? IsUpdated { get; set; }

        public bool? IsRejected { get; set; }

        public bool? IsCancelRequested { get; set; }

        public bool? IsCanceled { get; set; }

        public bool? IsCanceledRejected { get; set; }

        public bool? IsRescheduled { get; set; }

        public int? Adt { get; set; }

        public int? Chd { get; set; }

        public int? Inf { get; set; }

        public decimal? TotalTax { get; set; }

        public decimal? TotalBasic { get; set; }

        public decimal? TotalYq { get; set; }

        public decimal? TotalFare { get; set; }

        public decimal? TotalServiceTax { get; set; }

        public decimal? TotalMarkup { get; set; }

        public decimal? TotalBasicDeal { get; set; }

        public decimal? TotalYqDeal { get; set; }

        public decimal? TotalCbDeal { get; set; }

        public decimal? TotalPromoDeal { get; set; }

        public decimal? TotalServiceFeeDeal { get; set; }

        public decimal? TotalCommission { get; set; }

        public decimal? TotalTds { get; set; }

        public decimal? TotalSeat { get; set; }

        public decimal? TotalMeal { get; set; }

        public decimal? TotalBaggage { get; set; }

        public decimal? TotalImport { get; set; }

        public decimal? TotalMarkup1 { get; set; }

        public decimal? TotalBasicDeal1 { get; set; }

        public decimal? TotalYqDeal1 { get; set; }

        public decimal? TotalCbDeal1 { get; set; }

        public decimal? TotalPromoDeal1 { get; set; }

        public decimal? TotalCommission1 { get; set; }

        public decimal? TotalTds1 { get; set; }

        public decimal? TotalFare1 { get; set; }

        public decimal? Totalcfee { get; set; }

        public bool? Status { get; set; }

        public DateTime? EventTime { get; set; }

        public string? PriceTypeD { get; set; }

        public string? PriceTypeA { get; set; }

        public int? AgMarkupD { get; set; }

        public int? AgMarkupA { get; set; }

        public int? Fdmarkup { get; set; }
        public string Email { get; set; } = null!;
        public string AA_Email { get; set; } = null!;
        public string A_Email { get; set; } = null!;

        public string? UserType { get; set; }

        public bool? AccessStatus { get; set; }

        public bool? ActiveStatus { get; set; }

        public int? AccountId { get; set; }
        public int? AA_AccountId { get; set; }

        public string? CompanyLogo { get; set; }
        public string? A_CompanyLogo { get; set; }

        public string? AA_CompanyName { get; set; }
        public string? A_CompanyName { get; set; }
        public string? CompanyName { get; set; }

        public string? AdminId { get; set; }

        public string? Title { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }
        public string? A_Address { get; set; }
        public string? AA_Address { get; set; }

        public int? PostalCode { get; set; }
        public int? AA_PostalCode { get; set; }
        public int? A_PostalCode { get; set; }

        public string? Mobile { get; set; }
        public string? AA_Mobile { get; set; }
        public string? A_Mobile { get; set; }

        public string? PhoneNo { get; set; }
        public string? AA_PhoneNo { get; set; }
        public string? A_PhoneNo { get; set; }

        public string? CompanyEmail { get; set; }

        public string? CompanyMobile { get; set; }

        public string? CompanyPhoneNo { get; set; }
        public bool? AgencyPrintB2c { get; set; }
        public string? FaxNo { get; set; }

        public string? StaxNo { get; set; }

        public decimal? TdsAmount { get; set; }

        public decimal? TdsExemption { get; set; }

        public string? Host { get; set; }

        public string? AbtaNo { get; set; }

        public string? IataNo { get; set; }

        public string? IataMemberSinceYrs { get; set; }

        public string? AtolNo { get; set; }

        public string? TtaNo { get; set; }

        public string? GtgNo { get; set; }

        public string? VatNo { get; set; }

        public string? PanName { get; set; }

        public string? Pan_No { get; set; }
        public string? AA_Pan_No { get; set; }
        public string? A_Pan_No { get; set; }

        public string? TanNo { get; set; }

        public string? NtnNo { get; set; }

        public string? CnpjNo { get; set; }

        public string? ServiceTaxNo { get; set; }

        public string? TpinNo { get; set; }

        public string? LicenseNo { get; set; }

        public string? GstName { get; set; }

        public string? Gst { get; set; }
        public string? A_Gst { get; set; }
        public string? AA_Gst { get; set; }

        public bool? SafiCharge { get; set; }

        public bool? DirectDebitAgent { get; set; }

        public bool? SelfBilling { get; set; }

        public int? YrsInBusiness { get; set; }

        public int? TotalEmployee { get; set; }

        public int? TotalBranches { get; set; }

        public int? AnnualTurnover { get; set; }

        public int? MonthlyBookingVolume { get; set; }

        public string? BusinessType { get; set; }

        public string? OfficeSpace { get; set; }

        public string? ReferredBy { get; set; }

        public bool? CorporateAgent { get; set; }

        public bool? WhitelabelAgent { get; set; }

        public bool? DistributorAgent { get; set; }

        public string? Reference { get; set; }

        public string? Consolidators { get; set; }

        public string? Remarks { get; set; }

        public int? StaffType { get; set; }

        public string? Ip { get; set; }


        public bool? Fdfares { get; set; }


        public string? UpdateBy { get; set; }

    }
}
