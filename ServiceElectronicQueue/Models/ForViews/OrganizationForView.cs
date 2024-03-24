using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.ForViews;

public class OrganizationForView
{
    [DisplayName("Название организации")]
    public string? Title { get; set; }
    
    [DisplayName("Логотип")]
    [DataType(DataType.ImageUrl)]
    public string? Logo { get; set; }
}