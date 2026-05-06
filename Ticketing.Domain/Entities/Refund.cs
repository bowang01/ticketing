using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Refund
{
    public Guid Id { get; set; }

    public Guid SalesOrderId { get; set; }

    public decimal Amount { get; set; }

    public byte Status { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual SalesOrder SalesOrder { get; set; } = null!;
}
