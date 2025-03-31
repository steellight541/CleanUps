using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record UserDTO : RecordDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<EventAttendanceDTO> EventAttendances { get; set; } = new List<EventAttendanceDTO>();
    }
}
