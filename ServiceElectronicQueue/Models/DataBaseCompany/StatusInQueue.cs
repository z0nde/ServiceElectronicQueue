using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany;

public class StatusInQueue
{
    [Key]
    public Guid IdStatus { get; set; }
    
    public string Status { get; set; }
}