using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;

namespace ServiceElectronicQueue.Controllers;

public class OrganizationAccountController : Controller
{
    private readonly UnitOfWorkCompany _unitOfWork;

    private readonly UserManager _userManager;
    private User _user;
    private Organization _organization;
    
    [HttpGet]
    public IActionResult OrganizationAccount(Guid orgId, Guid userId, Guid roleId)
    {
        _user = _unitOfWork.UsersRep.GetByIndex(userId);
        _organization = _unitOfWork.OrganizationsRep.GetByIndex(orgId);
        /*_organization = new Organization(orgId, emailOrg, passwordOrg, title, null, null);
        _unitOfWork.OrganizationsRep.Update(_organization);*/
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

    [HttpPost]
    public IActionResult OrganizationAccountGenerateUniqueKey()
    {
        Random rnd = new();
        string uniqueKey = Convert.ToString(rnd.Next(0, 99999999));
        _organization.UniqueKey = uniqueKey;
        _unitOfWork.OrganizationsRep.Update(_organization);
        return RedirectToAction("OrganizationAccount", "OrganizationAccount", new
        {
            OrganizationId = _organization.IdOrganization, 
            UserId = _user.IdUser, 
            Role = _user.IdRole
        });
    }
}