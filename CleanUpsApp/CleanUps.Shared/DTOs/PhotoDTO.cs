using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    /// <summary>
    /// Represents a photo associated with an event.
    /// Contains the photo data and an optional caption.
    /// </summary>
    public record PhotoDTO : RecordDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the photo.
        /// </summary>
        public int PhotoId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the event this photo belongs to.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the binary data of the photo.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public byte[] PhotoData { get; set; } = null!;

        /// <summary>
        /// Gets or sets an optional caption for the photo.
        /// This field is optional and can be <see langword="null"/> if not applicable.
        /// </summary>
        public string? Caption { get; set; }

        /// <summary>
        /// Gets or sets the event details associated with this photo.
        /// This field is required and cannot be <see langword="null"/> or empty.
        /// </summary>
        public virtual EventDTO Event { get; set; } = null!;
    }
}
