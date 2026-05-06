using System;
using System.Collections.Generic;

namespace Ticketing.Infrastructure.Persistence.Entities;

public partial class ShowSeatMap
{
    public Guid ShowId { get; set; }

    public Guid SeatMapId { get; set; }

    public virtual SeatMap SeatMap { get; set; } = null!;

    public virtual Show Show { get; set; } = null!;
}
