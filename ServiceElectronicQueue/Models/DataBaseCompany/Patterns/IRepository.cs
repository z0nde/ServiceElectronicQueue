namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetByIndex(Guid id);
        void Create(T item);
        void Update(T item);
        void Delete(Guid id);
    }
}