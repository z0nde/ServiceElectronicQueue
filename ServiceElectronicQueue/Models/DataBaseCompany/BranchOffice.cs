using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class BranchOffice
    {
        [Key] 
        public Guid IdBranchOffice { get; set; }
        public string Addres { get; set; }
        public string UniqueLink { get; set; }

        public Guid IdOrganization { get; set; }
        public virtual Organization Organization { get; set; }
        
        public virtual ICollection<Services> Services { get; set; }
    }
}