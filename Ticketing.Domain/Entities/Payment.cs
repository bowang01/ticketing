using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Payments")]
public sealed class Payment
{
    [Key]
    public Guid Id { get; set; }

    public Guid SalesOrderId { get; set; }

    [Required, MaxLength(64)]
    public string Provider { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ExternalId { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    public byte Status { get; set; }

    public string? RawJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
