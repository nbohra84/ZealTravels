using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PaymentGatewayLogger
{
    public int Id { get; set; }

    public string? MerchantCode { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public int? PaymentId { get; set; }

    public string? CardName { get; set; }

    public decimal? Amount { get; set; }

    public decimal? SurchargeAmount { get; set; }

    public string? TransactionType { get; set; }

    public string? CardType { get; set; }

    public decimal? Surcharge { get; set; }

    public string? RequestRemark { get; set; }

    public bool? RequestStatus { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Pgresponse { get; set; }

    public int? PgresponseCode { get; set; }

    public DateTime? EventTime { get; set; }

    public DateTime? ResponseEventTime { get; set; }

    public string? ResponseRemark { get; set; }

    public bool? ResponseStatus { get; set; }

    public string? Host { get; set; }

    public string? Ip { get; set; }

    public bool? Updated { get; set; }

    public bool? Refunded { get; set; }

    public bool IsManual { get; set; }

    public DateTime? IsManualEvent { get; set; }

    public decimal? RefundAmount { get; set; }

    public bool? IsBilled { get; set; }
}
