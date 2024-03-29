using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.ForViews;

public class OrganizationRegisterForView
{
    [DisplayName("Почта")]
    public string? Email { get; set; }
    
    [DisplayName("Пароль")] 
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DisplayName("Название организации")]
    public string? Title { get; set; }
    
    [DisplayName("Логотип")]
    [DataType(DataType.ImageUrl)]
    public string? Logo { get; set; }
}