using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;

namespace ServiceElectronicQueue.Controllers;

public class ServicesController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UnitOfWorkCompany _unitOfWork;
    private readonly ServiceManager _serviceManager;
        
    public ServicesController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = new UnitOfWorkCompany(db);
        _serviceManager = new ServiceManager(_unitOfWork);
    }

    // Получение всех продуктов
    public IActionResult ServicesDisplay()
    {
        var services = _unitOfWork.ServicesRep.GetAll();
        var model = services
            .Select(service => new ServicesFormForView 
                { NumberService = service.NumberService, Service = service.Service })
            .ToList();
        return View(model);
    }
    
    [HttpGet]
    public IActionResult CreateService()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateService(ServicesFormForView service)
    {
        if (ModelState.IsValid)
        {
            if (_serviceManager.CheckModel(service) != null)
            {
                _unitOfWork.ServicesRep.Create(new ServiceSector(
                    Guid.NewGuid(), (uint)service.NumberService!, service.Service!, new Guid(/*подтянуть сюда http сессии*/)));
                _unitOfWork.Save();
                return RedirectToAction(nameof(ServicesDisplay));
            }
        }
        return View(service);
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
        return View(new ServicesFormForView{NumberService = service.NumberService, Service = service.Service});
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateService(int numberService, ServicesFormForView service)
    {
        if (numberService != service.NumberService)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (_serviceManager.CheckModel(service) != null)
                {
                    _unitOfWork.ServicesRep.Update(_unitOfWork.ServicesRep.GetAll()
                        .FirstOrDefault(s => s.NumberService == service.NumberService)!);
                    _unitOfWork.Save();
                }
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
            return RedirectToAction(nameof(ServicesDisplay));
        }
        return View(service);
    }

    [HttpGet]
    public IActionResult DeleteService(int number)
    {
        var service = _unitOfWork.ServicesRep.GetAll().FirstOrDefault(s => s.NumberService == number);
        
        if (service == null)
        {
            return NotFound();
        }
        return View(new ServicesFormForView{ NumberService = service.NumberService, Service = service.Service});
    }
    
    [HttpPost, ActionName("DeleteService")]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmDeletionService(int number)
    {
        var service = _unitOfWork.ServicesRep.GetAll().FirstOrDefault(s => s.NumberService == number);
        _unitOfWork.ServicesRep.Delete(_unitOfWork.ServicesRep.GetAll()
            .Where(s => service != null && s.NumberService == service.NumberService)
            .Select(s => s.IdServices).FirstOrDefault());
        _unitOfWork.Save();
        return RedirectToAction(nameof(ServicesDisplay));
    }

    protected override void Dispose(bool disposing)
    {
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}