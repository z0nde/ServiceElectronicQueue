namespace ServiceElectronicQueue.Models.DataBaseCompany.Patterns
{
    public class UnitOfWorkCompany : IDisposable
    {
        private readonly CompanyDbContext _db;
        private bool _disposed = false;
        
        private UserRepository _userRepository;
        private OrganizationRepository _organizationRepository;
        private BranchOfficeRepository _branchRepository;
        private RoleRepository _roleRepository;
        private ServicesRepository _servicesRepository;
        private StatusRepository _statusRepository;
        private ElectronicQueueRepository _electronicQueueRepository;

        public OrganizationRepository OrganizationsRep
        {
            get
            {
                if (_organizationRepository == null)
                    _organizationRepository = new OrganizationRepository(_db);
                return _organizationRepository;
            }
        }
        
        public UserRepository UsersRep
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_db);
                return _userRepository; 
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
        
        public RoleRepository RoleRep
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_db);
                return _roleRepository;
            }
        }
        
        public ServicesRepository ServicesRep
        {
            get
            {
                if (_servicesRepository == null)
                    _servicesRepository = new ServicesRepository(_db);
                return _servicesRepository;
            }
        }
        
        public StatusRepository StatusRep
        {
            get
            {
                if (_statusRepository == null)
                    _statusRepository = new StatusRepository(_db);
                return _statusRepository;
            }
        }
        
        public ElectronicQueueRepository ElectronicQueueRep
        {
            get
            {
                if (_electronicQueueRepository == null)
                    _electronicQueueRepository = new ElectronicQueueRepository(_db);
                return _electronicQueueRepository;
            }
        }

        public UnitOfWorkCompany(CompanyDbContext db) => _db = db;
        
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