using CleanUps.BusinessDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.DatabaseHub;

public partial class CleanUpsContext : DbContext
{
    public CleanUpsContext()
    {
    }

    public CleanUpsContext(DbContextOptions<CleanUpsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventAttendance> EventAttendances { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Do nothing; configuration is handled via dependency injection in Program.cs
        // Optionally, throw an exception if not configured for clarity
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("DbContext has not been configured. Ensure that AddDbContext is called in Program.cs with a valid connection string.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StreetName).HasMaxLength(100);
            entity.Property(e => e.TrashCollected).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ZipCode).HasMaxLength(10);
        });

        modelBuilder.Entity<EventAttendance>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.UserId });

            entity.Property(e => e.CheckIn).HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.EventAttendances)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventAttendancesEventId");

            entity.HasOne(d => d.User).WithMany(p => p.EventAttendances)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventAttendancesUserId");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.Property(e => e.Caption).HasMaxLength(255);

            entity.HasOne(d => d.Event).WithMany(p => p.Photos)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotosId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Role");

            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User");

            entity.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
