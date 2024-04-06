using System.ComponentModel;

namespace ServiceElectronicQueue.Models.ForViews.Account;

public class OrganizationAccountForView
{
    [DisplayName("Название организации")]
    public string? Title { get; set; }
    
    [DisplayName("Фамилия")]
    public string? Surname { get; set; }
    
    [DisplayName("Имя")]
    public string? Name { get; set; }
    
    [DisplayName("Отчество")]
    public string? Patronymic { get; set; }
}