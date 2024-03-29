using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;

namespace ServiceElectronicQueue.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UnitOfWorkCompany _unitOfWork;
        
        public AccountController(ILogger<AccountController> logger, CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _logger = logger;
        }

        [HttpGet]
        public IActionResult UserAccount()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrganizationAccount()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}