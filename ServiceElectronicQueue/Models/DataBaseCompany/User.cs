using System.ComponentModel.DataAnnotations;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public sealed class User
    {
        [Key] 
        public Guid IdUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        
        public Guid? IdOrganization { get; set; }
        public Organization? Organization { get; set; }
        
        public Guid IdRole { get; set; }
        public Role Role { get; set; }
        
        public User()
        { }

        public User(string email, string password) =>
            (Email, Password) =
            (email, password);

        public User(string email, string password, Guid roleId, string surname, string name, string patronymic,
            string phoneNumber) =>
            (IdUser, Email, Password, IdRole, Surname, Name, Patronymic, PhoneNumber) =
            (Guid.NewGuid(), email, password, roleId, surname, name, patronymic, phoneNumber);
        
        public User(Guid idUser, string email, string password, Guid roleId, string surname, string name, 
            string patronymic, string phoneNumber) =>
            (IdUser, Email, Password, IdRole, Surname, Name, Patronymic, PhoneNumber) =
            (idUser, email, password, roleId, surname, name, patronymic, phoneNumber);

        

        /*public User ToDb(UserRegisterForView view)
        {
            return new User(view.Email!, view.Password!, view.Role!, 
                view.Surname!, view.Name!, view.Patronymic!, view.PhoneNumber!);
        }*/

        //public override string ToString() => $"{Email}, {Password}, {Role}, {Surname}, {Name}, {Patronymic}, {PhoneNumber}";
    }
}