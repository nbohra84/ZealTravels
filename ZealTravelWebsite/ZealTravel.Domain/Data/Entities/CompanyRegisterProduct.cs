using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRegisterProduct
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public bool? Flight { get; set; }

    public bool? Hotel { get; set; }

    public bool? Car { get; set; }

    public bool? Insurance { get; set; }

    public bool? Holiday { get; set; }

    public bool? Package { get; set; }

    public bool? Cruise { get; set; }

    public bool? Dmt { get; set; }

    public bool? Visa { get; set; }

    public bool? Railway { get; set; }

    public bool? Bus { get; set; }

    public bool? Recharge { get; set; }

    public bool? OfflineBooking { get; set; }

    public bool? ImportPnr { get; set; }

    public bool? SubCompany { get; set; }

    public bool? Customer { get; set; }

    public bool? Gdsterminal { get; set; }

    public bool? Staff { get; set; }

    public bool? FxdDepartures { get; set; }

    public bool? HotelDepartures { get; set; }

    public bool? Tour { get; set; }

    public bool? Groups { get; set; }

    public bool? AirCalender { get; set; }

    public bool? Prepaid { get; set; }

    public bool? Dc { get; set; }

    public bool? Cc { get; set; }

    public bool? Netbanking { get; set; }

    public bool? Wallet { get; set; }

    public bool? Upi { get; set; }
}
