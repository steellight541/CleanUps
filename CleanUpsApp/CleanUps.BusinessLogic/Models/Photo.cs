using CleanUps.BusinessLogic.Models.Flags;

namespace CleanUps.BusinessLogic.Models;

/// <summary>
/// Represents a photo associated with an event in the CleanUps application.
/// This class stores the photo's data, caption, and its relationship to a specific event.
/// </summary>
public partial class Photo : EFModel
{
    public int PhotoId { get; set; }

    public int EventId { get; set; }

    public byte[] PhotoData { get; set; } = null!;

    public string? Caption { get; set; }

    public virtual Event Event { get; set; } = null!;
}
