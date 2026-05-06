using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class PriceTier
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
}
