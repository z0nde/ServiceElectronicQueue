using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.DataBaseCompany
{
    public class Role
    {
        [Key] public Guid IdRole { get; set; }
        public string Amplua { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role(string role) =>
            (IdRole, Amplua) = (Guid.NewGuid(), role);
    }
}