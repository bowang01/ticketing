using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

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

    public virtual DbSet<ShowPriceTier> ShowPriceTiers { get; set; }

    public virtual DbSet<ShowSeat> ShowSeats { get; set; }

    public virtual DbSet<ShowSeatMap> ShowSeatMaps { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

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
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
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

            entity.HasIndex(e => new { e.Status, e.ProcessedAtUtc, e.CreatedAtUtc }, "IX_Outbox_Unprocessed").HasFilter("([Status]=(0))");

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
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
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
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasIndex(e => e.SalesOrderId, "IX_Refunds_Order");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
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
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<SalesOrderLine>(entity =>
        {
            entity.HasIndex(e => e.SalesOrderId, "IX_SOL_Order");

            entity.HasIndex(e => e.ShowSeatId, "IX_SOL_ShowSeat");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasIndex(e => e.SeatMapId, "IX_Seats_SeatMap");

            entity.HasIndex(e => new { e.SeatMapId, e.RowLabel, e.NumberLabel }, "UQ_Seats_Map_Row_Num").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.NumberLabel).HasMaxLength(20);
            entity.Property(e => e.RowLabel).HasMaxLength(20);
            entity.Property(e => e.Section).HasMaxLength(100);
        });

        modelBuilder.Entity<SeatMap>(entity =>
        {
            entity.HasIndex(e => e.VenueId, "IX_SeatMaps_Venue");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
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
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<ShowPriceTier>(entity =>
        {
            entity.HasKey(e => new { e.ShowId, e.PriceTierId });

            entity.HasIndex(e => e.PriceTierId, "IX_ShowPriceTiers_PriceTier");
        });

        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.HasIndex(e => e.LockExpiresAtUtc, "IX_ShowSeats_LockExpires").HasFilter("([LockExpiresAtUtc] IS NOT NULL)");

            entity.HasIndex(e => new { e.ShowId, e.Status }, "IX_ShowSeats_Show_Status");

            entity.HasIndex(e => new { e.ShowId, e.SeatId }, "UQ_ShowSeats_Show_Seat").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.LockExpiresAtUtc).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<ShowSeatMap>(entity =>
        {
            entity.HasKey(e => e.ShowId);

            entity.HasIndex(e => e.SeatMapId, "IX_ShowSeatMaps_SeatMap");

            entity.Property(e => e.ShowId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(e => e.Slug, "UQ_Tenants_Slug").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Slug).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasIndex(e => e.SalesOrderLineId, "IX_Tickets_SOL");

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
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
            entity.Property(e => e.UsedAtUtc).HasPrecision(3);
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
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => e.RoleId, "IX_UserRoles_Role");

            entity.HasIndex(e => e.UserId, "IX_UserRoles_User");
        });

        modelBuilder.Entity<UserTenantMembership>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.UserId }, "IX_UTM_Tenant_User");

            entity.HasIndex(e => new { e.UserId, e.TenantId }, "UQ_UTM_User_Tenant").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
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
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAtUtc).HasPrecision(3);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
