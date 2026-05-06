using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Event
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();

    public virtual Tenant Tenant { get; set; } = null!;
}
