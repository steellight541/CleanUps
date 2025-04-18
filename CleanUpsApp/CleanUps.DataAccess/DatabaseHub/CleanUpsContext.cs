using CleanUps.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanUps.DataAccess.DatabaseHub;

/// <summary>
/// Database context for the CleanUps application, providing access to all entity collections
/// and configuring entity relationships, constraints, and database mappings.
/// This context is designed to work with the CleanUps database schema.
/// </summary>
public partial class CleanUpsContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CleanUpsContext"/> class
    /// with default options. This constructor is used by the design-time tools.
    /// </summary>
    public CleanUpsContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CleanUpsContext"/> class
    /// with the specified options. This constructor is used for dependency injection.
    /// </summary>
    /// <param name="options">The options to be used by this context.</param>
    public CleanUpsContext(DbContextOptions<CleanUpsContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the collection of Events in the database.
    /// Events represent cleanup activities with details like title, description, start/end times, and location.
    /// </summary>
    public virtual DbSet<Event> Events { get; set; }

    /// <summary>
    /// Gets or sets the collection of Locations in the database.
    /// Locations contain geographical coordinates (latitude and longitude) for Events.
    /// </summary>
    public virtual DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Gets or sets the collection of Roles in the database.
    /// Roles define the permission levels and capabilities of users in the system (e.g., Organizer, Volunteer).
    /// </summary>
    public virtual DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Gets or sets the collection of Statuses in the database.
    /// Statuses indicate the current state of Events (e.g., Upcoming, Ongoing, Completed, Canceled).
    /// </summary>
    public virtual DbSet<Status> Statuses { get; set; }

    /// <summary>
    /// Gets or sets the collection of EventAttendances in the database.
    /// EventAttendances track which Users are attending which Events, including check-in times.
    /// </summary>
    public virtual DbSet<EventAttendance> EventAttendances { get; set; }

    /// <summary>
    /// Gets or sets the collection of Photos in the database.
    /// Photos store image data and captions related to Events.
    /// </summary>
    public virtual DbSet<Photo> Photos { get; set; }

    /// <summary>
    /// Gets or sets the collection of Users in the database.
    /// Users represent individuals with accounts in the system, containing credentials and personal information.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the collection of PasswordResetTokens in the database.
    /// PasswordResetTokens are used for secure password reset operations, storing tokens with expiration dates.
    /// </summary>
    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    /// <summary>
    /// Configures the database connection for this context when no connection string is explicitly provided.
    /// This method sets up standard behaviors and warnings for the database connection.
    /// </summary>
    /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ensure that the context has been properly configured with a connection string
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("DbContext has not been configured. Ensure that AddDbContext is called in Program.cs with a valid connection string.");
        }

        // Ignore warnings about pending model changes to prevent unnecessary messages during development
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    /// <summary>
    /// Configures the model and entity relationships for this context using the Fluent API.
    /// This method sets up table mappings, relationships, keys, and constraints for all entities.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Event entity
        modelBuilder.Entity<Event>(entity =>
        {
            // Configure one-to-many relationship between Location and Event
            entity.HasOne(e => e.Location)           // Each Event has one Location
                .WithMany()                          // A Location can be referenced by many Events
                .HasForeignKey(e => e.LocationId)    // Foreign key property in Event
                .OnDelete(DeleteBehavior.NoAction);  // Prevent cascading deletes

            // Configure one-to-many relationship between Status and Event
            entity.HasOne(e => e.Status)           // Each Event has one Status
                .WithMany()                        // A Status can be referenced by many Events
                .HasForeignKey(e => e.StatusId)    // Foreign key property in Event
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading deletes

            // Configure TrashCollected as a decimal with precision 18,0
            entity.Property(e => e.TrashCollected).HasColumnType("decimal(18, 0)");

            // Configure CreatedDate with default value of current UTC time
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime2");

            // Configure isDeleted with default value of false
            entity.Property(e => e.isDeleted)
                .HasDefaultValue(false);

            // Add soft-delete trigger to enable soft deletes instead of hard deletes
            entity.ToTable(tb => tb.HasTrigger("TR_Events_InsteadOfDelete"));
            
            // Add check constraint to ensure end time is after start time
            entity.ToTable(t => t.HasCheckConstraint("CHK_EndTimeAfterStartTime", "EndTime > StartTime"));
        });

        // Configure EventAttendance entity (junction table for many-to-many between Event and User)
        modelBuilder.Entity<EventAttendance>(entity =>
        {
            // Set composite primary key using both foreign keys
            entity.HasKey(e => new { e.EventId, e.UserId });

            // Configure CheckIn as datetime2 type
            entity.Property(e => e.CheckIn).HasColumnType("datetime2");

            // Configure CreatedDate with default value of current UTC time
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime2");

            // Configure many-to-one relationship from EventAttendance to Event
            entity.HasOne(d => d.Event).WithMany(p => p.EventAttendances)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)  // Delete attendance records when parent event is deleted
                .HasConstraintName("FK_EventAttendances_Events_EventId");

            // Configure many-to-one relationship from EventAttendance to User
            entity.HasOne(d => d.User).WithMany(p => p.EventAttendances)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)  // Delete attendance records when parent user is deleted
                .HasConstraintName("FK_EventAttendances_Users_UserId");
        });

        // Configure Photo entity
        modelBuilder.Entity<Photo>(entity =>
        {
            // Set maximum length for Caption
            entity.Property(e => e.Caption).HasMaxLength(255);

            // Configure many-to-one relationship from Photo to Event
            entity.HasOne(d => d.Event).WithMany(p => p.Photos)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.NoAction)  // Prevent cascading deletes for photos
                .HasConstraintName("FK_Photos_Events_EventId");
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Set primary key name explicitly
            entity.HasKey(e => e.UserId).HasName("PK_User");

            // Set unique constraint on Email
            entity.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            // Configure CreatedDate with default value and correct type
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime2"); // Updated type back to datetime2

            // Configure isDeleted with default value of false (soft delete)
            entity.Property(e => e.isDeleted)
                .HasDefaultValue(false);

            // Set maximum lengths for string properties
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            // PasswordHash is NVARCHAR(MAX) in SQL, so no MaxLength needed here for EF Core
            // entity.Property(e => e.PasswordHash).HasMaxLength(50); // Removed MaxLength

            // Configure one-to-many relationship between Role and User
            entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);  // Prevent cascading deletes for users

            // Add soft-delete trigger to enable soft deletes instead of hard deletes
            entity.ToTable(tb => tb.HasTrigger("TR_Users_InsteadOfDelete"));
        });

        // Configure Location entity
        modelBuilder.Entity<Location>(entity =>
        {
            // Set precision for latitude (degrees) to allow accurate coordinate storage
            entity.Property(l => l.Latitude)
            .HasPrecision(10, 7);  // 7 decimal places allows for ~1cm precision

            // Set precision for longitude (degrees) to allow accurate coordinate storage
            entity.Property(l => l.Longitude)
            .HasPrecision(10, 7);  // 7 decimal places allows for ~1cm precision

            // Configure Id as auto-incrementing
            entity.Property(l => l.Id)
            .ValueGeneratedOnAdd(); // Ensures ID is generated by the database
        });

        // Configure PasswordResetToken entity
        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            // Set primary key
            entity.HasKey(e => e.Id);

            // Set unique constraint on Token and updated MaxLength
            entity.HasIndex(e => e.Token, "UQ_PasswordResetToken_Token").IsUnique();
            entity.Property(e => e.Token).HasMaxLength(450); // Updated MaxLength

            // Configure CreatedDate with default value of current UTC time
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime2");

            // Configure IsUsed with default value of false
            entity.Property(e => e.IsUsed)
                .HasDefaultValue(false);

            // Configure one-to-many relationship between User and PasswordResetToken
            entity.HasOne(d => d.User)
                .WithMany()  // User can have many password reset tokens
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Delete tokens when user is deleted
        });

        // Call any additional model configuration defined in partial class implementations
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
