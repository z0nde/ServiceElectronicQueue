/*using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace ServiceElectronicQueue.Containers.CompanyDB
{
    public class UsersCheckCreateToDb<TView, TEntity> : IContainer<TView, TEntity> 
        where TView : class 
        where TEntity : class
    {
        public void Expansion(ManagerContainer<TView, TEntity> container)
        {
            if (container._check.Check(container._obj) != null)
            {
                container._unitOfWorkCompany.UsersRep.Create(container._toDb.ToDb(container._obj));
            }
        }
    }
}*/