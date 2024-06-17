using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class ServicesRepository : IRepository<ServiceSector>
    {
        private readonly CompanyDbContext _db;

        public ServicesRepository(CompanyDbContext context) => _db = context;
        
        public IEnumerable<ServiceSector> GetAll()
        {
            return _db.Services;
        }

        public ServiceSector GetByIndex(Guid id)
        {
            return _db.Services
                .Where(s => s.IdServices == id)
                .Select(s => s)
                .First();
        }

        public void Create(ServiceSector item)
        {
            _db.Services.Add(item);
        }

        public void Update(Guid id, ServiceSector newItem)
        {
            var unitOfWork = new UnitOfWorkCompany(_db);
            var oldService = unitOfWork.ServicesRep.GetByIndex(id);
            oldService.Service = newItem.Service;
        }

        public void Delete(Guid id)
        {
            ServiceSector serviceSector = _db.Services.Find(id);
            if (serviceSector != null)
                _db.Services.Remove(serviceSector);
        }
    }
}