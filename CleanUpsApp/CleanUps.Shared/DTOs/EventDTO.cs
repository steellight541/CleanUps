using CleanUps.BusinessDomain.Models;

namespace CleanUps.Shared.DTOs
{
    /// <summary>
    /// A data transfer object (DTO) representing an event in the CleanUps application.
    /// This record is used to transfer event data between the API layer and the business logic layer.
    /// </summary>
    public record EventDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the street name of the event's location.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string StreetName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the city of the event's location.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ZIP code of the event's location.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string ZipCode { get; set; } = null!;

        /// <summary>
        /// Gets or sets the country of the event's location.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the event.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date on which the event occurs.
        /// </summary>
        public DateOnly DateOfEvent { get; set; }

        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the event.
        /// </summary>
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the event (e.g., "Planned", "Completed").
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the event is family-friendly.
        /// </summary>
        public bool FamilyFriendly { get; set; }

        /// <summary>
        /// Gets or sets the amount of trash collected during the event, in kilograms.
        /// This field is optional and can be <see langword="null"/> if not applicable.
        /// </summary>
        public decimal? TrashCollected { get; set; }

        /// <summary>
        /// Gets or sets the number of attendees at the event.
        /// Defaults to <see cref="int"/> value 0 if not specified.
        /// </summary>
        public int NumberOfAttendees { get; set; } = 0;

        /// <summary>
        /// Gets or sets the collection of attendance records for the event.
        /// Each record links a <see cref="User"/> to the event with a check-in time.
        /// </summary>
        public virtual ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();

        /// <summary>
        /// Gets or sets the collection of photos associated with the event.
        /// Each photo contains image data and metadata related to the event.
        /// </summary>
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}