using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgPaymentoption
{
    public int Id { get; set; }

    public int? Paymentid { get; set; }

    public string Paymentoptions { get; set; } = null!;
}
