using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class FlightSearchCacheMaster
{
    public int CacheId { get; set; }

    public string CompanyId { get; set; } = null!;

    public string Guid { get; set; } = null!;

    public string SearchRequest { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifyOn { get; set; }

    public int? CreatedBy { get; set; }

    public string? SupplierCode { get; set; }
}
