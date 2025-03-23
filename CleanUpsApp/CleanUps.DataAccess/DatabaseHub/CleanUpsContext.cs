using CleanUps.BusinessDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.DataAccess.DatabaseHub;

/// <summary>
/// Represents the database context for the CleanUps application, providing access to entity sets and configuring the database connection.
/// This class inherits from <see cref="DbContext"/> and is used to interact with the underlying SQL Server database.
/// </summary>
public partial class CleanUpsContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CleanUpsContext"/> class.
    /// This parameterless constructor is typically used for design-time operations, such as migrations.
    /// </summary>
    public CleanUpsContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CleanUpsContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The <see cref="DbContextOptions{TContext}"/> containing configuration options for the context, such as the database connection string.</param>
    public CleanUpsContext(DbContextOptions<CleanUpsContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="Event"/> entities, representing events in the CleanUps application.
    /// </summary>
    public virtual DbSet<Event> Events { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="EventAttendance"/> entities, representing user attendance records for events.
    /// </summary>
    public virtual DbSet<EventAttendance> EventAttendances { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="Photo"/> entities, representing photos associated with events.
    /// </summary>
    public virtual DbSet<Photo> Photos { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="Role"/> entities, representing user roles in the CleanUps application.
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="User"/> entities, representing users in the CleanUps application.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }


    ////protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    ////    => optionsBuilder.UseSqlServer("Server=cleanups.database.windows.net;Database=CleanUpsDb;User Id=zerowaste;Password=Erasmusbipcleanups4!;TrustServerCertificate=True;");
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

    /// <summary>
    /// Provides a partial method for additional model configuration in a separate partial class.
    /// This method allows extending the model configuration without modifying the main <see cref="OnModelCreating"/> method.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity model.</param>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
