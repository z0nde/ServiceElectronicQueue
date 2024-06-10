using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private User _user;

        public UserAuthController(CompanyDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(dbContext);
            _userManager = new UserManager(_unitOfWork);
            //_organizationManager = new OrganizationManager(_unitOfWork);
            _user = new User();
        }


        /// <summary>
        /// Регистрация пользователя, GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserRegister()
        {
            return View();
        }

        /// <summary>
        /// Регистрация пользователя, POST
        /// </summary>
        /// <param name="userRegisterForView"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserRegister(UserRegisterForView userRegisterForView/*, string Role*/)
        {
            if (!ModelState.IsValid)
                return View();
            /*userRegisterForView.Role = Role;*/
            DataComeFrom userAuthStatusPost = new DataComeFrom(1);
            if (_userManager.CheckRegisterModel(userRegisterForView) != null)
            {
                if (userRegisterForView.Role is "Пользователь организации" or "Пользователь филиала")
                {
                    if (_unitOfWork.UsersRep
                            .GetAll()
                            .Where(s => s.Email == userRegisterForView.Email)
                            .Select(s => s)
                            .FirstOrDefault() == null)
                    {
                        _user.SetPropertiesWithoutIdOrganizations(
                            Guid.NewGuid(),
                            userRegisterForView.Email!,
                            userRegisterForView.Password!,
                            _unitOfWork.RoleRep.GetAll().Where(s => s.Amplua == userRegisterForView.Role)
                                .Select(s => s.IdRole).First(),
                            userRegisterForView.Surname!,
                            userRegisterForView.Name!,
                            userRegisterForView.Patronymic!,
                            userRegisterForView.PhoneNumber!
                        );
                        ParserTransmittingPostDataContainer container =
                            new ParserTransmittingPostDataContainer(_httpContextAccessor);
                        string jsonUserUrl = container.ParseSerialize(userAuthStatusPost, _user);
                        
                        return RedirectToAction("UserAccount", "UserAccount", new { jsonUserUrl });
                    }
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

            DataComeFrom userAuthStatusPost = new DataComeFrom(2);
            if (_userManager.CheckLoginModel(userLoginForView) != null)
            {
                Guid? userId = _unitOfWork.UsersRep
                    .GetAll()
                    .ToList()
                    .Where(s => s.Email == userLoginForView.Email && s.Password == userLoginForView.Password)
                    .Select(s => s.IdUser)
                    .FirstOrDefault();
                if (userId != null && userId != Guid.Empty)
                {
                    User? user = _unitOfWork.UsersRep.GetByIndex((Guid)userId);
                    if (user != null!)
                    {
                        ParserTransmittingPostDataContainer container =
                            new ParserTransmittingPostDataContainer(_httpContextAccessor);
                        string jsonUserUrl = container.ParseSerialize(userAuthStatusPost, user);
                        
                        return RedirectToAction("UserAccount", "UserAccount", new { jsonUserUrl });
                    }
                }
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }


        /*[HttpPost]
        public IActionResult ValidationRoles()
        {
            Role role1 = new Role("Пользователь организации");
            Role role2 = new Role("Пользователь филиала");
            _unitOfWork.RoleRep.Create(role1);
            _unitOfWork.Save();
            _unitOfWork.RoleRep.Create(role2);
            _unitOfWork.Save();
            _unitOfWork.Dispose();
            return Ok();
        }*/
    }
}