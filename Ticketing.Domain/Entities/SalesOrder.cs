using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class SalesOrder
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public Guid? UserId { get; set; }

    public string? GuestEmail { get; set; }

    public string? GuestName { get; set; }

    public byte Status { get; set; }

    public string Currency { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public DateTime? LockExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? PaidAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? User { get; set; }
}
