using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record EventAttendanceDTO : RecordDTO
    {
        public int EventId { get; set; }

        public int UserId { get; set; }

        public DateTime CheckIn { get; set; }

        public virtual EventDTO Event { get; set; } = null!;

        public virtual UserDTO User { get; set; } = null!;
    }
}
