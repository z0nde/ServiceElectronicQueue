using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.ManagersData
{
    public class BranchOfficeManager
    {
        private readonly UnitOfWorkCompany _unitOfWork;

        public BranchOfficeManager(UnitOfWorkCompany unitOfWork) =>
            _unitOfWork = unitOfWork;

        public BranchOfficeRegisterForView? CheckRegisterModel(BranchOfficeRegisterForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null, Addres: not null, UniqueKeyOrganization: not null
            } ? obj : null;
        }

        public BranchOfficeLoginForView? CheckLoginModel(BranchOfficeLoginForView? obj)
        {
            return obj is
            {
                Email: not null, Password: not null
            } ? obj : null;
        }
    }
}