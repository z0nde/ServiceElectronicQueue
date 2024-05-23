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
            // получение и десериализация данных из предыдущего контроллера
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, options);
            UserUrl userUrl = JsonSerializer.Deserialize<UserUrl>(jsonUserUrl, options)!;
            UserHttp userHttp = JsonSerializer.Deserialize<UserHttp>(_httpContextAccessor.HttpContext!.Session.GetString("UserDataHttp")!, options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            // парсинг данных в класс User
            _user.SetPropertiesWithoutIdOrganizations(userHttp.IdUser, userUrl.Email, userHttp.Password, userHttp.IdRole, 
                userUrl.Surname, userUrl.Name, userUrl.Patronymic, userUrl.PhoneNumber);
            
            var model = new UserAccountForView
            {
                Name = _user.Name,
                Patronymic = _user.Patronymic,
                Role = _unitOfWork.RoleRep.GetAll()
                    .Where(s => s.IdRole == _user.IdRole)
                    .Select(s => s.Amplua).FirstOrDefault()
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
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, options);
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            
            //отправка статуса auth пользователя в Account контроллер
            //статус - 1/2
            //точка отправки UserAccountRegisterOrganization
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, options));
            
            // распарсинг данных на классы, отвественные за перенаправление определённым типом
            // перенаправление с помощью http
            string jsonUserHttp = JsonSerializer.Serialize(
                new UserHttp(Guid.NewGuid(), user.Password, user.IdRole),
                options
            );
            _httpContextAccessor.HttpContext.Session.SetString("UserDataHttp", jsonUserHttp);
                        
            // перенаправление с помощью url адреса
            string jsonUserUrl = JsonSerializer.Serialize(
                new UserUrl(user.Email, user.Surname, user.Name, user.Patronymic, user.PhoneNumber),
                options
            );
            return RedirectToAction("OrganizationRegister", "OrganizationAuth", new { jsonUserUrl });
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
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, options);
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            
            //отправка статуса auth пользователя в Account контроллер
            //статус - 1/2
            //точка отправки UserAccountRegisterOrganization
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, options));
            
            // распарсинг данных на классы, отвественные за перенаправление определённым типом
            // перенаправление с помощью http
            string jsonUserHttp = JsonSerializer.Serialize(
                new UserHttp(Guid.NewGuid(), user.Password, user.IdRole),
                options
            );
            _httpContextAccessor.HttpContext.Session.SetString("UserDataHttp", jsonUserHttp);
                        
            // перенаправление с помощью url адреса
            string jsonUserUrl = JsonSerializer.Serialize(
                new UserUrl(user.Email, user.Surname, user.Name, user.Patronymic, user.PhoneNumber),
                options
            );
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