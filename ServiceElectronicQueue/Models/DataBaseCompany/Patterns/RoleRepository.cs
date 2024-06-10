using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly CompanyDbContext _db;

        public RoleRepository(CompanyDbContext context) => _db = context;
        
        public IEnumerable<Role> GetAll()
        {
            return _db.Roles;
        }

        public Role GetByIndex(Guid id)
        {
            return _db.Roles
                .Where(s => s.IdRole == id)
                .Select(s => s)
                .First();
        }

        public void Create(Role item)
        {
            _db.Roles.Add(item);
        }

        public void Update(Guid id, Role newItem)
        {
            var unitOfWork = new UnitOfWorkCompany(_db);
            var oldRole = unitOfWork.RoleRep.GetByIndex(id);
            oldRole.Amplua = newItem.Amplua;
        }

        public void Delete(Guid id)
        {
            Role role = _db.Roles.Find(id);
            if (role != null)
                _db.Roles.Remove(role);
        }
    }
}