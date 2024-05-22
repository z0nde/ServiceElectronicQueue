using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private User _user;

        public UserAccountController(CompanyDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(dbContext);
            _user = new User();
        }


        /// <summary>
        /// Аккаунт пользователя, GET
        /// Параметры для перенаправления и отображения данных о пользователе
        /// </summary>
        /// <param name="jsonUserUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserAccount(string jsonUserUrl)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(
                _httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, options);
            UserUrl userUrl = JsonSerializer.Deserialize<UserUrl>(jsonUserUrl, options)!;
            UserHttp userHttp = JsonSerializer.Deserialize<UserHttp>(
                _httpContextAccessor.HttpContext!.Session.GetString("UserDataHttp")!, options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            _user = new User(userHttp.IdUser, userUrl.Email, userHttp.Password, userHttp.IdRole, 
                userUrl.Surname, userUrl.Name, userUrl.Patronymic, userUrl.PhoneNumber);
            
            string role = _unitOfWork.RoleRep.GetAll()
                .Where(s => s.IdRole == _user.IdRole)
                .Select(s => s.Amplua).First();
            
            var model = new UserAccountForView
            {
                Name = _user.Name,
                Patronymic = _user.Patronymic,
                Role = role
            };

            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, options));
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(_user, options));

            return View(model);
        }

        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу регистрации
        /// аккаунта организации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountRegisterOrganization()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, options)!;
            //_httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("OrganizationRegister", "OrganizationAuth", new
            {
                _user.IdUser, _user.Email, _user.Password, _user.Role,
                _user.Surname, _user.Name, _user.Patronymic,
                _user.PhoneNumber
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
            return RedirectToAction("OrganizationLogin", "OrganizationAuth");
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


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}