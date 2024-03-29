using ServiceElectronicQueue.DataCheck.Interfaces;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;

namespace ServiceElectronicQueue.Containers.CompanyDB
{
    public class ManagerCreate<T, T1>
        where T : class //UserForViews
        where T1 : class //User
    {
        private readonly IDataCheck<T> _check;
        private readonly T _obj;
        private readonly IModelViewToDb<T1, T> _toDb;
        private readonly UnitOfWorkCompany _unitOfWorkCompany;

        public ManagerCreate(IDataCheck<T> check, T obj, IModelViewToDb<T1, T> toDb, UnitOfWorkCompany unitOfWorkCompany) =>
            (_check, _obj, _toDb, _unitOfWorkCompany) = 
            (check, obj, toDb, unitOfWorkCompany);

        public void MakeContainer()
        {
            
        }
    }
}