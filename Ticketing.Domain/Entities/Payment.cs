using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid SalesOrderId { get; set; }

    public string Provider { get; set; } = null!;

    public string? ExternalId { get; set; }

    public decimal Amount { get; set; }

    public byte Status { get; set; }

    public string? RawJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual SalesOrder SalesOrder { get; set; } = null!;
}
