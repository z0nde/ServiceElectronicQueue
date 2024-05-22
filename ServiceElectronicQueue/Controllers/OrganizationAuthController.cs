using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

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
    public IActionResult OrganizationRegister(Guid userId, string email, string password, Guid role,
        string surname, string name, string patronymic, string phoneNumber)
    {
        /*User user = new User(userId, email, password, role, surname, name, patronymic, phoneNumber);
        _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(user));*/
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
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            Organization organization = _organizationManager.RegisterToDb(organizationForView);
            _unitOfWork.OrganizationsRep.Create(organization);
            _unitOfWork.Save();

            user.IdOrganization = organization.IdOrganization;
            _unitOfWork.UsersRep.Create(user);
            _unitOfWork.Save();
            
            //разобраться, почему теряются данные при переходе на следующую страницу!!!
            //потом удалить
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
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