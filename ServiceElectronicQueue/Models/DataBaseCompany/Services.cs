using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Services
    {
        [Key] public Guid IdServices { get; set; }
        public string Service { get; set; }

        public Guid IdBranchOffice { get; set; }
        public BranchOffice BranchOffices { get; set; }

        public ICollection<ElectronicQueue> ElectronicQueues { get; set; }
    }
}