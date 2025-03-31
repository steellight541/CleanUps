using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record PhotoDTO(int PhotoId, int EventId, byte[] PhotoData, string? Caption) : RecordDTO;
}
