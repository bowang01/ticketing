using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("PriceTiers")]
public sealed class PriceTier
{
    [Key]
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(3)]
    public string Currency { get; set; } = "NZD";

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
