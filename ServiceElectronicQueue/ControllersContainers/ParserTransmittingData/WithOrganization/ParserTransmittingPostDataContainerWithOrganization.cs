using System.Text.Json;
using System.Text.Json.Serialization;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithOrganization
{
    public class ParserTransmittingPostDataContainerWithOrganization
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options;

        public ParserTransmittingPostDataContainerWithOrganization(IHttpContextAccessor httpContextAccessor)
        {
            _options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _httpContextAccessor = httpContextAccessor;
        }

        public (DataComeFrom, User, Organization) ParseDeserialize()
        {
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, _options);
            User user = JsonSerializer.Deserialize<User>(_httpContextAccessor.HttpContext!.Session.GetString("UserData")!, _options)!;
            Organization organization = JsonSerializer.Deserialize<Organization>(_httpContextAccessor.HttpContext!.Session.GetString("OrganizationData")!, _options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            return (userAuthStatusPost, user, organization);
        }
        
        public (string, string) ParseSerialize(DataComeFrom userAuthStatusPost, User user, Organization organization)
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
                new OrganizationHttp(
                    organization.IdOrganization, organization.Password, organization.UniqueKey, organization.Logo),
                _options
            ); 
            _httpContextAccessor.HttpContext!.Session.SetString("OrganizationData", jsonOrganizationHttp);
            string jsonOrganizationUrl = JsonSerializer.Serialize(
                new OrganizationUrl(organization.Email, organization.Title)
            );

            return (jsonUserUrl, jsonOrganizationUrl);
        }
    }
}