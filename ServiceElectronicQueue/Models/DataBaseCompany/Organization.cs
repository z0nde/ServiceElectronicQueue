using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBase;

public class Organization
{
    [Key]
    public Guid IdOrganization { get; set; }
    public string Title { get; set; }
    public byte[]? Logo { get; set; }

    public virtual ICollection<BranchOffice> Branches { get; set; }
    public virtual ICollection<User> Users { get; set; }
}