using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CustomerFareDetailHotel
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int? BookingRef { get; set; }

    public decimal? CompanyDebit { get; set; }

    public decimal? CompanyCredit { get; set; }

    public decimal? CompanyTds { get; set; }

    public decimal? CustomerDebit { get; set; }

    public decimal? CustomerCredit { get; set; }

    public int? Markup { get; set; }
}
