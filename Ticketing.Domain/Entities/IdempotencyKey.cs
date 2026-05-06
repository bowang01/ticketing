using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class IdempotencyKey
{
    public string KeyHash { get; set; } = null!;

    public string Scope { get; set; } = null!;

    public string? ResponseJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }
}
