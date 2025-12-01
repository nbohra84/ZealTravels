using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class UapiCcDetail
{
    public int Id { get; set; }

    public string? BankCountryCode { get; set; }

    public string? BankName { get; set; }

    public string? Cvv { get; set; }

    public string? ExpDate { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public string? Type { get; set; }

    public string? AddressName { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Carriers { get; set; }
}
