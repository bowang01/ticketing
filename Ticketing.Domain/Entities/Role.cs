using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserTenantMembership> UserTenantMemberships { get; set; } = new List<UserTenantMembership>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
