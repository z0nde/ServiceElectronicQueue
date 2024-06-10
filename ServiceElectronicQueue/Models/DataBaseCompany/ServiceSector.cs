using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class ServiceSector
    {
        [Key] public Guid IdServices { get; set; }
        public int NumberService { get; set; }
        public string Service { get; set; }

        public Guid IdBranchOffice { get; set; }
        
        [ForeignKey("IdBranchOffice")]
        public virtual BranchOffice BranchOffices { get; set; }

        public virtual ICollection<ElectronicQueue> ElectronicQueues { get; set; }

        public ServiceSector()
        { }

        public ServiceSector(Guid idServices, int numberService, string service, Guid idBranchOffice) =>
            (IdServices, NumberService, Service, IdBranchOffice) = (idServices, numberService, service, idBranchOffice);
    }
}