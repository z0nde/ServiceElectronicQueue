using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews;

//using ServiceElectronicQueue.Containers.CompanyDB;

namespace ServiceElectronicQueue.Controllers
{
    public class LoginRegistrationController : Controller
    {
        private readonly ILogger<LoginRegistrationController> _logger;
        private readonly UnitOfWorkCompany _unitOfWork;

        public LoginRegistrationController(ILogger<LoginRegistrationController> logger, CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _logger = logger;
        }

        [HttpGet]
        public IActionResult UserRegister()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult UserRegister(UserRegisterForView userRegisterForView)
        {
            if (!ModelState.IsValid) 
                return View();
            User user = new();
            _unitOfWork.UsersRep.Create(user.ToDb(userRegisterForView));
            //return RedirectToAction("OrganizationRegister");
            return View();
        }

        /*[HttpGet]
        public IActionResult OrganizationRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrganizationRegister(OrganizationForView organizationForView)
        {
            if (!ModelState.IsValid) 
                return View();
            Organization organization = new();
            _unitOfWork.OrganizationsRep.Create(organization.ToDb(organizationForView));
            return View();
        }*/
        
        public IActionResult Privacy()
        {
            return View();
        }


        
        /*public IActionResult OrganizationLogin()
        {
            return View();
        }

        public IActionResult UserLogin()
        {
            return View();
        }*/

        
        
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