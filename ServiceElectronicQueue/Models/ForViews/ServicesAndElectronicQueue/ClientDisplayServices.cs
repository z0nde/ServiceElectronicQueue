using System.ComponentModel;

namespace ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement.ServicesAndElectronicQueue;

public class ClientDisplayServices
{
    public string? Organization { get; set; }
    public string? BranchOfficeAddres { get; set; }
    public List<ServicesFormForView> ServicesFormForViews { get; set; } = new();
}