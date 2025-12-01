using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class WhitelabelSupport
{
    public int Id { get; set; }

    public string CompanyId { get; set; } = null!;

    public string? Description { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }

    public string? SocialLinkDesc { get; set; }

    public string? SocialUrl { get; set; }
}
