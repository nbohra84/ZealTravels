using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaCategory
{
    public int Id { get; set; }

    public string? VisaCategory { get; set; }

    public string? VisaCategoryName { get; set; }
}
