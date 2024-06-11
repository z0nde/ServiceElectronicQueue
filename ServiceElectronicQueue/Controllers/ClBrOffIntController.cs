using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement.ServicesAndElectronicQueue;
using ServiceElectronicQueue.Models.JsonModels;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;
using ServiceElectronicQueue.Models.KafkaQueue;

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
        /// <param name="guidBrOffice"></param>
        /// <returns></returns>
        public IActionResult ClientServiceDisplay(Guid BrOffCli)
        {
            //todo "сделать проверки";
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

        public IActionResult RecordToQueue(int numberService)
        {
            Guid idBrOffice = JsonSerializer.Deserialize<Guid>(_httpContextAccessor.HttpContext!.Session
                .GetString("ClientRecordToQueue")!);
            _httpContextAccessor.HttpContext.Session.Clear();

            ServiceSector? serviceSector = _unitOfWork.ServicesRep.GetAll()
                .FirstOrDefault(s => s.NumberService == numberService && s.IdBranchOffice == idBrOffice);

            if (serviceSector != null)
            {
                var producer = new ProducerQueueService(KafkaFactory.CreateProducer(
                    new ConfigProducer(JsonSerializer.Serialize(idBrOffice))), ConfigKafka.Topic); 
            
                producer.PostMessage(JsonSerializer.Serialize(new KafkaQuery(
                    Rand.Str(2), numberService, serviceSector.Service)));
            
            
            
                var serviceClientUrl = JsonSerializer.Serialize(
                    new ServiceDisplayQueueUrl{IdBrOffice = idBrOffice, NumberService = numberService});
                return RedirectToAction("DisplayQueue", new { serviceClientUrl});
            }

            Guid BrOffCli = idBrOffice;
            return RedirectToAction("ClientServiceDisplay", new { BrOffCli });
        }
        
        
        public IActionResult DisplayQueue(string serviceClientUrl)
        {
            return NoContent();
        }



        /*public IActionResult GetUrlLink(string idBrOffice)
        {
            string url = 
                $"https://{Request.Host}{Request.PathBase}/ClBrOffIntController/ClientServiceDisplay?idBrOffice: {idBrOffice}";
            return RedirectToAction();
        }*/
        
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}