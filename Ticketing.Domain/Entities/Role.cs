using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Roles")]
public sealed class Role
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(64)]
    public string Name { get; set; } = string.Empty;
}
