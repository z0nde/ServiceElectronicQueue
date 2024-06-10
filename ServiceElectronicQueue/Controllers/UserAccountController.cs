using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;

namespace ServiceElectronicQueue.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UnitOfWorkCompany _unitOfWork;

        private readonly UserManager _userManager;
        private User _user;

        public UserAccountController(CompanyDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(dbContext);
            _user = new User();
        }


        /// <summary>
        /// Аккаунт пользователя, GET
        /// Параметры для перенаправления и отображения данных о пользователе
        /// </summary>
        /// <param name="jsonUserUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserAccount(string jsonUserUrl)
        {
            // получение и десериализация данных из предыдущего контроллера
            ParserTransmittingGetDataContainer container = new ParserTransmittingGetDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize(jsonUserUrl);
            
            var model = new UserAccountForView
            {
                Name = _user.Name,
                Patronymic = _user.Patronymic,
                Role = _unitOfWork.RoleRep.GetAll()
                    .Where(s => s.IdRole == _user.IdRole)
                    .Select(s => s.Amplua).FirstOrDefault()
            };
            
            container.ParseSerialize(userAuthStatusPost, _user);
            return View(model);
        }

        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу регистрации
        /// аккаунта организации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountRegisterOrganization()
        {
            ParserTransmittingPostDataContainer container = new ParserTransmittingPostDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize();
            
            string jsonUserUrl = container.ParseSerialize(userAuthStatusPost, _user);
            return RedirectToAction("OrganizationRegister", "OrganizationAuth", new { jsonUserUrl });
        }

        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу входа в аккаунт организации
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountLoginOrganization()
        {
            ParserTransmittingPostDataContainer container = new ParserTransmittingPostDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize();
            
            string jsonUserUrl = container.ParseSerialize(userAuthStatusPost, _user);
            return RedirectToAction("OrganizationLogin", "OrganizationAuth", new { jsonUserUrl });
        }


        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу регистрации аккаунта филиала
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountRegisterBranchOffice()
        {
            ParserTransmittingPostDataContainer container = new ParserTransmittingPostDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize();
            
            string jsonUserUrl = container.ParseSerialize(userAuthStatusPost, _user);
            return RedirectToAction("BranchOfficeRegister", "BranchOfficeAuth", new {jsonUserUrl});
        }

        /// <summary>
        /// Аккаунт пользователя, POST
        /// Метод, используемый в качестве конпки для пользователя, перенаправляющий на страницу входа в аккаунт филиала
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserAccountLoginBranchOffice()
        {
            ParserTransmittingPostDataContainer container = new ParserTransmittingPostDataContainer(_httpContextAccessor);
            (DataComeFrom userAuthStatusPost, _user) = container.ParseDeserialize();
            
            string jsonUserUrl = container.ParseSerialize(userAuthStatusPost, _user);
            return RedirectToAction("BranchOfficeLogin", "BranchOfficeAuth", new {jsonUserUrl});
        }


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}