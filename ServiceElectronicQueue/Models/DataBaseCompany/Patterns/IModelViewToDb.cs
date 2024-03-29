namespace ServiceElectronicQueue.Models.DataBaseCompany;

public interface IModelViewToDb<T, T1> 
    where T : class
    where T1 : class
{
    public T ToDb(T1 view);
}