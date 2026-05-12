using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("OutboxMessages")]
public sealed class OutboxMessage
{
    [Key]
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    [Required, MaxLength(200)]
    public string EventType { get; set; } = string.Empty;

    [Required]
    public string PayloadJson { get; set; } = string.Empty;

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ProcessedAtUtc { get; set; }
}
