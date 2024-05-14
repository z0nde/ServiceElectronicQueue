using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Controllers;

public class OrganizationAuthController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private readonly UnitOfWorkCompany _unitOfWork;

    private readonly UserManager _userManager;
    private readonly OrganizationManager _organizationManager;
    private User _user;
    private Organization _organization;

    public OrganizationAuthController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = new UnitOfWorkCompany(db);
        _userManager = new UserManager(_unitOfWork);
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
        _user = new User(userId, email, password, role, surname, name, patronymic, phoneNumber);
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
            _user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            _organization = _organizationManager.RegisterToDb(organizationForView);
            _unitOfWork.OrganizationsRep.Create(_organization);
            _unitOfWork.Save();

            _user.IdOrganization = _organization.IdOrganization;
            _user.Organization = _organization;
            _unitOfWork.UsersRep.Create(_user);
            _unitOfWork.Save();
            return RedirectToAction("OrganizationAccount", "OrganizationAccount", new
            {
                OrganizationId = _organization.IdOrganization,
                UserId = _user.IdUser,
                Role = _user.IdRole
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