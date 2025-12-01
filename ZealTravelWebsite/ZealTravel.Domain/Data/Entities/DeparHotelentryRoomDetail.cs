using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelentryRoomDetail
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public int BookingRef { get; set; }

    public string? RoomType { get; set; }

    public string? RoomDescription { get; set; }

    public string? RoomShortDescription { get; set; }

    public DateTime Eventtime { get; set; }
}
