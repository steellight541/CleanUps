namespace CleanUps.BusinessDomain.Models;

public partial class Photo
{
    public int PhotoId { get; set; }

    public int EventId { get; set; }

    public byte[] PhotoData { get; set; } = null!;

    public string? Caption { get; set; }

    public virtual Event Event { get; set; } = null!;
}
