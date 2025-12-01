using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgAddCity
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public int? Day { get; set; }

    public string? City { get; set; }

    public int? Night { get; set; }

    public string? HotelName { get; set; }

    public string? HotelAddress { get; set; }

    public string? SightseeingSeen { get; set; }

    public string? MealPlan { get; set; }

    public string? Attraction { get; set; }

    public string? Itinerary { get; set; }
}
