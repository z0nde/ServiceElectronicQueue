using System.Text.Json;
using System.Text.Json.Serialization;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice
{
    public class ParserTransmittingPostDataContainerWithBranchOffice
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options;

        public ParserTransmittingPostDataContainerWithBranchOffice(IHttpContextAccessor httpContextAccessor)
        {
            _options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _httpContextAccessor = httpContextAccessor;
        }

        public (DataComeFrom, User, BranchOffice) ParseDeserialize()
        {
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, _options);
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, _options)!;
            BranchOffice brOffice = JsonSerializer.Deserialize<BranchOffice>(_httpContextAccessor.HttpContext!.Session.GetString("BrOfficeData")!, _options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            return (userAuthStatusPost, user, brOffice);
        }
        
        public (string, string) ParseSerialize(DataComeFrom userAuthStatusPost, User user, BranchOffice brOffice)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, _options));
            
            
            string jsonUserHttp = JsonSerializer.Serialize(
                new UserHttp(user.IdUser, user.Password, user.IdRole),
                _options
            );
            _httpContextAccessor.HttpContext.Session.SetString("UserDataHttp", jsonUserHttp);
            string jsonUserUrl = JsonSerializer.Serialize(
                new UserUrl(user.Email, user.Surname, user.Name, user.Patronymic, user.PhoneNumber),
                _options
            );
            
            string jsonOrganizationHttp = JsonSerializer.Serialize(
                new BranchOfficeHttp(
                    brOffice.IdBranchOffice, brOffice.Password, brOffice.UniqueLink, brOffice.IdOrganization),
                _options
            ); 
            _httpContextAccessor.HttpContext!.Session.SetString("BrOfficeDataHttp", jsonOrganizationHttp);
            string jsonOrganizationUrl = JsonSerializer.Serialize(
                new OrganizationUrl(brOffice.Email, brOffice.Addres)
            );

            return (jsonUserUrl, jsonOrganizationUrl);
        }
    }
}