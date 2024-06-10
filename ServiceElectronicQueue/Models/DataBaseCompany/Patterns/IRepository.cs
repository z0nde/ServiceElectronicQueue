namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetByIndex(Guid id);
        void Create(T item);
        void Update(Guid id, T newItem);
        void Delete(Guid id);
    }
}