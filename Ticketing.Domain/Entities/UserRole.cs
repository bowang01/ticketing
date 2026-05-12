using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Domain.Entities;

[Table("UserRoles")]
[PrimaryKey(nameof(UserId), nameof(RoleId))]
public sealed class UserRole
{
    public Guid UserId { get; set; }

    public int RoleId { get; set; }
}
