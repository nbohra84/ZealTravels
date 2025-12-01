using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAddRoom
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public int? Roomid { get; set; }

    public string? RoomType { get; set; }

    public string? RoomDescription { get; set; }

    public string? RoomShortDescription { get; set; }

    public int? Quantity { get; set; }

    public int? Adults { get; set; }

    public int? Childs { get; set; }

    public int? ExtraBeds { get; set; }

    public string? Amenities { get; set; }

    public string? Services { get; set; }

    public string? Inclusion { get; set; }

    public bool? Status { get; set; }

    public DateTime? Eventtime { get; set; }
}
