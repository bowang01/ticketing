using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Users")]
public sealed class User
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string EmailNormalized { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? DisplayName { get; set; }

    public string? PasswordHash { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
