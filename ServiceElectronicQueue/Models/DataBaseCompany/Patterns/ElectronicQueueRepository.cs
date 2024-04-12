using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class ElectronicQueueRepository : IRepository<ElectronicQueue>
    {
        private readonly CompanyDbContext _db;

        public ElectronicQueueRepository(CompanyDbContext context) => _db = context;
        
        public IEnumerable<ElectronicQueue> GetAll()
        {
            return _db.ElectronicQueues;
        }

        public ElectronicQueue GetByIndex(Guid id)
        {
            return _db.ElectronicQueues
                .Where(s => s.IdElectronicQueue == id)
                .Select(s => s)
                .First();
        }

        public void Create(ElectronicQueue item)
        {
            _db.ElectronicQueues.Add(item);
        }

        public void Update(ElectronicQueue item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            ElectronicQueue queue = _db.ElectronicQueues.Find(id);
            if (queue != null)
                _db.ElectronicQueues.Remove(queue);
        }
    }
}