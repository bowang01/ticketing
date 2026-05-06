using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Ticketing.Infrastructure.Persistence.Entities;

namespace Ticketing.Infrastructure.Persistence;

public partial class TicketingDbContext : DbContext
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<IdempotencyKey> IdempotencyKeys { get; set; }

    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PriceTier> PriceTiers { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalesOrder> SalesOrders { get; set; }

    public virtual DbSet<SalesOrderLine> SalesOrderLines { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatMap> SeatMaps { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    public virtual DbSet<ShowSeat> ShowSeats { get; set; }

    public virtual DbSet<ShowSeatMap> ShowSeatMaps { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTenantMembership> UserTenantMemberships { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.Status }, "IX_Events_Tenant_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Subtitle).HasMaxLength(300);
            entity.Property(e => e.Title).HasMaxLength(300);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Events)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Events_Tenants");
        });

        modelBuilder.Entity<IdempotencyKey>(entity =>
        {
            entity.HasKey(e => new { e.KeyHash, e.Scope });

            entity.Property(e => e.KeyHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Scope).HasMaxLength(64);
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ExpiresAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Outbox");

            entity.HasIndex(e => new { e.ProcessedAtUtc, e.CreatedAtUtc }, "IX_Outbox_Unprocessed").HasFilter("([ProcessedAtUtc] IS NULL)");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EventType).HasMaxLength(200);
            entity.Property(e => e.ProcessedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(e => e.SalesOrderId, "IX_Payments_Order");

            entity.HasIndex(e => new { e.Provider, e.ExternalId }, "UQ_Payments_Provider_External")
                .IsUnique()
                .HasFilter("([ExternalId] IS NOT NULL)");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ExternalId).HasMaxLength(200);
            entity.Property(e => e.Provider).HasMaxLength(64);

            entity.HasOne(d => d.SalesOrder).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<PriceTier>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("NZD")
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasOne(d => d.Tenant).WithMany(p => p.PriceTiers)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PriceTiers_Tenants");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Reason).HasMaxLength(500);

            entity.HasOne(d => d.SalesOrder).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Refunds_Orders");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(64);
        });

        modelBuilder.Entity<SalesOrder>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CreatedAtUtc }, "IX_SalesOrders_Tenant_Created").IsDescending(false, true);

            entity.HasIndex(e => e.UserId, "IX_SalesOrders_User").HasFilter("([UserId] IS NOT NULL)");

            entity.HasIndex(e => e.OrderNumber, "UQ_SalesOrders_OrderNumber").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("NZD")
                .IsFixedLength();
            entity.Property(e => e.GuestEmail).HasMaxLength(256);
            entity.Property(e => e.GuestName).HasMaxLength(200);
            entity.Property(e => e.LockExpiresAtUtc).HasPrecision(3);
            entity.Property(e => e.OrderNumber).HasMaxLength(40);
            entity.Property(e => e.PaidAtUtc).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Tenant).WithMany(p => p.SalesOrders)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SO_Tenants");

            entity.HasOne(d => d.User).WithMany(p => p.SalesOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_SO_Users");
        });

        modelBuilder.Entity<SalesOrderLine>(entity =>
        {
            entity.HasIndex(e => e.SalesOrderId, "IX_SOL_Order");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.SalesOrder).WithMany(p => p.SalesOrderLines)
                .HasForeignKey(d => d.SalesOrderId)
                .HasConstraintName("FK_SOL_Orders");

            entity.HasOne(d => d.ShowSeat).WithMany(p => p.SalesOrderLines)
                .HasForeignKey(d => d.ShowSeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SOL_ShowSeats");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasIndex(e => e.SeatMapId, "IX_Seats_SeatMap");

            entity.HasIndex(e => new { e.SeatMapId, e.RowLabel, e.NumberLabel }, "UQ_Seats_Map_Row_Num").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.NumberLabel).HasMaxLength(20);
            entity.Property(e => e.RowLabel).HasMaxLength(20);
            entity.Property(e => e.Section).HasMaxLength(100);

            entity.HasOne(d => d.SeatMap).WithMany(p => p.Seats)
                .HasForeignKey(d => d.SeatMapId)
                .HasConstraintName("FK_Seats_SeatMaps");
        });

        modelBuilder.Entity<SeatMap>(entity =>
        {
            entity.HasIndex(e => e.VenueId, "IX_SeatMaps_Venue");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Tenant).WithMany(p => p.SeatMaps)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatMaps_Tenants");

            entity.HasOne(d => d.Venue).WithMany(p => p.SeatMaps)
                .HasForeignKey(d => d.VenueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeatMaps_Venues");
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasIndex(e => e.StartsAtUtc, "IX_Shows_StartsAt");

            entity.HasIndex(e => new { e.TenantId, e.EventId }, "IX_Shows_Tenant_Event");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EndsAtUtc).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.SaleClosesAtUtc).HasPrecision(3);
            entity.Property(e => e.SaleOpensAtUtc).HasPrecision(3);
            entity.Property(e => e.StartsAtUtc).HasPrecision(3);

            entity.HasOne(d => d.Event).WithMany(p => p.Shows)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shows_Events");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Shows)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shows_Tenants");

            entity.HasOne(d => d.Venue).WithMany(p => p.Shows)
                .HasForeignKey(d => d.VenueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shows_Venues");

            entity.HasMany(d => d.PriceTiers).WithMany(p => p.Shows)
                .UsingEntity<Dictionary<string, object>>(
                    "ShowPriceTier",
                    r => r.HasOne<PriceTier>().WithMany()
                        .HasForeignKey("PriceTierId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SPT_PriceTiers"),
                    l => l.HasOne<Show>().WithMany()
                        .HasForeignKey("ShowId")
                        .HasConstraintName("FK_SPT_Shows"),
                    j =>
                    {
                        j.HasKey("ShowId", "PriceTierId");
                        j.ToTable("ShowPriceTiers");
                    });
        });

        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.HasIndex(e => e.LockExpiresAtUtc, "IX_ShowSeats_LockExpires").HasFilter("([LockExpiresAtUtc] IS NOT NULL)");

            entity.HasIndex(e => new { e.ShowId, e.Status }, "IX_ShowSeats_Show_Status");

            entity.HasIndex(e => new { e.ShowId, e.SeatId }, "UQ_ShowSeats_Show_Seat").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.LockExpiresAtUtc).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.PriceTier).WithMany(p => p.ShowSeats)
                .HasForeignKey(d => d.PriceTierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShowSeats_PriceTiers");

            entity.HasOne(d => d.SalesOrderLine).WithMany(p => p.ShowSeats)
                .HasForeignKey(d => d.SalesOrderLineId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ShowSeats_SalesOrderLines");

            entity.HasOne(d => d.Seat).WithMany(p => p.ShowSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShowSeats_Seats");

            entity.HasOne(d => d.Show).WithMany(p => p.ShowSeats)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK_ShowSeats_Shows");
        });

        modelBuilder.Entity<ShowSeatMap>(entity =>
        {
            entity.HasKey(e => e.ShowId);

            entity.Property(e => e.ShowId).ValueGeneratedNever();

            entity.HasOne(d => d.SeatMap).WithMany(p => p.ShowSeatMaps)
                .HasForeignKey(d => d.SeatMapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SSM_SeatMaps");

            entity.HasOne(d => d.Show).WithOne(p => p.ShowSeatMap)
                .HasForeignKey<ShowSeatMap>(d => d.ShowId)
                .HasConstraintName("FK_SSM_Shows");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(e => e.Slug, "UQ_Tenants_Slug").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Slug).HasMaxLength(100);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.ShowId }, "IX_Tickets_Tenant_Show");

            entity.HasIndex(e => e.TicketNumber, "UQ_Tickets_Number").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.IssuedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.QrPayloadOrHash).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TicketNumber).HasMaxLength(40);
            entity.Property(e => e.UsedAtUtc).HasPrecision(3);

            entity.HasOne(d => d.SalesOrderLine).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SalesOrderLineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_SOL");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Seats");

            entity.HasOne(d => d.Show).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ShowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Shows");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Tenants");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.EmailNormalized, "IX_Users_EmailNormalized");

            entity.HasIndex(e => e.EmailNormalized, "UQ_Users_EmailNormalized").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailNormalized).HasMaxLength(256);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserRoles_Roles"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRoles_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UserRoles");
                    });
        });

        modelBuilder.Entity<UserTenantMembership>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.UserId }, "IX_UTM_Tenant_User");

            entity.HasIndex(e => new { e.UserId, e.TenantId }, "UQ_UTM_User_Tenant").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.UserTenantMemberships)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UTM_Roles");

            entity.HasOne(d => d.Tenant).WithMany(p => p.UserTenantMemberships)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UTM_Tenants");

            entity.HasOne(d => d.User).WithMany(p => p.UserTenantMemberships)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UTM_Users");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasIndex(e => e.TenantId, "IX_Venues_Tenant");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AddressLine).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Tenant).WithMany(p => p.Venues)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venues_Tenants");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
