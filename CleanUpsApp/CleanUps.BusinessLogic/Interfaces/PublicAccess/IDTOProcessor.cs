using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IDTOProcessor<DTORecord> where DTORecord : RecordFlag
    {
        public Task<DTORecord> CreateAsync(DTORecord dtoToCreate);

        public Task<List<DTORecord>> GetAllAsync();

        public Task<DTORecord> GetByIdAsync(int id);

        public Task<DTORecord> UpdateAsync(int id, DTORecord dtoToUpdate);

        public Task<DTORecord> DeleteAsync(int id);
    }
}
