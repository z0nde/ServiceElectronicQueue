using System.ComponentModel;

namespace ServiceElectronicQueue.Models.ForViews.Account;

public class UserAccountForView
{
    [DisplayName("Имя")]
    public string? Name { get; set; }
    
    [DisplayName("Отчество")]
    public string? Patronymic { get; set; }
}