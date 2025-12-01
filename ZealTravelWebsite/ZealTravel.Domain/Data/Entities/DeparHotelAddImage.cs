using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class DeparHotelAddImage
{
    public int Id { get; set; }

    public int? Refid { get; set; }

    public string? Images { get; set; }

    public int? ImagesOrder { get; set; }
}
