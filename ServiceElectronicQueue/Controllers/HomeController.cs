using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UnitOfWorkCompany _unitOfWork;

        public HomeController(ILogger<HomeController> logger, CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(UserRegisterForView userRegisterForView)
        {
            if (!ModelState.IsValid) 
                return View();
            User user = new();
            //_unitOfWork.UsersRep.Create(user.ToDb(userRegisterForView));
            //return RedirectToAction("OrganizationRegister");
            return View();
        }

        [HttpGet]
        public IActionResult OrganizationRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrganizationRegister(OrganizationRegisterForView organizationForView)
        {
            if (!ModelState.IsValid) 
                return View();
            Organization organization = new();
            _unitOfWork.OrganizationsRep.Create(organization.ToDb(organizationForView));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        
        public IActionResult OrganizationLogin()
        {
            return View();
        }

        public IActionResult UserLogin()
        {
            return View();
        }

        
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}