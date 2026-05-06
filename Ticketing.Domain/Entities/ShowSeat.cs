using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class ShowSeat
{
    public Guid Id { get; set; }

    public Guid ShowId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceTierId { get; set; }

    public byte Status { get; set; }

    public Guid? LockToken { get; set; }

    public DateTime? LockExpiresAtUtc { get; set; }

    public Guid? SalesOrderLineId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual PriceTier PriceTier { get; set; } = null!;

    public virtual SalesOrderLine? SalesOrderLine { get; set; }

    public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();

    public virtual Seat Seat { get; set; } = null!;

    public virtual Show Show { get; set; } = null!;
}
