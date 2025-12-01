using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class CompanyRegisterGst
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? GstcompanyName { get; set; }

    public string? GstcompanyEmail { get; set; }

    public string? GstcompanyContactNumber { get; set; }

    public string? GstcompanyAddress { get; set; }

    public string? Gstnumber { get; set; }

    public DateTime? EventTime { get; set; }
}
