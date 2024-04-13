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
        private Organization _organization;

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
        public IActionResult UserAccount(Guid userId, string email, string password, string role,
            string surname, string name, string patronymic, string phoneNumber)
        {
            _user = new User(
                userId, 
                email, 
                password, 
                _unitOfWork.RoleRep
                    .GetAll()
                    .Where(s => s.Amplua == role)
                    .Select(s => s.IdRole)
                    .First(), 
                surname, 
                name, 
                patronymic, 
                phoneNumber);
            
            var model = new UserAccountForView
            {
                Name = name,
                Patronymic = patronymic,
                Role = role
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
            return RedirectToAction("OrganizationRegister", "Home", new
            {
                UserId = _user.IdUser, Email = _user.Email, Password = _user.Password, Role = _user.Role,
                Surname = _user.Surname, Name = _user.Name, Patronymic = _user.Patronymic, PhoneNumber = _user.PhoneNumber
            });
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
        public IActionResult OrganizationAccount(Guid orgId, string emailOrg, string passwordOrg, string title, Guid userId,
            string emailUser, string passwordUser, Guid roleId, string surname, string name, string patronymic, string phoneNumber)
        {
            _organization = new Organization(orgId, emailOrg, passwordOrg, title, null, null);
            Random rnd = new();
            string uniqueKey = Convert.ToString(rnd.Next(0, 99999999));
            _organization.UniqueKey = uniqueKey;
            _unitOfWork.OrganizationsRep.Update(_organization);
            return View();
        }

        [HttpPost]
        public IActionResult OrganizationAccountGenerateUniqueKey()
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