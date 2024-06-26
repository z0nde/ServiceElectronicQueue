using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class ElectronicQueue
    {
        [Key] 
        public Guid IdElectronicQueue { get; set; }
        public string NumberInQueue { get; set; }
        public DateTime PendingServiceDateTime { get; set; }
        public DateTime ReadyServiceDateTime { get; set; }
        public DateTime StartServiceDateTime { get; set; }
        public DateTime EndServiceDateTime { get; set; }
        
        public Guid IdStatus { get; set; }
        [ForeignKey("IdStatus")]
        public virtual StatusInQueue StatusInQueue { get; set; }
        
        public Guid IdServices { get; set; }
        [ForeignKey("IdServices")]
        public virtual ServiceSector ServiceSector { get; set; }

        public ElectronicQueue()
        { }
    }
}