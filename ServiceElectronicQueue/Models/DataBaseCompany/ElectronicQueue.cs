using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        [ForeignKey("IdServices")]
        public virtual Services Services { get; set; }

        public ElectronicQueue()
        { }
    }
}