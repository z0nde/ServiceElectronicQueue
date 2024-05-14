using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class ElectronicQueue
    {
        [Key] 
        public Guid IdElectronicQueue { get; set; }
        public string NumberService { get; set; }
        public DateTime StartService { get; set; }
        public DateTime EndService { get; set; }

        public Guid IdServices { get; set; }
        public virtual Services Services { get; set; }

        public ElectronicQueue()
        { }
    }
}