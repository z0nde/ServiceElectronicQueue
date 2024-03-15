using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.ForViews;

public class UserForView
{
    [DisplayName("Электронная почта")]
    public string? Email { get; set; }
    
    [DisplayName("Пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [DisplayName("Роль")]
    public string? Role { get; set; }
    
    [DisplayName("Фамилия")]
    public string? Surname { get; set; }
    
    [DisplayName("Имя")]
    public string? Name { get; set; }
    
    [DisplayName("Отчество")]
    public string? Patronymic { get; set; }
    
    [DisplayName("Номер телефона")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
}