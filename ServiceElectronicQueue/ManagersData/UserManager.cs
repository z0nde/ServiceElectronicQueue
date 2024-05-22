using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.ManagersData
{
    public class UserManager : IDisposable
    {
        private readonly UnitOfWorkCompany _unitOfWork;

        public UserManager(UnitOfWorkCompany unitOfWork) =>
            _unitOfWork = unitOfWork;
        
        public UserRegisterForView? CheckRegisterModel(UserRegisterForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null, Role: not null, Surname: not null, Name: not null,
                Patronymic: not null, PhoneNumber: not null
            } ? obj : null;
        }

        public UserLoginForView? CheckLoginModel(UserLoginForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null
            } ? obj : null;
        }

        public User RegisterToDb(UserRegisterForView obj)
        {
            return new User(
                obj.Email, 
                obj.Password, 
                _unitOfWork.RoleRep
                    .GetAll()
                    .Where(s => s.Amplua == obj.Role)
                    .Select(s => s.IdRole)
                    .FirstOrDefault(),
                obj.Surname,
                obj.Name, 
                obj.Patronymic,
                obj.PhoneNumber);
        }

        public User LoginToDb(UserLoginForView obj)
        {
            return new User(obj.Email, obj.Password);
        }

        

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}