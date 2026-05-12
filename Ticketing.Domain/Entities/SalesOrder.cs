using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("SalesOrders")]
public sealed class SalesOrder
{
    [Key]
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    [Required, MaxLength(40)]
    public string OrderNumber { get; set; } = string.Empty;

    public Guid? UserId { get; set; }

    [MaxLength(256)]
    public string? GuestEmail { get; set; }

    [MaxLength(200)]
    public string? GuestName { get; set; }

    public byte Status { get; set; }

    [Required, MaxLength(3)]
    public string Currency { get; set; } = "NZD";

    [Column(TypeName = "decimal(12,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime? LockExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? PaidAtUtc { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
