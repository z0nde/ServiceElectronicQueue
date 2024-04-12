using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class ServicesRepository : IRepository<Services>
    {
        private readonly CompanyDbContext _db;

        public ServicesRepository(CompanyDbContext context) => _db = context;
        
        public IEnumerable<Services> GetAll()
        {
            return _db.Services;
        }

        public Services GetByIndex(Guid id)
        {
            return _db.Services
                .Where(s => s.IdServices == id)
                .Select(s => s)
                .First();
        }

        public void Create(Services item)
        {
            _db.Services.Add(item);
        }

        public void Update(Services item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            Services service = _db.Services.Find(id);
            if (service != null)
                _db.Services.Remove(service);
        }
    }
}