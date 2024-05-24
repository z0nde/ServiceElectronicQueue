using System.Text.Json;
using System.Text.Json.Serialization;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithOrganization
{
    public class ParserTransmittingGetDataContainerWithOrganization
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options;

        public ParserTransmittingGetDataContainerWithOrganization(IHttpContextAccessor httpContextAccessor)
        {
            _options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _httpContextAccessor = httpContextAccessor;
        }

        public (DataComeFrom, User, Organization) ParseDeserialize(string jsonUserUrl, string jsonOrgUrl)
        {
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, _options);
            UserUrl userUrl = JsonSerializer.Deserialize<UserUrl>(jsonUserUrl, _options)!;
            UserHttp userHttp = JsonSerializer.Deserialize<UserHttp>(_httpContextAccessor.HttpContext!.Session.GetString("UserDataHttp")!, _options)!;
            OrganizationUrl orgUrl = JsonSerializer.Deserialize<OrganizationUrl>(jsonOrgUrl, _options)!;
            OrganizationHttp orgHttp = JsonSerializer.Deserialize<OrganizationHttp>(_httpContextAccessor.HttpContext!.Session.GetString("OrganizationData")!, _options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            return (
                userAuthStatusPost, 
                new User(userHttp.IdUser, userUrl.Email, userHttp.Password, userHttp.IdRole, 
                    userUrl.Surname, userUrl.Name, userUrl.Patronymic, userUrl.PhoneNumber),
                new Organization(orgHttp.IdOrganization, orgUrl.Email, orgHttp.Password, 
                    orgUrl.Title, orgHttp.UniqueKey, orgHttp.Logo)
            );
        }

        public void ParseSerialize(DataComeFrom userAuthStatusPost, User user, Organization organization)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(user, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("OrganizationData", JsonSerializer.Serialize(organization, _options));
        }
    }
}