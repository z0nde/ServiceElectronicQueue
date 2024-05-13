using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;

namespace ServiceElectronicQueue.Controllers;

public class UserAccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private readonly UnitOfWorkCompany _unitOfWork;

    private readonly UserManager _userManager;
    private User _user;
    private Organization _organization;

    public UserAccountController(CompanyDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = new UnitOfWorkCompany(dbContext);
        _user = new User();
        _organization = new Organization();
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
    public IActionResult UserAccount(string email, string password, string role,
    string surname, string name, string patronymic, string phoneNumber)
    {
        _user = new User(
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
            phoneNumber
        );

        string userDataJson = JsonSerializer.Serialize(_user);
        
        _httpContextAccessor.HttpContext!.Session.SetString("UserData", userDataJson);
            
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
        var regUserDataJson = _httpContextAccessor.HttpContext!.Session.GetString("UserData");
        _user = JsonSerializer.Deserialize<User>(regUserDataJson!)!;
        
        return RedirectToAction("OrganizationRegister", "OrganizationAuth", new
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
        //_userManager.Dispose();
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}