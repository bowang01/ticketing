using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class OutboxMessage
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string EventType { get; set; } = null!;

    public string PayloadJson { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ProcessedAtUtc { get; set; }
}
