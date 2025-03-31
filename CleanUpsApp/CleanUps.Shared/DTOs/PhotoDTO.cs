using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record PhotoDTO : RecordDTO
    {
        public int PhotoId { get; set; }

        public int EventId { get; set; }

        public byte[] PhotoData { get; set; } = null!;

        public string? Caption { get; set; }

        public virtual EventDTO Event { get; set; } = null!;
    }
}
