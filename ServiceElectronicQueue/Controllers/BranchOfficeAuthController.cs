using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.Controllers
{
    public class BranchOfficeAuthController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;
        private readonly BranchOfficeManager _branchOfficeManager;
        private BranchOffice _branchOffice;
        private User _user;
        
        public BranchOfficeAuthController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(db);
            _branchOfficeManager = new BranchOfficeManager(_unitOfWork);
            _branchOffice = new BranchOffice();
            _user = new User();
        }

        [HttpGet]
        public IActionResult BranchOfficeRegister(string jsonUserUrl)
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
            
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, options));
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(_user, options));
            return View();
        }

        [HttpPost]
        public IActionResult BranchOfficeRegister(BranchOfficeRegisterForView branchOfficeRegisterForView)
        {
            if (!ModelState.IsValid)
                return View();
            if (_branchOfficeManager.CheckRegisterModel(branchOfficeRegisterForView) != null)
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };
                
                return RedirectToAction();
            }
            return View();
        }
        
        
        
        [HttpGet]
        public IActionResult BranchOfficeLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BranchOfficeLogin(BranchOfficeLoginForView branchOfficeLoginForView)
        {
            if (!ModelState.IsValid)
                return View();
            if (_branchOfficeManager.CheckLoginModel(branchOfficeLoginForView) != null)
            {
                
                return RedirectToAction();
            }
            return View();
        }
    }
}