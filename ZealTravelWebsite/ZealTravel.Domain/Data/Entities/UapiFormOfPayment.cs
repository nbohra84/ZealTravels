using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Domain.Data.Entities;

public partial class UapiFormOfPayment
{
    
    public int? Id { get; set; }

    public string? Fop { get; set; }

    public bool? Status { get; set; }
}
