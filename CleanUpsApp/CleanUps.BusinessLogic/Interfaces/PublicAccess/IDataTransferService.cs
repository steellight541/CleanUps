using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IDataTransferService<T> where T : RecordDTO
    {
        public Task<T> CreateAsync(T dtoToCreate);

        public Task<List<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int id);

        public Task<T> UpdateAsync(int id, T dtoToUpdate);

        public Task<T> DeleteAsync(int id);
    }
}
