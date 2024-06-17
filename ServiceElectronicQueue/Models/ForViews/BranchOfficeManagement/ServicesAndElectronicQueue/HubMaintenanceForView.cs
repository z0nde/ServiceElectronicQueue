using System.ComponentModel;

namespace ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement.ServicesAndElectronicQueue;

public class HubMaintenanceForView
{
    [DisplayName("Статус")]
    public string Status { get; set; }
    
    [DisplayName("Номер")]
    public string NumberQueue { get; set; }
}