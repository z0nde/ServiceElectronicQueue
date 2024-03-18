namespace ServiceElectronicQueue.DataCheck.Interfaces;

public interface IDataCheck<T> where T : class
{
    public T? Check(T? obj);
}