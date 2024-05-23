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

namespace ServiceElectronicQueue.Controllers;

public class OrganizationAuthController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private readonly UnitOfWorkCompany _unitOfWork;

    private readonly OrganizationManager _organizationManager;
    private User _user;
    private Organization _organization;

    public OrganizationAuthController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = new UnitOfWorkCompany(db);
        _organizationManager = new OrganizationManager(_unitOfWork);
        _user = new User();
        _organization = new Organization();
    }
    
    /// <summary>
    /// Регистрация организации, GET
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult OrganizationRegister(string jsonUserUrl)
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

    /// <summary>
    /// Регистрация организации, POST
    /// </summary>
    /// <param name="organizationForView"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult OrganizationRegister(OrganizationRegisterForView organizationForView)
    {
        if (!ModelState.IsValid)
            return View();
        if (_organizationManager.CheckRegister(organizationForView) != null)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            Organization organization = _organizationManager.RegisterToDb(organizationForView);
            _unitOfWork.OrganizationsRep.Create(organization);
            _unitOfWork.Save();

            user.IdOrganization = organization.IdOrganization;
            _unitOfWork.UsersRep.Create(user);
            _unitOfWork.Save();
            
            
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(user, options));
            _httpContextAccessor.HttpContext!.Session.SetString("OrganizationData", JsonSerializer.Serialize(organization, options));
            
            return RedirectToAction("OrganizationAccount", "OrganizationAccount", new
            {
                organization.IdOrganization, user.IdUser, user.IdRole
            });
        }

        return View();
    }

    /// <summary>
    /// Вход организации, GET
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult OrganizationLogin()
    {
        return View();
    }

    /// <summary>
    /// Вход организации, POST
    /// </summary>
    /// <param name="organizationLoginForView"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult OrganizationLogin(OrganizationLoginForView organizationLoginForView)
    {
        return RedirectToAction();
    }

    protected override void Dispose(bool disposing)
    {
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}