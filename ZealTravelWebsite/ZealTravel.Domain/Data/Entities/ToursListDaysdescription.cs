using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListDaysdescription
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public int? Day { get; set; }

    public string? TourDescription { get; set; }

    public string? City { get; set; }

    public string? CityDetail { get; set; }

    public string? HotelName { get; set; }

    public string? HotelAddress { get; set; }

    public string? HotelExtraDetail { get; set; }

    public DateTime? Eventtime { get; set; }
}
