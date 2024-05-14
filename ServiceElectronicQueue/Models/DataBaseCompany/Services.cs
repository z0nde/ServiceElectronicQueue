using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Services
    {
        [Key] public Guid IdServices { get; set; }
        public string Service { get; set; }

        public Guid IdBranchOffice { get; set; }
        public virtual BranchOffice BranchOffices { get; set; }

        public virtual ICollection<ElectronicQueue> ElectronicQueues { get; set; }

        public Services()
        { }
    }
}