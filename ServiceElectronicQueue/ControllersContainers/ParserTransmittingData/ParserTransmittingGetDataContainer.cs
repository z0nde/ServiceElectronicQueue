using System.Text.Json;
using System.Text.Json.Serialization;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.JsonModels.TransmittingHttp;
using ServiceElectronicQueue.Models.JsonModels.TransmittingUrl;

namespace ServiceElectronicQueue.ControllersContainers.ParserTransmittingData
{
    public class ParserTransmittingGetDataContainer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options;

        public ParserTransmittingGetDataContainer(IHttpContextAccessor httpContextAccessor)
        {
            _options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual (DataComeFrom, User) ParseDeserialize(string jsonUserUrl)
        {
            DataComeFrom userAuthStatusPost = JsonSerializer.Deserialize<DataComeFrom>(_httpContextAccessor.HttpContext!.Session.GetString("UserAuthStatus")!, _options);
            UserUrl userUrl = JsonSerializer.Deserialize<UserUrl>(jsonUserUrl, _options)!;
            UserHttp userHttp = JsonSerializer.Deserialize<UserHttp>(_httpContextAccessor.HttpContext!.Session.GetString("UserDataHttp")!, _options)!;
            _httpContextAccessor.HttpContext.Session.Clear();
            return (
                userAuthStatusPost, 
                new User(userHttp.IdUser, userUrl.Email, userHttp.Password, userHttp.IdRole, 
                    userUrl.Surname, userUrl.Name, userUrl.Patronymic, userUrl.PhoneNumber));
        }

        public virtual void ParseSerialize(DataComeFrom userAuthStatusPost, User user)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("UserAuthStatus",
                JsonSerializer.Serialize(userAuthStatusPost, _options));
            _httpContextAccessor.HttpContext!.Session.SetString("UserData", JsonSerializer.Serialize(user, _options));
        }
    }
}