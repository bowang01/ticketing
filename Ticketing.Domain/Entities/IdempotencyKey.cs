using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Domain.Entities;

[Table("IdempotencyKeys")]
[PrimaryKey(nameof(KeyHash), nameof(Scope))]
public sealed class IdempotencyKey
{
    [MaxLength(64)]
    public string KeyHash { get; set; } = string.Empty;

    [MaxLength(64)]
    public string Scope { get; set; } = string.Empty;

    public string? ResponseJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }
}
