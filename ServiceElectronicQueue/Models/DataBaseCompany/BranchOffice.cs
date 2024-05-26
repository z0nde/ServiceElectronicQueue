using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class BranchOffice
    {
        [Key] 
        public Guid IdBranchOffice { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Addres { get; set; }
        public string? UniqueLink { get; set; }

        public Guid IdOrganization { get; set; }
        
        [ForeignKey("IdOrganization")]
        public virtual Organization Organization { get; set; }
        
        public virtual ICollection<Services> Services { get; set; }

        public BranchOffice()
        { }
        
        public BranchOffice(Guid idBranchOffice, string email, string password, string addres,
            string? uniqueLink, Guid idOrganization) =>
            (IdBranchOffice, Email, Password, Addres, UniqueLink, IdOrganization) =
            (idBranchOffice, email, password, addres, uniqueLink, idOrganization);
        
        public void SetProperties(Guid idBranchOffice, string email, string password, string addres,
            string? uniqueLink, Guid idOrganization) =>
            (IdBranchOffice, Email, Password, Addres, UniqueLink, IdOrganization) =
            (idBranchOffice, email, password, addres, uniqueLink, idOrganization);
    }
}