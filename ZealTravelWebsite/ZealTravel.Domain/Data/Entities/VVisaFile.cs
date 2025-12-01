using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class VVisaFile
{
    public int Id { get; set; }

    public int? Visaid { get; set; }

    public int? Fileid { get; set; }

    public string? VisaCategory { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public DateTime? Eventtime { get; set; }
}
