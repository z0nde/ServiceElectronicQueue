using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.JsonModels;
using ServiceElectronicQueue.Models.KafkaQueue;

namespace ServiceElectronicQueue.Controllers
{
    public class HubMessageBranchOfficeController : Controller
    {
        /*private readonly IHubContext<KafkaMessageHub> _hubContext;*/
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;
        
        public HubMessageBranchOfficeController(/*IHubContext<KafkaMessageHub> hubContext,*/ IHttpContextAccessor httpContextAccessor,
            CompanyDbContext dbContext)
        {
            /*_hubContext = hubContext;*/
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(dbContext);
        }
        
        [HttpGet]
        public IActionResult ElectronicQueueClients()
        {
            var postDataContainerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) = postDataContainerWithBranchOffice.ParseDeserialize();
            
            var idBrOffice = branchOffice.IdBranchOffice;

            var consumer = new ConsumerQueueService(KafkaFactory.CreateConsumer(
                new ConfigConsumer(JsonConvert.SerializeObject(idBrOffice))), ConfigKafka.Topic);

            var model = consumer.GetAllMessage(idBrOffice);

            var getDataContainerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            getDataContainerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);
            
            return View(model);
        }
        
        [HttpGet]
        public IActionResult ElectronicQueueClientsForAjax()
        {
            var postDataContainerWithBranchOffice = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) = postDataContainerWithBranchOffice.ParseDeserialize();
            
            var idBrOffice = branchOffice.IdBranchOffice;

            var consumer = new ConsumerQueueService(KafkaFactory.CreateConsumer(
                new ConfigConsumer(JsonConvert.SerializeObject(idBrOffice))), ConfigKafka.Topic);

            var model = consumer.GetAllMessage(idBrOffice);

            var getDataContainerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            getDataContainerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);
            
            return Json(model);
        }

        [HttpGet]
        public IActionResult StartService(string numberQueue)
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) = container.ParseDeserialize();
            
            
            
            
            (string jsonUserUrl, string jsonBrOfficeUrl) = container.ParseSerialize(userAuthStatus, user, branchOffice);
            return RedirectToAction("Maintenance", new {jsonUserUrl, jsonBrOfficeUrl});
        }
            
        
        [HttpGet]
        public IActionResult Maintenance(string jsonUserUrl, string jsonBrOfficeUrl)
        {
            var containerWithBranchOffice =
                new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) =
                containerWithBranchOffice.ParseDeserialize(jsonUserUrl, jsonBrOfficeUrl);
            
            containerWithBranchOffice.ParseSerialize(userAuthStatus, user,branchOffice);
            return View();
        }

        [HttpPost]
        public IActionResult EndService()
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) = container.ParseDeserialize();

            //так же пощаманить с датой
            
            (string jsonUserUrl, string jsonBrOfficeUrl) = container.ParseSerialize(userAuthStatus, user, branchOffice);
            return RedirectToAction("ElectronicQueueClients", new {jsonUserUrl, jsonBrOfficeUrl});
        }
        

        /*public IActionResult Details(int id)
        {
            var consumer = new ProducerQueueService(KafkaFactory.CreateProducer(
                new ConfigProducer(JsonSerializer.Serialize(idBrOffice))), ConfigKafka.Topic); 
            var message = _kafkaService.GetMessageById(id);
            return View(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            
            var messages = await _kafkaService.GetMessagesAsync();
            return Json(messages);
        }*/
    }
}