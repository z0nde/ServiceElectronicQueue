using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.ManagersData;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Account;
using ServiceElectronicQueue.Models.JsonModels;
using ServiceElectronicQueue.Models.KafkaQueue;
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
                Patronymic = _user.Patronymic,
                UniqueLink = _branchOffice.UniqueLink
            };
            
            containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Services()
        {
            var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();

            (string jsonUserUrl, string jsonBrOfficeUrl) = containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return RedirectToAction("ServicesDisplay", "Services", new 
                {jsonUserUrl, jsonBrOfficeUrl});
        }

        [HttpPost]
        public IActionResult GenerateUniqueLink()
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = container.ParseDeserialize();

            Guid idBrOffice = _branchOffice.IdBranchOffice;
            
            _branchOffice.UniqueLink = 
                $"https://{Request.Host}{Request.PathBase}/ClBrOffInt/ClientServiceDisplay?BrOffCli={idBrOffice}";
            _unitOfWork.BranchesRep.Update(_branchOffice.IdBranchOffice, _branchOffice);
            _unitOfWork.Save();

            (string jsonUserUrl, string jsonBrOfficeUrl) = container.ParseSerialize(userAuthStatus, _user, _branchOffice);

            return RedirectToAction("BranchOfficeAccount", "BranchOfficeAccount", new 
                {jsonUserUrl, jsonBrOfficeUrl});
        }

        [HttpPost]
        public IActionResult BranchOfficeToElectronicQueueClients()
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = container.ParseDeserialize();

            var parserTransmittingGetDataContainerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            parserTransmittingGetDataContainerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return RedirectToAction("ElectronicQueueClients", "HubMessageBranchOffice");
        }

        [HttpPost]
        public IActionResult GoToStatistic()
        {
            var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();

            (string jsonUserUrl, string jsonBrOfficeUrl) = containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return RedirectToAction("Statistic", new 
                {jsonUserUrl, jsonBrOfficeUrl});
        }

        [HttpGet]
        public IActionResult Statistic(string jsonUserUrl, string jsonBrOfficeUrl)
        {
            var containerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize(jsonUserUrl, jsonBrOfficeUrl);

            
            
            containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return View();
        }

        [HttpPost]
        public IActionResult GoToAccount()
        {
            var containerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize();

            (string jsonUserUrl, string jsonBrOfficeUrl) = containerWithBranchOffice.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return RedirectToAction("Statistic", new 
                {jsonUserUrl, jsonBrOfficeUrl});
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}