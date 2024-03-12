using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBase;

public class User
{
    [Key]
    public Guid IdUser { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }

    public User() => IdUser = Guid.NewGuid();

    public User(string email, string password) => 
        (IdUser, Email, Password) = 
        (Guid.NewGuid(), email, password);

    public User(string email, string password, string role, string surname, string name, string patronymic,
        string phoneNumber) =>
        (IdUser, Email, Password, Role, Surname, Name, Patronymic, PhoneNumber) =
        (Guid.NewGuid(), email, password, role, surname, name, patronymic, phoneNumber);
    
    public Guid IdOrganization { get; set; }
    public virtual Organization Organization { get; set; }
}