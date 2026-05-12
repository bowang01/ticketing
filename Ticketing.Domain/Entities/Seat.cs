using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities;

[Table("Seats")]
public sealed class Seat
{
    [Key]
    public Guid Id { get; set; }

    public Guid SeatMapId { get; set; }

    [MaxLength(100)]
    public string? Section { get; set; }

    [Required, MaxLength(20)]
    public string RowLabel { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string NumberLabel { get; set; } = string.Empty;

    public byte SeatKind { get; set; }
}
