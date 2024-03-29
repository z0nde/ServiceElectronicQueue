using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.ForViews.Login;

public class OrganizationLoginForView
{
    [DisplayName("Почта")]
    public string? Email { get; set; }
    
    [DisplayName("Пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}