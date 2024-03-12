using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class OrganizationRepository : IRepository<Organization>
    {
        private readonly CompanyDbContext _db;

        public OrganizationRepository(CompanyDbContext context) => _db = context;

        public IEnumerable<Organization> GetAll()
        {
            return _db.Organizations;
        }

        public Organization GetByIndex(int id)
        {
            return _db.Organizations.Find(id);
        }

        public void Create(Organization item)
        {
            _db.Organizations.Add(item);
        }

        public void Update(Organization item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Organization organization = _db.Organizations.Find(id);
            if (organization != null)
                _db.Organizations.Remove(organization);
        }
    }
}