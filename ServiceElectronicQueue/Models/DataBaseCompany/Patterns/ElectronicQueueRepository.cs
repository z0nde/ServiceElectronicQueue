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

        public void Update(Guid id, ElectronicQueue newItem)
        {
            var unitOfWork = new UnitOfWorkCompany(_db);
            var oldElectQueue = unitOfWork.ElectronicQueueRep.GetByIndex(id);
            oldElectQueue.NumberService = newItem.NumberService;
            oldElectQueue.StartService = newItem.StartService;
            oldElectQueue.EndService = newItem.EndService;
        }

        public void Delete(Guid id)
        {
            ElectronicQueue queue = _db.ElectronicQueues.Find(id);
            if (queue != null)
                _db.ElectronicQueues.Remove(queue);
        }
    }
}