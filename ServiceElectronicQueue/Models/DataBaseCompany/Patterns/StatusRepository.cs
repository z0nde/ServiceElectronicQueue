namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class StatusRepository : IRepository<StatusInQueue>
    {
        private readonly CompanyDbContext _db;

        public StatusRepository(CompanyDbContext context) => _db = context;
        
        public IEnumerable<StatusInQueue> GetAll()
        {
            return _db.Statuses;
        }

        public StatusInQueue GetByIndex(Guid id)
        {
            return _db.Statuses
                .Where(s => s.IdStatus == id)
                .Select(s => s)
                .First();
        }

        public void Create(StatusInQueue item)
        {
            _db.Statuses.Add(item);
        }

        public void Update(Guid id, StatusInQueue newItem)
        {
            var unitOfWork = new UnitOfWorkCompany(_db);
            var oldStatus = unitOfWork.StatusRep.GetByIndex(id);
            oldStatus.Status = newItem.Status;
        }

        public void Delete(Guid id)
        {
            StatusInQueue status = _db.Statuses.Find(id);
            if (status != null)
                _db.Statuses.Remove(status);
        }
    }
}