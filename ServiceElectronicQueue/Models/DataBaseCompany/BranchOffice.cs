using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class BranchOffice
    {
        [Key] 
        public Guid IdBranchOffice { get; set; }
        public string Addres { get; set; }
        public string UniqueKey { get; set; }

        public Guid IdOrganization { get; set; }
        public Organization Organization { get; set; }
        
        public ICollection<Services> Services { get; set; }
    }
}