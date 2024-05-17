using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;

namespace ServiceElectronicQueue.Controllers;

public class OrganizationAccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private readonly UnitOfWorkCompany _unitOfWork;

    private readonly UserManager _userManager;
    private User _user;
    private Organization _organization;

    public OrganizationAccountController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = new UnitOfWorkCompany(db);
        _userManager = new UserManager(_unitOfWork);
        _user = new User();
        _organization = new Organization();
    }
    
    [HttpGet]
    public IActionResult OrganizationAccount(Guid orgId, Guid userId, Guid roleId)
    {
        //разобраться, почему теряются данные при переходе на эту страницу!!!
        //потом удалить
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        _user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, options)!;
        _organization = JsonSerializer.Deserialize<Organization>(_httpContextAccessor.HttpContext!.Session.GetString("OrganizationData")!, options)!;
        _httpContextAccessor.HttpContext.Session.Clear();
        
        //потом раскомментить
        /*_user = _unitOfWork.UsersRep.GetByIndex(userId);
        _organization = _unitOfWork.OrganizationsRep.GetByIndex(orgId);*/
        
        _unitOfWork.OrganizationsRep.Update(_organization);
        _unitOfWork.Save();
        var model = new OrganizationAccountForView
        {
            Title = _organization.Title,
            Surname = _user.Surname,
            Name = _user.Name,
            Patronymic = _user.Patronymic,
            UniqueKey = _organization.UniqueKey
        };
        
        _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(_user, options));
        _httpContextAccessor.HttpContext!.Session.SetString("OrganizationData", JsonSerializer.Serialize(_organization, options));
        
        return View(model);
    }

    [HttpPost]
    public IActionResult OrganizationAccountGenerateUniqueKey()
    {
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        _user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, options)!;
        _organization = JsonSerializer.Deserialize<Organization>(_httpContextAccessor.HttpContext.Session.GetString("OrganizationData")!, options)!;
        _httpContextAccessor.HttpContext.Session.Clear();
        
        Random rnd = new();
        string uniqueKey = Convert.ToString(rnd.Next(0, 99999999));
        bool verificationFlagUniqueKey = true;
        while (verificationFlagUniqueKey == true)
        {
            if (_unitOfWork.OrganizationsRep
                    .GetAll()
                    .Where(s => s.UniqueKey == uniqueKey)
                    .Select(s => s.UniqueKey)
                    .FirstOrDefault() != null) continue;
            _organization.UniqueKey = uniqueKey;
            _unitOfWork.OrganizationsRep.Update(_organization);
            verificationFlagUniqueKey = false;
        }
        
        //разобраться, почему теряются данные при переходе на эту страницу!!!
        //потом удалить
        _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(_user, options));
        _httpContextAccessor.HttpContext!.Session.SetString("OrganizationData", JsonSerializer.Serialize(_organization, options));
        
        return RedirectToAction("OrganizationAccount", "OrganizationAccount", new
        {
            OrganizationId = _organization.IdOrganization, 
            UserId = _user.IdUser, 
            Role = _user.IdRole
        });
    }

    protected override void Dispose(bool disposing)
    {
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}