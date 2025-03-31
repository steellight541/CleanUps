using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.Shared.DTOs;

public record EventAttendanceDTO(int EventId, int UserId, DateTime CheckIn) : RecordDTO;
