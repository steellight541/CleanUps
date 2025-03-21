namespace CleanUps.BusinessDomain.Interfaces
{
    public interface ICRUDRepository<T>
    {
        //Create
        public Task Create(T entityToBeCreated);

        //Read
        public Task<List<T>> GetAll();
        public Task<T> GetById(int id);

        //Update
        public Task Update(T entityToBeUpdated);

        //Delete
        public Task Delete(int id);
    }
}
