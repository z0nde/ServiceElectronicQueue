using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Services
    {
        [Key] public Guid IdServices { get; set; }
        public string Service { get; set; }

        public Guid IdBranchOffice { get; set; }
        
        [ForeignKey("IdBranchOffice")]
        public virtual BranchOffice BranchOffices { get; set; }

        public virtual ICollection<ElectronicQueue> ElectronicQueues { get; set; }

        public Services()
        { }
    }
}