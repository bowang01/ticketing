using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("SalesOrderLines")]
public sealed class SalesOrderLine
{
    [Key]
    public Guid Id { get; set; }

    public Guid SalesOrderId { get; set; }

    public Guid ShowSeatId { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal UnitPrice { get; set; }

    [MaxLength(200)]
    public string? DisplayName { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
