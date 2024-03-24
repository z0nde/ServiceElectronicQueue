using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews;
//using ServiceElectronicQueue.Containers.CompanyDB;

namespace ServiceElectronicQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //private readonly CompanyDbContext _db;
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

        /*[HttpPost]
        public IActionResult Index(UserForView userForView)
        {
            if (ModelState.IsValid)
            {
                ManagerCreate<UserForView, User> managerCreate = new(new CheckCreate(), userForView, new User(), new UserRepository());
                managerCreate.Expansion();
            }
            return RegisterOrganization();
        }*/

        [HttpPost]
        public IActionResult RegisterOrganization(UserForView userForView)
        {
            if (!ModelState.IsValid)
                return View();
            /*ManagerCreate<UserForView, User> managerCreate = new(new CheckCreate(), userForView, new User(), new UserRepository());
            managerCreate.Expansion();*/

            //UnitOfWorkCompany unitOfWorkCompany = new();
            User user = new();
            _unitOfWork.UsersRep.Create(user.ToDb(userForView));
            return View();
        }

        public IActionResult Privacy()
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