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

        public Organization GetByIndex(Guid id)
        {
            return _db.Organizations
                .Where(s => s.IdOrganization == id)
                .Select(s => s)
                .First();
        }

        public void Create(Organization item)
        {
            _db.Organizations.Add(item);
        }

        public void Update(Organization item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            Organization organization = _db.Organizations.Find(id);
            if (organization != null)
                _db.Organizations.Remove(organization);
        }
    }
}