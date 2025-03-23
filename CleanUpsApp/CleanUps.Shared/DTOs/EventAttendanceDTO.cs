namespace CleanUps.Shared.DTOs
{
    public record EventAttendanceDTO : RecordFlag
    {
        public int EventId { get; set; }

        public int UserId { get; set; }

        public DateTime CheckIn { get; set; }

        public virtual EventDTO Event { get; set; } = null!;

        public virtual UserDTO User { get; set; } = null!;
    }
}
