using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Service
    {
        [Key] public Guid IdServices { get; set; }
        public uint NumberService { get; set; }
        public string Service { get; set; }

        public Guid IdBranchOffice { get; set; }
        
        [ForeignKey("IdBranchOffice")]
        public virtual BranchOffice BranchOffices { get; set; }

        public virtual ICollection<ElectronicQueue> ElectronicQueues { get; set; }

        public Service()
        { }

        public Service(Guid idServices, uint numberService, string service, Guid idBranchOffice) =>
            (IdServices, NumberService, Service, IdBranchOffice) = (idServices, numberService, service, idBranchOffice);
    }
}