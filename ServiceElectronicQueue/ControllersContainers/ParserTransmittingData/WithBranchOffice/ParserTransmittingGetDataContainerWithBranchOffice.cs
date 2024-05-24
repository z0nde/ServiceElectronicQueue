using System.Text.Json;
using System.Text.Json.Serialization;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.ControllersContainers.ParserTransmittingData.WithBranchOffice
{
    public class ParserTransmittingGetDataContainerWithBranchOffice
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options;

        public ParserTransmittingGetDataContainerWithBranchOffice(IHttpContextAccessor httpContextAccessor)
        {
            _options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _httpContextAccessor = httpContextAccessor;
        }
        
        public (DataComeFrom, User, BranchOffice) ParseDeserialize(string jsonUserUrl, string jsonBrOfficeUrl)
        {
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, _options);
            UserUrl userUrl = JsonSerializer.Deserialize<UserUrl>(jsonUserUrl, _options)!;
            UserHttp userHttp = JsonSerializer.Deserialize<UserHttp>(_httpContextAccessor.HttpContext!.Session.GetString("UserDataHttp")!, _options)!;
            BranchOfficeUrl brOfficeUrl = JsonSerializer.Deserialize<BranchOfficeUrl>(jsonBrOfficeUrl, _options)!;
            BranchOfficeHttp brOfficeHttp = JsonSerializer.Deserialize<BranchOfficeHttp>(_httpContextAccessor.HttpContext!.Session.GetString("BrOfficeDataHttp")!, _options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            
            return (
                userAuthStatusPost, 
                new User(userHttp.IdUser, userUrl.Email, userHttp.Password, userHttp.IdRole, 
                    userUrl.Surname, userUrl.Name, userUrl.Patronymic, userUrl.PhoneNumber),
                new BranchOffice(brOfficeHttp.IdBranchOffice, brOfficeUrl.Email, brOfficeHttp.Password, 
                    brOfficeUrl.Addres, brOfficeHttp.UniqueLink, brOfficeHttp.IdOrganization)
            );
        }

        public void ParseSerialize(DataComeFrom userAuthStatusPost, User user, BranchOffice brOffice)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(user, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("BrOfficeData", JsonSerializer.Serialize(brOffice, _options));
        }
    }
}