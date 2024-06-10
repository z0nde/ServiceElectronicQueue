using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithOrganization;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
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
    public IActionResult OrganizationAccount(string jsonUserUrl, string jsonOrgUrl)
    {
        ParserTransmittingGetDataContainerWithOrganization containerWithOrganization =
            new ParserTransmittingGetDataContainerWithOrganization(_httpContextAccessor);
        (DataComeFrom userAuthStatus, _user, _organization) = containerWithOrganization.ParseDeserialize(jsonUserUrl, jsonOrgUrl);
        
        var model = new OrganizationAccountForView
        {
            Title = _organization.Title,
            Surname = _user.Surname,
            Name = _user.Name,
            Patronymic = _user.Patronymic,
            UniqueKey = _organization.UniqueKey
        };
        
        containerWithOrganization.ParseSerialize(userAuthStatus, _user, _organization);
        return View(model);
    }

    [HttpPost]
    public IActionResult OrganizationAccountGenerateUniqueKey()
    {
        ParserTransmittingPostDataContainerWithOrganization containerWithOrganization =
            new ParserTransmittingPostDataContainerWithOrganization(_httpContextAccessor);
        (DataComeFrom userAuthStatus, _user, _organization) = containerWithOrganization.ParseDeserialize();
        
        Random rnd = new();
        string uniqueKey = Convert.ToString(rnd.Next(0, 99999999));
        bool verificationFlagUniqueKey = true;
        while (verificationFlagUniqueKey)
        {
            if (_unitOfWork.OrganizationsRep
                    .GetAll()
                    .Where(s => s.UniqueKey == uniqueKey)
                    .Select(s => s.UniqueKey)
                    .FirstOrDefault() != null) continue;
            _organization.UniqueKey = uniqueKey;
            _unitOfWork.OrganizationsRep.Update(_organization.IdOrganization, _organization);
            _unitOfWork.Save();
            verificationFlagUniqueKey = false;
        }

        (string jsonUserUrl, string jsonOrgUrl) = containerWithOrganization.ParseSerialize(userAuthStatus, _user, _organization);
        return RedirectToAction("OrganizationAccountWithKey", "OrganizationAccount", new
            { jsonUserUrl, jsonOrgUrl});
    }
    
    [HttpGet]
    public IActionResult OrganizationAccountWithKey(string jsonUserUrl, string jsonOrgUrl)
    {
        ParserTransmittingGetDataContainerWithOrganization containerWithOrganization =
            new ParserTransmittingGetDataContainerWithOrganization(_httpContextAccessor);
        (DataComeFrom userAuthStatus, _user, _organization) = containerWithOrganization.ParseDeserialize(jsonUserUrl, jsonOrgUrl);
        
        var model = new OrganizationAccountForView
        {
            Title = _organization.Title,
            Surname = _user.Surname,
            Name = _user.Name,
            Patronymic = _user.Patronymic,
            UniqueKey = _organization.UniqueKey
        };
        
        return View(model);
    }

    protected override void Dispose(bool disposing)
    {
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}