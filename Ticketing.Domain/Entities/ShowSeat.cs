using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("ShowSeats")]
public sealed class ShowSeat
{
    [Key]
    public Guid Id { get; set; }

    public Guid ShowId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceTierId { get; set; }

    public byte Status { get; set; }

    public Guid? LockToken { get; set; }

    public DateTime? LockExpiresAtUtc { get; set; }

    public Guid? SalesOrderLineId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
