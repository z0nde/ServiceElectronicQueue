namespace ServiceElectronicQueue.Models.JsonModels.TransmittingUrl
{
    public class UserUrl
    {
        public string Email { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }

        public UserUrl(string email, string surname, string name, string patronymic, string phoneNumber) =>
            (Email, Surname, Name, Patronymic, PhoneNumber) = (email, surname, name, patronymic, phoneNumber);
        
        public void SetProperties(string email, string surname, string name, string patronymic, string phoneNumber) =>
            (Email, Surname, Name, Patronymic, PhoneNumber) = (email, surname, name, patronymic, phoneNumber);
    }
}