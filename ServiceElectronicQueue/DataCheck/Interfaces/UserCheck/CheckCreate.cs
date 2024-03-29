using ServiceElectronicQueue.Models.ForViews;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.DataCheck.Interfaces.UserCheck
{
    public class CheckCreate : IDataCheck<UserRegisterForView>
    {
        public UserRegisterForView? Check(UserRegisterForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null, Role: not null, Surname: not null, Name: not null,
                Patronymic: not null, PhoneNumber: not null
            } ? obj : null;
        }
    }
}