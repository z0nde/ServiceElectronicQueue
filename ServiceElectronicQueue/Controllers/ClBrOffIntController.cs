using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement.ServicesAndElectronicQueue;
using ServiceElectronicQueue.Models.JsonModels;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;
using ServiceElectronicQueue.Models.KafkaQueue;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ServiceElectronicQueue.Controllers
{
    public class ClBrOffIntController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkCompany _unitOfWork;

        public ClBrOffIntController(CompanyDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = new UnitOfWorkCompany(db);
        }

        /// <summary>
        /// Генерируемая ссылка. В метод передаётся guid филиала, далее по филиалу ищется всё информация и записывается
        /// в модель, после чего, отображается в представлении
        /// </summary>
        /// <param name="BrOffCli"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ClientServiceDisplay(Guid BrOffCli)
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            var brOffice = _unitOfWork.BranchesRep.GetAll()
                .FirstOrDefault(s => s.IdBranchOffice == BrOffCli);
            var org = _unitOfWork.OrganizationsRep.GetAll()
                .FirstOrDefault(s => s.IdOrganization == brOffice.IdOrganization);
            var services = _unitOfWork.ServicesRep.GetAll()
                .Where(s => s.IdBranchOffice == BrOffCli).Select(s => s).ToList();

            var model = new ClientDisplayServices
            {
                Organization = org.Title,
                BranchOfficeAddres = brOffice.Addres,
                ServicesFormForViews = services
                    .Select(s => new ServicesFormForView
                        { NumberService = s.NumberService, Service = s.Service })
                    .ToList()
            };

            _httpContextAccessor.HttpContext!.Session.SetString(
                "ClientRecordToQueue", JsonSerializer.Serialize(brOffice.IdBranchOffice));

            return View(model);
        }

        [HttpGet]
        public IActionResult RecordToQueue(int numberService)
        {
            Guid idBrOffice = JsonSerializer.Deserialize<Guid>(_httpContextAccessor.HttpContext!.Session
                .GetString("ClientRecordToQueue")!);
            _httpContextAccessor.HttpContext.Session.Clear();

            ServiceSector? serviceSector = _unitOfWork.ServicesRep.GetAll()
                .FirstOrDefault(s => s.NumberService == numberService && s.IdBranchOffice == idBrOffice);

            if (serviceSector != null)
            {
                var producer = new ProducerQueueService(KafkaFactory.CreateProducer(), ConfigKafka.Topic);
                var numberQueue = Rand.Str(2);
                Guid idElectronicQueue = Guid.NewGuid();
                producer.PostMessage(JsonConvert.SerializeObject(new KafkaMessageClientToBranchOffice(
                        idElectronicQueue, numberQueue, numberService, serviceSector.Service)),
                    JsonConvert.SerializeObject(idBrOffice));

                var brOffice = _unitOfWork.BranchesRep.GetByIndex(idBrOffice);
                var service = _unitOfWork.ServicesRep.GetAll()
                    .FirstOrDefault(
                        s => s.IdBranchOffice == brOffice.IdBranchOffice && s.NumberService == numberService);

                DateTime dateTime = DateTime.UtcNow;
                ElectronicQueue electronicQueue = new ElectronicQueue
                {
                    IdElectronicQueue = idElectronicQueue,
                    NumberInQueue = numberQueue,
                    Status = QueueStatusStatic.Status[0],
                    DateAndTimeStatus = dateTime,
                    IdServices = service.IdServices
                };
                _unitOfWork.ElectronicQueueRep.Create(electronicQueue);
                _unitOfWork.Save();

                _httpContextAccessor.HttpContext!.Session.SetString("ClientDisplayQueue", JsonSerializer.Serialize(
                    new ClientHttp
                    {
                        IdBranchOffice = idBrOffice,
                        IdService = service.IdServices,
                        IdQueue = idElectronicQueue
                    }));

                return RedirectToAction("DisplayQueue");
            }

            Guid BrOffCli = idBrOffice;
            return RedirectToAction("ClientServiceDisplay", new { BrOffCli });
        }


        public IActionResult DisplayQueue()
        {
            ClientHttp clientHttp = JsonSerializer.Deserialize<ClientHttp>(_httpContextAccessor.HttpContext!.Session
                .GetString("ClientDisplayQueue")!)!;
            ElectronicQueue electronicQueue = _unitOfWork.ElectronicQueueRep.GetAll()
                .First(s => s.IdElectronicQueue == clientHttp.IdQueue && s.IdServices == clientHttp.IdService);
            ClientDisplayQueue model = new ClientDisplayQueue
            {
                NumberQueue = electronicQueue.NumberInQueue,
                Status = electronicQueue.Status
            };
            return View(model);
        }
        
        
        [HttpGet]
        [Route("/ClBrOffInt/DisplayQueueForAjax")]
        public IActionResult DisplayQueueForAjax()
        {
            ClientHttp clientHttp = JsonSerializer.Deserialize<ClientHttp>(_httpContextAccessor.HttpContext!.Session
                .GetString("ClientDisplayQueue")!)!;
            ElectronicQueue electronicQueue = _unitOfWork.ElectronicQueueRep.GetAll()
                .First(s => s.IdElectronicQueue == clientHttp.IdQueue && s.IdServices == clientHttp.IdService);
            ClientDisplayQueue model = new ClientDisplayQueue
            {
                NumberQueue = electronicQueue.NumberInQueue,
                Status = electronicQueue.Status
            };
            return Json(model);
        }


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}