using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Refunds")]
public sealed class Refund
{
    [Key]
    public Guid Id { get; set; }

    public Guid SalesOrderId { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal Amount { get; set; }

    public byte Status { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
