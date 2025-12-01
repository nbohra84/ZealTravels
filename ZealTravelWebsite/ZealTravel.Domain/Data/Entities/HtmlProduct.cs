using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HtmlProduct
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public string? ProductType { get; set; }

    public string? ProductHeading { get; set; }

    public string? Sector { get; set; }

    public string? Description1 { get; set; }

    public string? Description2 { get; set; }

    public string? Description3 { get; set; }

    public string? HeadImageLocation { get; set; }

    public int? Star { get; set; }

    public string? PageName { get; set; }

    public bool? Status { get; set; }

    public int? OrderNo { get; set; }

    public DateTime? Eventtime { get; set; }
}
