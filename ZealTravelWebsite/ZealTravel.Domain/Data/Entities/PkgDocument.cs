using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgDocument
{
    public int Id { get; set; }

    public int? QueryRef { get; set; }

    public string? DocumentType { get; set; }

    public string? Documents { get; set; }

    public string? StaffId { get; set; }

    public DateTime? EventTime { get; set; }
}
