using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.BranchOfficeManagement;

namespace ServiceElectronicQueue.ManagersData
{
    public class ServiceManager
    {
        private readonly UnitOfWorkCompany _unitOfWork;

        public ServiceManager(UnitOfWorkCompany unitOfWork) => 
            _unitOfWork = unitOfWork;

        public ServicesFormForView? CheckModel(ServicesFormForView? obj)
        {
            return obj is
            {
                NumberService: not null, Service: not null
            } ? obj : null;
        }
    }
}