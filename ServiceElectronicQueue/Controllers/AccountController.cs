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
        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private User _user;

        public AccountController(CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _userManager = new UserManager(_unitOfWork);
        }

        
        
        /// <summary>
        /// Аккаунт пользователя, GET
        /// Параметры для перенаправления и отображения данных о пользователе
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="patronymic"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserAccount(Guid userId, string email, string password, Guid role,
            string surname, string name, string patronymic, string phoneNumber)
        {
            _user = new User(userId, email, password, role, surname, name, patronymic, phoneNumber);
            var model = new UserAccountForView
            {
                Name = name,
                Patronymic = patronymic,
                Role = _unitOfWork.RoleRep
                    .GetAll()
                    .Where(s => s.IdRole == role)
                    .Select(s => s.Amplua)
                    .FirstOrDefault()
            };
            return View(model);
        }
        
        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу регистрации аккаунта организации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountRegisterOrganization()
        {
            return RedirectToAction("OrganizationRegister", "Home");
        }
        
        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу входа в аккаунт организации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountLoginOrganization()
        {
            return RedirectToAction("OrganizationLogin", "Home");
        }
        
        
        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу регистрации аккаунта филиала
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountRegisterBranchOffice()
        {
            return RedirectToAction();
        }

        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу входа в аккаунт филиала
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountLoginBranchOffice()
        {
            return RedirectToAction();
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
            _userManager.Dispose();
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}