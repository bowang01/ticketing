using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class SeatMap
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid VenueId { get; set; }

    public string Name { get; set; } = null!;

    public string? LayoutJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<ShowSeatMap> ShowSeatMaps { get; set; } = new List<ShowSeatMap>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual Venue Venue { get; set; } = null!;
}
