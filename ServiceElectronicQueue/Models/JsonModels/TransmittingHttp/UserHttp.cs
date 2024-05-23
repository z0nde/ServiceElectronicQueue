namespace ServiceElectronicQueue.Models.JsonModels.TransmittingHttp
{
    public class UserHttp
    {
        public Guid IdUser { get; set; }
        public string Password { get; set; }
        public Guid? IdOrganization { get; set; }
        public Guid IdRole { get; set; }

        public UserHttp(Guid idUser, string password, Guid idRole) =>
            (IdUser, Password, IdRole) = (idUser, password, idRole);

        public void SetPropertiesWithoutIdOrganization(Guid idUser, string password, Guid idRole) =>
            (IdUser, Password, IdRole) = (idUser, password, idRole);
    }
}