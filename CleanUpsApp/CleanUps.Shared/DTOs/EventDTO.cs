using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs
{
    public record EventDTO : RecordDTO
    {
        public int EventId { get; set; }

        public string StreetName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateOnly DateOfEvent { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Status { get; set; } = null!;

        public bool FamilyFriendly { get; set; }

        public decimal? TrashCollected { get; set; }

        public int NumberOfAttendees { get; set; } = 0;

        public virtual ICollection<EventAttendanceDTO> EventAttendances { get; set; } = new List<EventAttendanceDTO>();

        public virtual ICollection<PhotoDTO> Photos { get; set; } = new List<PhotoDTO>();
    }
}