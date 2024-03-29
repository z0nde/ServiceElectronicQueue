using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceElectronicQueue.Models.ForViews.Login
{
    public class UserLoginForView
    {
        [DisplayName("Электронная почта")] 
        public string? Email { get; set; }

        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}