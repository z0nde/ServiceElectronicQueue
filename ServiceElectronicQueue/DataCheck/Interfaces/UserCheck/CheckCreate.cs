using ServiceElectronicQueue.Models.ForViews;

namespace ServiceElectronicQueue.DataCheck.Interfaces.UserCheck
{
    public class CheckCreate : IDataCheck<UserForView>
    {
        public UserForView? Check(UserForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null, Role: not null, Surname: not null, Name: not null,
                Patronymic: not null, PhoneNumber: not null
            } ? obj : null;
        }
    }
}