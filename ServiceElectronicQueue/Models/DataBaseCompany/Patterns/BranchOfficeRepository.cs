using Microsoft.EntityFrameworkCore;

namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class BranchOfficeRepository : IRepository<BranchOffice>
    {
        private readonly CompanyDbContext _db;

        public BranchOfficeRepository(CompanyDbContext context) => _db = context;

        public IEnumerable<BranchOffice> GetAll()
        {
            return _db.BranchOffices;
        }

        public BranchOffice GetByIndex(Guid id)
        {
            return _db.BranchOffices.Find(id);
        }

        public void Create(BranchOffice item)
        {
            _db.BranchOffices.Add(item);
        }

        public void Update(BranchOffice item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            BranchOffice branchOffice = _db.BranchOffices.Find(id);
            if (branchOffice != null!)
                _db.BranchOffices.Remove(branchOffice);
        }
    }
}