using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Shows")]
public sealed class Show
{
    [Key]
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid EventId { get; set; }

    public Guid VenueId { get; set; }

    public DateTime StartsAtUtc { get; set; }

    public DateTime? EndsAtUtc { get; set; }

    public DateTime? SaleOpensAtUtc { get; set; }

    public DateTime? SaleClosesAtUtc { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
