using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Venue
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? AddressLine { get; set; }

    public string? City { get; set; }

    public string? CountryCode { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<SeatMap> SeatMaps { get; set; } = new List<SeatMap>();

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();

    public virtual Tenant Tenant { get; set; } = null!;
}
