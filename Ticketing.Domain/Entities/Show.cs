using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Show
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid EventId { get; set; }

    public Guid VenueId { get; set; }

    public DateTime StartsAtUtc { get; set; }

    public DateTime? EndsAtUtc { get; set; }

    public DateTime? SaleOpensAtUtc { get; set; }

    public DateTime? SaleClosesAtUtc { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual ShowSeatMap? ShowSeatMap { get; set; }

    public virtual ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Venue Venue { get; set; } = null!;

    public virtual ICollection<PriceTier> PriceTiers { get; set; } = new List<PriceTier>();
}
