using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class PkgQueryReplyVendor
{
    public int Id { get; set; }

    public int? QueryRef { get; set; }

    public int? ReplyRef { get; set; }

    public int? VendorTypeId { get; set; }

    public string? Vendor { get; set; }
}
