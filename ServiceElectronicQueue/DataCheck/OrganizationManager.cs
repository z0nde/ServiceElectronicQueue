using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.DataCheck;

public class OrganizationManager : IDisposable
{
    private readonly UnitOfWorkCompany _unitOfWork;

    public OrganizationManager(UnitOfWorkCompany unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public OrganizationRegisterForView? CheckRegister(OrganizationRegisterForView? obj)
    {
        return obj is
        {
            Email: not null, Password: not null, Title: not null
        } ? obj : null;
    }

    public OrganizationLoginForView? CheckLogin(OrganizationLoginForView? obj)
    {
        return obj is
        {
            Email: not null, Password: not null
        } ? obj : null;
    }

    public Organization RegisterToDb(OrganizationRegisterForView obj)
    {
        return new Organization(obj.Email, obj.Password, obj.Title, null, null);
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