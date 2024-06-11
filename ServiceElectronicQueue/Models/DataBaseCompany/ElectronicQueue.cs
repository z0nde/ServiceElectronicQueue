using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class ElectronicQueue
    {
        [Key] 
        public Guid IdElectronicQueue { get; set; }
        public string NumberInQueue { get; set; }
        public string Status { get; set; }
        public DateTime DateAndTimeStatus { get; set; }
        
        public Guid IdServices { get; set; }
        
        [ForeignKey("IdServices")]
        public virtual ServiceSector ServiceSector { get; set; }

        public ElectronicQueue()
        { }

        public ElectronicQueue(Guid idElectronicQueue, string numberService, string status, DateTime dateTime) =>
            (IdElectronicQueue, NumberInQueue, Status, DateAndTimeStatus) =
            (idElectronicQueue, numberService, status, dateTime);
        
        public ElectronicQueue(Guid idElectronicQueue, string numberService, string status, DateTime dateTime, Guid idServices) =>
            (IdElectronicQueue, NumberInQueue, Status, DateAndTimeStatus, IdServices) =
            (idElectronicQueue, numberService, status, dateTime, idServices);
        
        public void SetProperties(Guid idElectronicQueue, string numberService, string status, DateTime dateTime) =>
            (IdElectronicQueue, NumberInQueue, Status, DateAndTimeStatus) =
            (idElectronicQueue, numberService, status, dateTime);
        
        public void SetProperties(Guid idElectronicQueue, string numberService, string status, DateTime dateTime, Guid idServices) =>
            (IdElectronicQueue, NumberInQueue, Status, DateAndTimeStatus, IdServices) =
            (idElectronicQueue, numberService, status, dateTime, idServices);
    }
}