using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithOrganization;
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
        ParserTransmittingGetDataContainer container = new ParserTransmittingGetDataContainer(_httpContextAccessor);
        (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize(jsonUserUrl);
        
        container.ParseSerialize(userAuthStatusPost, _user);
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
            ParserTransmittingPostDataContainer container = new ParserTransmittingPostDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user) = container.ParseDeserialize();
            
            Organization organization = _organizationManager.RegisterToDb(organizationForView);

            if (userAuthStatus.AuthStatus == 1)
            {
                _unitOfWork.OrganizationsRep.Create(organization);
                _unitOfWork.Save();

                user.IdOrganization = organization.IdOrganization;
                _unitOfWork.UsersRep.Create(user);
                _unitOfWork.Save();
            }

            ParserTransmittingPostDataContainerWithOrganization containerWithOrganization =
                new ParserTransmittingPostDataContainerWithOrganization(_httpContextAccessor);
            (string jsonUserUrl, string jsonOrgUrl) = containerWithOrganization.ParseSerialize(userAuthStatus, user, organization);
            //todo "сделать orgAuthStatus";
            return RedirectToAction("OrganizationAccount", "OrganizationAccount", new 
            { jsonUserUrl, jsonOrgUrl });
        }

        return View();
    }

    /// <summary>
    /// Вход организации, GET
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult OrganizationLogin(string jsonUserUrl)
    {
        ParserTransmittingGetDataContainer container = new ParserTransmittingGetDataContainer(_httpContextAccessor);
        (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize(jsonUserUrl);
            
        container.ParseSerialize(userAuthStatusPost, _user);
        return View();
    }

    /// <summary>
    /// Вход организации, POST
    /// </summary>
    /// <param name="organizationLoginForView"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult OrganizationLogin(OrganizationLoginForView organizationForView)
    {
        if (!ModelState.IsValid)
            return View();
        if (_organizationManager.CheckLogin(organizationForView) != null)
        {
            Guid? orgId = _unitOfWork.OrganizationsRep.GetAll()
                .Where(s => s.Email == organizationForView.Email && s.Password == organizationForView.Password)
                .Select(s => s.IdOrganization).FirstOrDefault();
            if (orgId != null && orgId != Guid.Empty)
            {
                ParserTransmittingPostDataContainer container =
                    new ParserTransmittingPostDataContainer(_httpContextAccessor);
                (DataComeFrom userAuthStatus, _user) = container.ParseDeserialize();

                _organization = _unitOfWork.OrganizationsRep.GetByIndex((Guid)orgId);
                
                ParserTransmittingPostDataContainerWithOrganization containerWithOrganization =
                    new ParserTransmittingPostDataContainerWithOrganization(_httpContextAccessor);
                (string jsonUserUrl, string jsonOrgUrl) = containerWithOrganization.ParseSerialize(userAuthStatus, _user, _organization);
                if (_organization.UniqueKey != null)
                {
                    return RedirectToAction("OrganizationAccountWithKey", "OrganizationAccount", new 
                        {jsonUserUrl, jsonOrgUrl});
                }
                return RedirectToAction("OrganizationAccount", "OrganizationAccount", new 
                    {jsonUserUrl, jsonOrgUrl});
            }
        }
        return View();
    }

    protected override void Dispose(bool disposing)
    {
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}