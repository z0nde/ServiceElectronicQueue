using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;

namespace ServiceElectronicQueue.Controllers
{
    public class BranchOfficeAuthController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;
        private readonly BranchOfficeManager _branchOfficeManager;
        private BranchOffice _branchOffice;
        
        public BranchOfficeAuthController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(db);
            _branchOfficeManager = new BranchOfficeManager(_unitOfWork);
            _branchOffice = new BranchOffice();
        }

        [HttpGet]
        public IActionResult BranchOfficeRegister()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BranchOfficeLogin()
        {
            return View();
        }
    }
}