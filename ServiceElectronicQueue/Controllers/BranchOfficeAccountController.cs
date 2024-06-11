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

            var idBrOffice = _branchOffice.IdBranchOffice;
            return RedirectToAction("ServicesDisplay", "Services", new {idBrOffice});
        }

        [HttpPost]
        public IActionResult GenerateUniqueLink()
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = container.ParseDeserialize();

            Guid idBrOffice = _branchOffice.IdBranchOffice;
            
            _branchOffice.UniqueLink = $"https://{Request.Host}{Request.PathBase}/ClBrOffIntController/ClientServiceDisplay?BrOffCli=\"{idBrOffice}\"";
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
            
            
            (string jsonUserUrl, string jsonBrOfficeUrl) = container.ParseSerialize(userAuthStatus, _user, _branchOffice);
            return RedirectToAction("ElectronicQueueClients", new {jsonUserUrl, jsonBrOfficeUrl});
        }
        
        
        
        [HttpGet]
        public IActionResult ElectronicQueueClients(string jsonUserUrl, string jsonBrOfficeUrl)
        {
            var containerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, _user, _branchOffice) = containerWithBranchOffice.ParseDeserialize(jsonUserUrl, jsonBrOfficeUrl);
            var idBrOffice = _branchOffice.IdBranchOffice;
            
            StatusesPages.CurrentPage = 1;
            var configConsumer = new ConfigConsumer(JsonSerializer.Serialize(_branchOffice.IdBranchOffice));
            var consumer = KafkaFactory.CreateConsumer(configConsumer);
            var consumerQueueService = new ConsumerQueueService(consumer, ConfigKafka.Topic);
            
            
            //todo "стереть. использовать SignalR и JS, данная реализация не работает ввиду того, что в фоновом потоке нет HTTP";
            
            
            List<KafkaQuery> kafkaqueries = new();
            if (StatusesPages.FlagKafkaPage)
            {
                Task.Run(() =>
                {
                    consumer.Subscribe(ConfigKafka.Topic);
                    while (StatusesPages.CurrentPage == 1)
                    {
                        string? message = consumerQueueService.GetMessage();
                        if (message != null)
                            kafkaqueries.Add(JsonSerializer.Deserialize<KafkaQuery>(message)!);
                    }
                    StatusesPages.FlagKafkaPage = false;
                    Thread.Sleep(5000);
                    return RedirectToAction("ElectronicQueueClients", new {idBrOffice});
                });
            }
            if (StatusesPages.FlagKafkaPage == false)
                StatusesPages.FlagKafkaPage = true;
            return View(kafkaqueries);
        }
        

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}