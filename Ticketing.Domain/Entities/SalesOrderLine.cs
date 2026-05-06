using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class SalesOrderLine
{
    public Guid Id { get; set; }

    public Guid SalesOrderId { get; set; }

    public Guid ShowSeatId { get; set; }

    public decimal UnitPrice { get; set; }

    public string? DisplayName { get; set; }

    public virtual SalesOrder SalesOrder { get; set; } = null!;

    public virtual ShowSeat ShowSeat { get; set; } = null!;

    public virtual ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
