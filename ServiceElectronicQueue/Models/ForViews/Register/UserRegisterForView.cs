using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceElectronicQueue.Models.ForViews.Register;

public class UserRegisterForView
{
    [DisplayName("Электронная почта")]
    public string? Email { get; set; }
    
    [DisplayName("Пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DisplayName("Повторите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string? ConfirmPassword { get; set; }


    /*public int SelectRoleItem { get; set; }
    
    [DisplayName("Роли")] 
    public IEnumerable<SelectListItem> RoleItems { get; set; }*/
    
    [DisplayName("Роль")]
    public string Role { get; set; }
    
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