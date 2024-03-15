namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class UnitOfWorkCompany : IDisposable
    {
        private CompanyDbContext _db = new CompanyDbContext();
        private bool _disposed = false;
        
        private UserRepository _userRepository;
        private OrganizationRepository _organizationRepository;
        private BranchOfficeRepository _branchRepository;

        public UserRepository UsersRep
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_db);
                return _userRepository; 
            }
        }

        public OrganizationRepository OrganizationsRep
        {
            get
            {
                if (_organizationRepository == null)
                    _organizationRepository = new OrganizationRepository(_db);
                return _organizationRepository;
            }
        }

        public BranchOfficeRepository BranchesRep
        {
            get
            {
                if (_branchRepository == null)
                    _branchRepository = new BranchOfficeRepository(_db);
                return _branchRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) return;
            if (disposing)
                _db.Dispose();
            _disposed = true;
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}