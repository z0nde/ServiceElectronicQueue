using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;

namespace ServiceElectronicQueue.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private User _user;

        public AccountController(ILogger<AccountController> logger, CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _logger = logger;
            _userManager = new UserManager(_unitOfWork);
        }

        
        
        [HttpGet]
        public IActionResult UserAccount(Guid userId, string email, string password, Guid role,
            string surname, string name, string patronymic, string phoneNumber)
        {
            _user = new User(userId, email, password, role, surname, name, patronymic, phoneNumber);
            var model = new UserAccountForView
            {
                Name = name,
                Patronymic = patronymic
            };
            return View(model);
        }
        
        [HttpPost]
        public IActionResult UserAccountRegisterOrganization()
        {
            return RedirectToAction("OrganizationRegister", "Home");
        }
        
        [HttpPost]
        public IActionResult UserAccountLoginOrganization()
        {
            return RedirectToAction("OrganizationLogin", "Home");
        }
        
        
        
        [HttpGet]
        public IActionResult OrganizationAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrganizationAccountRegisterBranchOffice()
        {
            return RedirectToAction();
        }

        [HttpPost]
        public IActionResult OrganizationAccountLoginBranchOffice()
        {
            return RedirectToAction();
        }

        
        
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}