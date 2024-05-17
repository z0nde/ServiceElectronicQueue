using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private readonly OrganizationManager _organizationManager;
        private User _user;

        public HomeController(CompanyDbContext db)
        {
            _unitOfWork = new UnitOfWorkCompany(db);
            _userManager = new UserManager(_unitOfWork);
            _organizationManager = new OrganizationManager(_unitOfWork);
        }

        /*/// <summary>
        /// Регистрация пользователя, GET
        /// </summary>
        /// <returns></returns>
        /*[HttpGet]
        public IActionResult Index()
        {
            /*var model = new UserRegisterForView()
            {
                RoleItems = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "1",
                        Text = _unitOfWork.RoleRep.GetAll().Select(s => s.Amplua).First()
                    },
                    new SelectListItem
                    {
                        Value = "2",
                        Text = _unitOfWork.RoleRep.GetAll().Select(s => s.Amplua).Skip(1).First()
                    }
                }
            };#2#
            return View( /*model#2#);
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
            /*userRegisterForView.Role = userRegisterForView.RoleItems
                .Where(s => s.Value == userRegisterForView.SelectRoleItem.ToString())
                .Select(s => s.Text)
                .First();#2#
            if (_userManager.CheckRegisterModel(userRegisterForView) != null)
            {
                if (userRegisterForView.Role is "Пользователь организации" or "Пользователь филиала")
                {
                    /*_unitOfWork.UsersRep.Create(_userManager.RegisterToDb(userRegisterForView));
                    _unitOfWork.Save();#2#

                    //var user = _userManager.RegisterToDb(userRegisterForView);
                    return RedirectToAction("UserAccount", "UserAccount", new
                    {
                        UserId = Guid.NewGuid(), Email = userRegisterForView.Email,
                        Password = userRegisterForView.Password,
                        Role = userRegisterForView.Role, Surname = userRegisterForView.Surname,
                        Name = userRegisterForView.Name,
                        Patronymic = userRegisterForView.Patronymic, PhoneNumber = userRegisterForView.PhoneNumber
                    });
                }
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
            if (_userManager.CheckLoginModel(userLoginForView) != null)
            {
                Guid? userId = _unitOfWork.UsersRep
                    .GetAll()
                    .ToList()
                    .Where(s => s.Email == userLoginForView.Email && s.Password == userLoginForView.Password)
                    .Select(s => s.IdUser)
                    .FirstOrDefault();
                if (userId != null)
                {
                    User? user = _unitOfWork.UsersRep.GetByIndex((Guid)userId);
                    if (user != null)
                    {
                        return RedirectToAction("UserAccount", "UserAccount", new
                        {
                            UserId = user.IdUser, Email = user.Email, Password = user.Password, Role = user.Role,
                            Surname = user.Surname, Name = user.Name, Patronymic = user.Patronymic,
                            PhoneNumber = user.PhoneNumber
                        });
                    }
                }
            }

            return View();
        }#1#

        /#1#// <summary>
        /// Регистрация организации, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrganizationRegister(Guid userId, string email, string password, Guid role,
            string surname, string name, string patronymic, string phoneNumber)
        {
            _user = new User(userId, email, password, role, surname, name, patronymic, phoneNumber);
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
                Organization organization = new Organization();
                organization = _organizationManager.RegisterToDb(organizationForView);
                _unitOfWork.OrganizationsRep.Create(organization);
                _unitOfWork.Save();

                _user.IdOrganization = organization.IdOrganization;
                _unitOfWork.UsersRep.Create(_user);
                _unitOfWork.Save();
                return RedirectToAction("OrganizationAccount", "Account", new
                {
                    OrganizationId = organization.IdOrganization,
                    UserId = _user.IdUser,
                    Role = _user.IdRole
                });
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
        }#1#*/


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
            /*_userManager.Dispose();
            _organizationManager.Dispose();*/
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}