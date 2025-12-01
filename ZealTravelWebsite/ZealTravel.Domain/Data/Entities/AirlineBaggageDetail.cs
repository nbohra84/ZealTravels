using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class AirlineBaggageDetail
{
    public int Id { get; set; }

    public string? CarrierCode { get; set; }

    public string? CheckInBaggage { get; set; }

    public string? CabinBaggage { get; set; }

    public string? Sector { get; set; }
}
