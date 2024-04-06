using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private readonly OrganizationManager _organizationManager;
        
        public HomeController(ILogger<HomeController> logger, CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _logger = logger;
            _userManager = new UserManager(_unitOfWork);
            _organizationManager = new OrganizationManager(_unitOfWork);
        }

        /// <summary>
        /// Регистрация пользователя, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Регистрация пользователя, POST
        /// </summary>
        /// <param name="userRegisterForView"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(UserRegisterForView userRegisterForView)
        {
            if (!ModelState.IsValid) 
                return View();
            if (_userManager.CheckRegister(userRegisterForView) != null)
            {
                //_unitOfWork.UsersRep.Create(_userManager.RegisterToDb(userRegisterForView));
                
                var user = _userManager.RegisterToDb(userRegisterForView);
                return RedirectToAction("UserAccount", "Account", new
                {
                    //User = _userManager.RegisterToDb(userRegisterForView)
                    UserId = user.IdUser, Email = user.Email, Password = user.Password, Role = user.Role,
                    Surname = user.Surname, Name = user.Name, Patronymic = user.Patronymic, PhoneNumber = user.PhoneNumber
                });
            }
            return View();
        }

        /// <summary>
        /// Вход пользователя, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        /// <summary>
        /// Вход пользователя, POST
        /// </summary>
        /// <param name="userLoginForView"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserLogin(UserLoginForView userLoginForView)
        {
            if (!ModelState.IsValid) 
                return View();
            if (_userManager.CheckLogin(userLoginForView) != null)
            {
                
                
                return RedirectToAction("OrganizationRegister");
            }
            return View();
        }

        /// <summary>
        /// Регистрация организации, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrganizationRegister()
        {
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
                //_unitOfWork.OrganizationsRep.Create(_organizationManager.RegisterToDb(organizationForView));
                //логика входа
                return RedirectToAction("OrganizationRegister");
            }
            return View();
        }
        
        /// <summary>
        /// Вход организации, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrganizationLogin()
        {
            return View();
        }

        /// <summary>
        /// Вход организации, POST
        /// </summary>
        /// <param name="organizationLoginForView"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OrganizationLogin(OrganizationLoginForView organizationLoginForView)
        {
            return RedirectToAction();
        }

        
        
        [HttpGet]
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
            _userManager.Dispose();
            _organizationManager.Dispose();
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}