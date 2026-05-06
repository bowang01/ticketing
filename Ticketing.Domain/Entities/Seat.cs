using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class Seat
{
    public Guid Id { get; set; }

    public Guid SeatMapId { get; set; }

    public string? Section { get; set; }

    public string RowLabel { get; set; } = null!;

    public string NumberLabel { get; set; } = null!;

    public byte SeatKind { get; set; }

    public virtual SeatMap SeatMap { get; set; } = null!;

    public virtual ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
