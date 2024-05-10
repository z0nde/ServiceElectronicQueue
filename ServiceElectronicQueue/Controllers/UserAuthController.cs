using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.DataCheck;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews.Login;
using ServiceElectronicQueue.Models.ForViews.Register;

namespace ServiceElectronicQueue.Controllers;

public class UserAuthController : Controller
{
    private readonly UnitOfWorkCompany _unitOfWork;

    private readonly UserManager _userManager;
    private readonly OrganizationManager _organizationManager;
    private User _user;

    public UserAuthController(CompanyDbContext dbContext)
    {
        _unitOfWork = new UnitOfWorkCompany(dbContext);
        _userManager = new UserManager(_unitOfWork);
        _organizationManager = new OrganizationManager(_unitOfWork);
    }

    /// <summary>
    /// Регистрация пользователя, GET
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult UserRegister()
    {
        
        return View();
    }

    /// <summary>
    /// Регистрация пользователя, POST
    /// </summary>
    /// <param name="userRegisterForView"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult UserRegister(UserRegisterForView userRegisterForView)
    {
        if (!ModelState.IsValid)
            return View();
        if (_userManager.CheckRegister(userRegisterForView) != null)
        {
            if (userRegisterForView.Role is "Пользователь организации" or "Пользователь филиала")
            {
                return RedirectToAction("UserAccount", "UserAccount", new
                {
                    UserId = Guid.NewGuid(), Email = userRegisterForView.Email, Password = userRegisterForView.Password,
                    Role = userRegisterForView.Role, Surname = userRegisterForView.Surname,
                    Name = userRegisterForView.Name,
                    Patronymic = userRegisterForView.Patronymic, PhoneNumber = userRegisterForView.PhoneNumber
                });
            }
        }

        return View();
    }

    /// <summary>
    /// Вход пользователя, GET
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult UserLogin()
    {
        return View();
    }

    /// <summary>
    /// Вход пользователя, POST
    /// </summary>
    /// <param name="userLoginForView"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult UserLogin(UserLoginForView userLoginForView)
    {
        if (!ModelState.IsValid)
            return View();
        if (_userManager.CheckLogin(userLoginForView) != null)
        {
            Guid? userId = _unitOfWork.UsersRep
                .GetAll()
                .ToList()
                .Where(s => s.Email == userLoginForView.Email && s.Password == userLoginForView.Password)
                .Select(s => s.IdUser)
                .FirstOrDefault();
            if (userId != null)
            {
                User? user = _unitOfWork.UsersRep.GetByIndex((Guid)userId);
                if (user != null)
                {
                    return RedirectToAction("UserAccount", "UserAccount", new
                    {
                        UserId = user.IdUser, Email = user.Email, Password = user.Password, Role = user.Role,
                        Surname = user.Surname, Name = user.Name, Patronymic = user.Patronymic,
                        PhoneNumber = user.PhoneNumber
                    });
                }
            }
        }

        return View();
    }

    protected override void Dispose(bool disposing)
    {
        _userManager.Dispose();
        _unitOfWork.Dispose();
        base.Dispose(disposing);
    }


    /*[HttpPost]
    public IActionResult ValidationRoles()
    {
        Role role1 = new Role("Пользователь организации");
        Role role2 = new Role("Пользователь филиала");
        _unitOfWork.RoleRep.Create(role1);
        _unitOfWork.Save();
        _unitOfWork.RoleRep.Create(role2);
        _unitOfWork.Save();
        _unitOfWork.Dispose();
        return RedirectToAction("UserRegister", "UserAuth");
    }*/
}