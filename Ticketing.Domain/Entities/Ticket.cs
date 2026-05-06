using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Ticket
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid SalesOrderLineId { get; set; }

    public Guid ShowId { get; set; }

    public Guid SeatId { get; set; }

    public string TicketNumber { get; set; } = null!;

    public string? QrPayloadOrHash { get; set; }

    public byte Status { get; set; }

    public DateTime IssuedAtUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual SalesOrderLine SalesOrderLine { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual Show Show { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
