using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string EmailNormalized { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? PasswordHash { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();

    public virtual ICollection<UserTenantMembership> UserTenantMemberships { get; set; } = new List<UserTenantMembership>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
