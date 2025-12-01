using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class HtmlProductsImage
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public string? Image { get; set; }
}
