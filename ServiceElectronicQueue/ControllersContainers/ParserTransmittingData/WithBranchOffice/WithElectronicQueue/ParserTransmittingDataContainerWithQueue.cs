using System.Text.Json;
using System.Text.Json.Serialization;
using ServiceElectronicQueue.Models.DataBaseCompany;

namespace ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice.WithElectronicQueue
{
    public class ParserTransmittingDataContainerWithQueue
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options;

        public ParserTransmittingDataContainerWithQueue(IHttpContextAccessor httpContextAccessor)
        {
            _options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _httpContextAccessor = httpContextAccessor;
        }
        
        public (User, BranchOffice, ElectronicQueue) ParseDeserializePost()
        {
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, _options)!;
            BranchOffice brOffice = JsonSerializer.Deserialize<BranchOffice>(_httpContextAccessor.HttpContext!.Session.GetString("BrOfficeData")!, _options)!;
            ElectronicQueue elQueue = JsonSerializer.Deserialize<ElectronicQueue>(_httpContextAccessor.HttpContext!.Session.GetString("ElQueueData")!, _options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            return (user, brOffice, elQueue);
        }
        
        public void ParseSerializeGet(User user, BranchOffice brOffice, ElectronicQueue electronicQueue)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(user, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("BrOfficeData", JsonSerializer.Serialize(brOffice, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("ElQueueData", JsonSerializer.Serialize(electronicQueue, _options));
        }
    }
}