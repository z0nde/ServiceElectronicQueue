﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class User
    {
        [Key] 
        public Guid IdUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        
        
        public Guid IdOrganization { get; set; }
        public Guid IdRole { get; set; }
        
        
        [ForeignKey("IdOrganization")]
        public virtual Organization Organization { get; set; }
        
        [ForeignKey("IdRole")]
        public virtual Role Role { get; set; }
        
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
        
        public void SetPropertiesWithoutIdOrganizations(Guid idUser, string email, string password, Guid roleId, string surname, string name, 
            string patronymic, string phoneNumber) =>
            (IdUser, Email, Password, IdRole, Surname, Name, Patronymic, PhoneNumber) =
            (idUser, email, password, roleId, surname, name, patronymic, phoneNumber);

        /*public User(Guid idUser, string email, string password, Guid roleId, Role role, string surname, string name, 
            string patronymic, string phoneNumber) =>
            (IdUser, Email, Password, IdRole, Role, Surname, Name, Patronymic, PhoneNumber) =
            (idUser, email, password, roleId, role, surname, name, patronymic, phoneNumber);*/

        /*public User ToDb(UserRegisterForView view)
        {
            return new User(view.Email!, view.Password!, view.Role!, 
                view.Surname!, view.Name!, view.Patronymic!, view.PhoneNumber!);
        }*/

        //public override string ToString() => $"{Email}, {Password}, {Role}, {Surname}, {Name}, {Patronymic}, {PhoneNumber}";
    }
}