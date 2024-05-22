using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.ForViews.Register;

public class BranchOfficeRegisterForView
{
    [DisplayName("Почта")] 
    public string? Email { get; set; }

    [DisplayName("Пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [DisplayName("Повторите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string? ConfirmPassword { get; set; }
    
    [DisplayName("Адрес филиала")]
    public string? Addres { get; set; }
    
    [DisplayName("Уникальный ключ организации")]
    public string? UniqueKeyOrganization { get; set; }
}