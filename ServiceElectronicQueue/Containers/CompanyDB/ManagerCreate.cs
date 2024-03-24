/*using ServiceElectronicQueue.DataCheck.Interfaces;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;

namespace ServiceElectronicQueue.Containers.CompanyDB;

public class ManagerCreate<T, T1> : UnitOfWorkCompany
    where T : class  //UserForViews
    where T1 : class //User
{
    private readonly IDataCheck<T> _check;
    private readonly T _obj;
    private readonly IModelViewToDb<T1, T> _toDb;
    private readonly IRepository<T1> _repository;

    public ManagerCreate(IDataCheck<T> check, T obj, IModelViewToDb<T1, T> toDb, IRepository<T1> repository) =>
        (_check, _obj, _toDb, _repository) = (check, obj, toDb, repository);

    public void Expansion()
    {
        if (_check.Check(_obj) != null)
        {
            _repository.Create(_toDb.ToDb(_obj));
            Save();
            Dispose();
        }
    }
}*/