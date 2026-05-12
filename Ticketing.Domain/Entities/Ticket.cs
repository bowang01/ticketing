using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Tickets")]
public sealed class Ticket
{
    [Key]
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid SalesOrderLineId { get; set; }

    public Guid ShowId { get; set; }

    public Guid SeatId { get; set; }

    [Required, MaxLength(40)]
    public string TicketNumber { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? QrPayloadOrHash { get; set; }

    public byte Status { get; set; }

    public DateTime IssuedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
