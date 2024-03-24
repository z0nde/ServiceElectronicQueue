using System.ComponentModel.DataAnnotations;
using ServiceElectronicQueue.Models.ForViews;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Organization : IModelViewToDb<Organization, OrganizationForView>
    {
        [Key] public Guid IdOrganization { get; set; }
        public string Title { get; set; }
        public byte[]? Logo { get; set; }

        public Organization() =>
            IdOrganization = Guid.NewGuid();

        public Organization(string title, byte[]? logo) =>
            (IdOrganization, Title, Logo) = (Guid.NewGuid(), title, logo);
        
        public virtual ICollection<BranchOffice> Branches { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Organization ToDb(OrganizationForView view)
        {
            return new Organization(view.Title!, null);
        }
    }
}