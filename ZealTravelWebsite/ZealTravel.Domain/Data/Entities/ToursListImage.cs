using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class ToursListImage
{
    public int Id { get; set; }

    public int? Tourid { get; set; }

    public string? Images { get; set; }

    public int? ImagesOrder { get; set; }
}
