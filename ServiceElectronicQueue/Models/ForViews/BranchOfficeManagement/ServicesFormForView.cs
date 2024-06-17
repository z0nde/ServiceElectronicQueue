using System.ComponentModel;

namespace ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;

public class ServicesFormForView
{
    [DisplayName("Номер сервиса")]
    public int? NumberService { get; set; }
    
    [DisplayName("Сервис")]
    public string? Service { get; set; }
}