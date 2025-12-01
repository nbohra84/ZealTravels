using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAddRoomsPrice
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public int? Roomid { get; set; }

    public int? Priceid { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public int? Adults { get; set; }

    public int? Childs { get; set; }

    public int? ExtraBeds { get; set; }

    public decimal? MonChgs { get; set; }

    public decimal? TueChgs { get; set; }

    public decimal? WedChgs { get; set; }

    public decimal? ThuChgs { get; set; }

    public decimal? FriChgs { get; set; }

    public decimal? SatChgs { get; set; }

    public decimal? SunChgs { get; set; }

    public decimal? ExtraBedsChgs { get; set; }

    public bool? Status { get; set; }

    public DateTime? Eventtime { get; set; }
}
