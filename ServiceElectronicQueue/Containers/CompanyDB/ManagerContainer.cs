/*using System.ComponentModel;
using ServiceElectronicQueue.DataCheck.Interfaces;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;

namespace ServiceElectronicQueue.Containers.CompanyDB
{
    public class ManagerContainer<TView, TEntity>
        where TView : class //UserForViews
        where TEntity : class //User
    {
        public readonly IDataCheck<TView> _check;
        public readonly TView _obj;
        public readonly IModelViewToDb<TEntity, TView> _toDb;
        public readonly UnitOfWorkCompany _unitOfWorkCompany;

        public ManagerContainer(IDataCheck<TView> check, TView obj, IModelViewToDb<TEntity, TView> toDb, UnitOfWorkCompany unitOfWorkCompany) =>
            (_check, _obj, _toDb, _unitOfWorkCompany) = 
            (check, obj, toDb, unitOfWorkCompany);

        public void MakeContainer(IContainer<TView, TEntity> container)
        {
            container.Expansion(new ManagerContainer<TView, TEntity>(_check, _obj, _toDb, _unitOfWorkCompany));
        }
    }
}*/