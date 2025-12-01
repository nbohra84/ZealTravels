using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HtmlProductsQuery
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public string? ProductType { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public DateTime? Eventtime { get; set; }
}
