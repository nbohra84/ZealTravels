using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyProductTdsDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public decimal? FlightTds { get; set; }

    public decimal? HotelTds { get; set; }
}
