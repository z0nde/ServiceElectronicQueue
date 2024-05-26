using System.ComponentModel;

namespace ServiceElectronicQueue.Models.ForViews.Account;

public class BranchOfficeAccountForView
{
    [DisplayName("Название организации")]
    public string? TitleOrganization { get; set; }
    
    [DisplayName("Адрес филиала")]
    public string? Addres { get; set; }
    
    [DisplayName("Фамилия")]
    public string? Surname { get; set; }
    
    [DisplayName("Имя")]
    public string? Name { get; set; }
    
    [DisplayName("Отчество")]
    public string? Patronymic { get; set; }
    
    [DisplayName("Уникальная ссылка филиала")]
    public string? UniqueLink { get; set; }
}