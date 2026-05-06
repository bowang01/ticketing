using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Tenant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<PriceTier> PriceTiers { get; set; } = new List<PriceTier>();

    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();

    public virtual ICollection<SeatMap> SeatMaps { get; set; } = new List<SeatMap>();

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserTenantMembership> UserTenantMemberships { get; set; } = new List<UserTenantMembership>();

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();
}
