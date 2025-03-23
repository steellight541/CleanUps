using CleanUps.Shared.DTOs.Flags;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    /// <summary>
    /// Defines an interface for validating data transfer objects (DTOs) of type <typeparamref name="DTORecord"/>.
    /// </summary>
    /// <typeparam name="DTORecord">
    /// The type of the DTO, which must inherit from <see cref="RecordFlag"/>.
    /// </typeparam>
    internal interface IDTOValidator<DTORecord> where DTORecord : RecordFlag
    {
        /// <summary>
        /// Validates a DTO for creation operations.
        /// </summary>
        /// <param name="dto">
        /// The DTO to validate for creation.
        /// </param>
        void ValidateForCreate(DTORecord dto);

        /// <summary>
        /// Validates a DTO for update operations.
        /// </summary>
        /// <param name="id">
        /// The identifier corresponding to the DTO being updated.
        /// </param>
        /// <param name="dto">
        /// The DTO to validate for updating.
        /// </param>
        void ValidateForUpdate(int id, DTORecord dto);

        /// <summary>
        /// Validates that the provided identifier is valid.
        /// </summary>
        /// <param name="id">
        /// The identifier to validate.
        /// </param>
        void ValidateId(int id);
    }
}
