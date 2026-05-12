using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Domain.Entities;

[Table("ShowPriceTiers")]
[PrimaryKey(nameof(ShowId), nameof(PriceTierId))]
public sealed class ShowPriceTier
{
    public Guid ShowId { get; set; }

    public Guid PriceTierId { get; set; }
}
