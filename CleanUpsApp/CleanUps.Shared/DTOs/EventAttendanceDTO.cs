using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs;

public record EventAttendanceDTO(int UserId, int EventId, DateTime CheckIn) : RecordDTO;
