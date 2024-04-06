using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.DataCheck.Interfaces.UserCheck
{
    public class UserManager //: IDataCheck<UserRegisterForView>
    {
        private readonly UnitOfWorkCompany _unitOfWork;
        
        public UserManager(UnitOfWorkCompany unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public UserRegister? CheckRegister(UserRegister? obj)
        {
            return obj is
            {
                Email: not null, Password: not null, Role: not null, Surname: not null, Name: not null,
                Patronymic: not null, PhoneNumber: not null
            } ? obj : null;
        }

        public UserLoginForView? CheckLogin(UserLoginForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null
            } ? obj : null;
        }

        public User RegisterToDb(UserRegister userRegister)
        {
            return new User(
                userRegister.Email!, 
                userRegister.Password!, 
                _unitOfWork.RoleRep
                    .GetAll()
                    .Where(s => s.Amplua == userRegister.Role)
                    .Select(s => s.IdRole)
                    .First(),
                userRegister.Surname!,
                userRegister.Name!, 
                userRegister.Patronymic!,
                userRegister.PhoneNumber!);
        }

        public User LoginToDb(UserLoginForView userLoginForView)
        {
            return new User(userLoginForView.Email!, userLoginForView.Password!);
        }
    }
}