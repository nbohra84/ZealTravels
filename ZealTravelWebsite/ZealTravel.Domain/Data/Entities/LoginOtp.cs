using System;
using System.Collections.Generic;

namespace ZealTravel.Domain.Data.Entities;

public partial class LoginOtp
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? Otp { get; set; }

    public DateTime? EventTime { get; set; }
}
