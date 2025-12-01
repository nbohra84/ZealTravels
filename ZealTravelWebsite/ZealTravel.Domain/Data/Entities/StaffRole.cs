using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class StaffRole
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public int? ProductId { get; set; }

    public bool? Edit { get; set; }

    public bool? Show { get; set; }
}
