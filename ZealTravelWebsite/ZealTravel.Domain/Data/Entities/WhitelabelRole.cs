using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class WhitelabelRole
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public bool? Edit { get; set; }

    public bool? Show { get; set; }

    public bool? IsB2b { get; set; }

    public bool? IsB2c { get; set; }
}
