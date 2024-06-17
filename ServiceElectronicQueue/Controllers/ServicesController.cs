using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;

namespace ServiceElectronicQueue.Controllers;

public class ServicesController : Controller
{
    private readonly JsonSerializerOptions _options;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UnitOfWorkCompany _unitOfWork;
    private readonly ServiceManager _serviceManager;
    private User _user;
    private BranchOffice _branchOffice;

    public ServicesController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = new UnitOfWorkCompany(db);
        _serviceManager = new ServiceManager(_unitOfWork);
        _user = new User();
        _branchOffice = new BranchOffice();
    }

    public IActionResult ServicesDisplay(string jsonUserUrl, string jsonBrOfficeUrl)
    {
        var containerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
        (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) =
            containerWithBranchOffice.ParseDeserialize(jsonUserUrl, jsonBrOfficeUrl);

        var services = _unitOfWork.ServicesRep.GetAll();
        var model = services
            .Select(service => new ServicesFormForView
                { NumberService = service.NumberService, Service = service.Service })
            .ToList();

        containerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);

        return View(model);
    }

    [HttpPost]
    public IActionResult GoToCreateService()
    {
        return RedirectToAction("CreateService");
    }

    [HttpGet]
    public IActionResult CreateService()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateService(ServicesFormForView servicesFormForView)
    {
        if (ModelState.IsValid)
        {
            var containerWithBranchOffice =
                new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();

            if (_serviceManager.CheckModel(servicesFormForView) != null)
            {
                Guid idBrOffice = _branchOffice.IdBranchOffice;
                _unitOfWork.ServicesRep.Create(new ServiceSector(
                    Guid.NewGuid(), (int)servicesFormForView.NumberService!, servicesFormForView.Service!, idBrOffice));
                _unitOfWork.Save();

                (string jsonUserUrl, string jsonBrOfficeUrl) =
                    containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
                return RedirectToAction("ServicesDisplay", new { jsonUserUrl, jsonBrOfficeUrl });
            }
        }

        return View();
    }

    [HttpGet]
    public IActionResult UpdateService(int numberService)
    {
        var service = _unitOfWork.ServicesRep.GetAll()
            .FirstOrDefault(s => s.NumberService == numberService);
        if (service == null)
        {
            return NotFound();
        }

        return View(new ServicesFormForView { NumberService = service.NumberService, Service = service.Service });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateService(int numberService, string service)
    {
        var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
        (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();
        if (ModelState.IsValid)
        {
            try
            {
                var serviceSector = _unitOfWork.ServicesRep.GetAll()
                    .FirstOrDefault(s => s.NumberService == numberService);
                serviceSector.Service = service;
                _unitOfWork.ServicesRep.Update(serviceSector.IdServices, serviceSector);
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_unitOfWork.ServicesRep.GetAll()
                    .All(s => s.NumberService != numberService))
                {
                    return NotFound();
                }

                throw;
            }

            (string jsonUserUrl, string jsonBrOfficeUrl) =
                containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return RedirectToAction("ServicesDisplay", new { jsonUserUrl, jsonBrOfficeUrl });
        }

        return View(service);
    }

    [HttpGet]
    public IActionResult DeleteService(int numberService)
    {
        var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
        (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();


        var service = _unitOfWork.ServicesRep.GetAll().FirstOrDefault(s => s.NumberService == numberService);

        if (service == null)
        {
            return NotFound();
        }

        _unitOfWork.ServicesRep.Delete(_unitOfWork.ServicesRep.GetAll()
            .Where(s => service != null && s.NumberService == service.NumberService)
            .Select(s => s.IdServices).FirstOrDefault());
        _unitOfWork.Save();

        (string jsonUserUrl, string jsonBrOfficeUrl) =
            containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
        return RedirectToAction("ServicesDisplay", new { jsonUserUrl, jsonBrOfficeUrl });
    }

    [HttpPost]
    public IActionResult ExitToBranchOfficeAccount()
    {
        var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
        (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) =
            containerWithBranchOffice.ParseDeserialize();

        (string jsonUserUrl, string jsonBrOfficeUrl) =
            containerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);
        return RedirectToAction("BranchOfficeAccount", "BranchOfficeAccount", new { jsonUserUrl, jsonBrOfficeUrl });
    }

    protected override void Dispose(bool disposing)
    {
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}