using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice;
using ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice.WithElectronicQueue;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;
using ServiceElectronicQueue.Models.JsonModels;
using ServiceElectronicQueue.Models.KafkaQueue;

namespace ServiceElectronicQueue.Controllers
{
    public class HubMessageBranchOfficeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;

        public HubMessageBranchOfficeController(IHttpContextAccessor httpContextAccessor,
            CompanyDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(dbContext);
        }

        [HttpGet]
        public IActionResult ElectronicQueueClients()
        {
            var postDataContainerWithBranchOffice =
                new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) =
                postDataContainerWithBranchOffice.ParseDeserialize();

            var idBrOffice = branchOffice.IdBranchOffice;

            var consumer = new ConsumerQueueService(KafkaFactory.CreateConsumer(
                new ConfigConsumer(JsonConvert.SerializeObject(idBrOffice))), ConfigKafka.Topic);

            /*var items = _unitOfWork.ElectronicQueueRep.GetAll().Where(s => s.Status == QueueStatusStatic.Status[0]).ToList();
            foreach (var item in items)
            {
                CollectionElectronicQueue._allClients.Add(new KafkaMessageClientToBranchOffice
                {
                    NumberQueue = item.NumberInQueue,
                    NumberService = _unitOfWork.ServicesRep.GetAll().Where(s => s.IdServices == item.IdServices).Select(s => s.NumberService).First(),
                    Service = _unitOfWork.ServicesRep.GetAll().Where(s => s.IdServices == item.IdServices).Select(s => s.Service).First(),
                    IdClient = item.IdElectronicQueue
                });
            }*/
            
            consumer.GetAllMessage(JsonConvert.SerializeObject(idBrOffice));
            
            var model = CollectionElectronicQueue._allClients;
            
            var getDataContainerWithBranchOffice =
                new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            getDataContainerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("HubMessageBranchOffice/ElectronicQueueClientsForAjax")]
        public IActionResult ElectronicQueueClientsForAjax()
        {
            var postDataContainerWithBranchOffice =
                new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) =
                postDataContainerWithBranchOffice.ParseDeserialize();

            var idBrOffice = branchOffice.IdBranchOffice;

            var consumer = new ConsumerQueueService(KafkaFactory.CreateConsumer(
                new ConfigConsumer(JsonConvert.SerializeObject(idBrOffice))), ConfigKafka.Topic);

            consumer.GetAllMessage(JsonConvert.SerializeObject(idBrOffice));
            var model = CollectionElectronicQueue._allClients;

            var getDataContainerWithBranchOffice =
                new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            getDataContainerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);

            return Json(model);
        }
        
        
        


        [HttpGet]
        public IActionResult ReadyMaintenance(string numberQueue)
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) = container.ParseDeserialize();

            ElectronicQueue electronicQueue = _unitOfWork.ElectronicQueueRep.GetAll()
                .First(e => e.NumberInQueue == numberQueue);

            DateTime dateTime = DateTime.UtcNow;
            ElectronicQueue newElQueue = new ElectronicQueue
            {
                IdElectronicQueue = Guid.NewGuid(),
                NumberInQueue = electronicQueue.NumberInQueue,
                IdStatus = _unitOfWork.StatusRep.GetAll()
                    .Where(s => s.Status == "Готов к обслуживанию").Select(s => s.IdStatus).First(),
                ReadyServiceDateTime = dateTime,
                IdServices = electronicQueue.IdServices
            };
            _unitOfWork.ElectronicQueueRep.UpdateReadyService(electronicQueue.IdElectronicQueue, newElQueue);
            _unitOfWork.Save();

            ParserTransmittingDataContainerWithQueue
                containerWithQueue = new ParserTransmittingDataContainerWithQueue(_httpContextAccessor);
            containerWithQueue.ParseSerializeGet(user, branchOffice, newElQueue);

            return RedirectToAction("Maintenance");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Maintenance()
        {
            ParserTransmittingDataContainerWithQueue
                containerWithQueue = new ParserTransmittingDataContainerWithQueue(_httpContextAccessor);
            (User user, BranchOffice branchOffice, ElectronicQueue electronicQueue) = containerWithQueue.ParseDeserializePost();

            var model = new Maintenance
            {
                NumberQueue = electronicQueue.NumberInQueue,
                Status = _unitOfWork.StatusRep.GetAll()
                    .Where(s => s.IdStatus == electronicQueue.IdStatus)
                    .Select(s => s.Status).First()
            };
            
            containerWithQueue.ParseSerializeGet(user, branchOffice, electronicQueue);
            return View(model);
        }

        [HttpPost]
        public IActionResult StartService()
        {
            ParserTransmittingDataContainerWithQueue
                containerWithQueue = new ParserTransmittingDataContainerWithQueue(_httpContextAccessor);
            (User user, BranchOffice branchOffice, ElectronicQueue electronicQueue) = containerWithQueue.ParseDeserializePost();

            DateTime dateTime = DateTime.UtcNow;
            ElectronicQueue newElQueue = new ElectronicQueue
            {
                IdElectronicQueue = Guid.NewGuid(),
                NumberInQueue = electronicQueue.NumberInQueue,
                IdStatus = _unitOfWork.StatusRep.GetAll()
                    .Where(s => s.Status == "Начало обслуживания").Select(s => s.IdStatus).First(),
                ReadyServiceDateTime = dateTime,
                IdServices = electronicQueue.IdServices
            };
            _unitOfWork.ElectronicQueueRep.UpdateStartService(electronicQueue.IdElectronicQueue, newElQueue);
            _unitOfWork.Save();
           
            containerWithQueue.ParseSerializeGet(user, branchOffice, newElQueue);
            return RedirectToAction("Maintenance");
        }

        [HttpPost]
        public IActionResult EndService()
        {
            ParserTransmittingDataContainerWithQueue
                containerWithQueue = new ParserTransmittingDataContainerWithQueue(_httpContextAccessor);
            (User user, BranchOffice branchOffice, ElectronicQueue electronicQueue) = containerWithQueue.ParseDeserializePost();

            DateTime dateTime = DateTime.UtcNow;
            ElectronicQueue newElQueue = new ElectronicQueue
            {
                IdElectronicQueue = Guid.NewGuid(),
                NumberInQueue = electronicQueue.NumberInQueue,
                IdStatus = _unitOfWork.StatusRep.GetAll()
                    .Where(s => s.Status == "Конец обслуживания").Select(s => s.IdStatus).First(),
                ReadyServiceDateTime = dateTime,
                IdServices = electronicQueue.IdServices
            };
            _unitOfWork.ElectronicQueueRep.UpdateEndService(electronicQueue.IdElectronicQueue, newElQueue);
            _unitOfWork.Save();

            DataComeFrom userAuthStatus = new DataComeFrom(0);
            var parserTransmittingGetDataContainerWithBranchOffice = new ParserTransmittingGetDataContainerWithBranchOffice(_httpContextAccessor);
            parserTransmittingGetDataContainerWithBranchOffice.ParseSerialize(userAuthStatus, user, branchOffice);
            return RedirectToAction("ElectronicQueueClients");
        }

        [HttpPost]
        public IActionResult ExitToBranchOfficeAccount()
        {
            var container = new ParserTransmittingPostDataContainerWithBranchOffice(_httpContextAccessor);
            (DataComeFrom userAuthStatus, User user, BranchOffice branchOffice) = container.ParseDeserialize();

            (string jsonUserUrl, string jsonBrOfficeUrl) = container.ParseSerialize(userAuthStatus, user, branchOffice);
            return RedirectToAction("BranchOfficeAccount", "BranchOfficeAccount", new { jsonUserUrl, jsonBrOfficeUrl });
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}