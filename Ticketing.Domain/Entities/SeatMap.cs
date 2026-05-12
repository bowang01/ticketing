using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("SeatMaps")]
public sealed class SeatMap
{
    [Key]
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid VenueId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? LayoutJson { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
