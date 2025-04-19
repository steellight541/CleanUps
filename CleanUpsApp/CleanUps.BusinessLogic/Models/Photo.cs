using CleanUps.BusinessLogic.Models.AbstractModels;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a photo associated with an event in the CleanUps application.
/// This class stores the photo's data, caption, and its relationship to a specific event.
/// </summary>
public partial class Photo : EntityFrameworkModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the photo.
    /// </summary>
    /// <value>An <see cref="int"/> representing the photo's ID.</value>
    public int PhotoId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the event this photo belongs to.
    /// </summary>
    /// <value>An <see cref="int"/> representing the foreign key to the Event table.</value>
    public int EventId { get; set; }

    /// <summary>
    /// Gets or sets the binary data of the photo.
    /// </summary>
    /// <value>A <see cref="byte[]"/> containing the photo's binary data.</value>
    public byte[] PhotoData { get; set; } = null!;

    /// <summary>
    /// Gets or sets the optional caption for the photo.
    /// </summary>
    /// <value>A <see cref="string"/> containing a description or caption for the photo, or null if no caption is provided.</value>
    public string? Caption { get; set; }

    /// <summary>
    /// Gets or sets the event this photo is associated with.
    /// </summary>
    /// <value>An <see cref="Event"/> object representing the event this photo belongs to.</value>
    public virtual Event Event { get; set; } = null!;
}
