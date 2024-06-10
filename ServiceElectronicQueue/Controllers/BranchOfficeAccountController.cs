using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServiceElectronicQueue.Controllers
{
    public class BranchOfficeAccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;
        private readonly BranchOfficeManager _branchOfficeManager;
        private BranchOffice _branchOffice;
        private User _user;
        
        public BranchOfficeAccountController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(db);
            _branchOfficeManager = new BranchOfficeManager(_unitOfWork);
            _branchOffice = new BranchOffice();
            _user = new User();
        }
        
        [HttpGet]
        public IActionResult BranchOfficeAccount(string jsonUserUrl, string jsonBrOfficeUrl)
        {
            var containerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize(jsonUserUrl, jsonBrOfficeUrl);

            var model = new BranchOfficeAccountForView
            {
                TitleOrganization = _unitOfWork.OrganizationsRep.GetAll()
                    .Where(s => s.IdOrganization == _branchOffice.IdOrganization)
                    .Select(s => s.Title).FirstOrDefault(),
                Addres = _branchOffice.Addres,
                Surname = _user.Surname,
                Name = _user.Name,
                Patronymic = _user.Patronymic
            };
            
            containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Services()
        {
            var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();

            var idBrOffice = _branchOffice.IdBranchOffice;
            return RedirectToAction("ServicesDisplay", "Services", new {idBrOffice});
        }

        [HttpPost]
        public IActionResult GenerateUniqueLink()
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = container.ParseDeserialize();

            string idBrOffice = JsonSerializer.Serialize(_branchOffice.IdBranchOffice);
            return RedirectToAction("GetUrlLink", "ClBrOffInt", new {idBrOffice});
        }
        
        

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}