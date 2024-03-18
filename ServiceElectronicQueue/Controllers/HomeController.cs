using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceElectronicQueue.Containers.CompanyDB;
using ServiceElectronicQueue.DataCheck.Interfaces.UserCheck;
using ServiceElectronicQueue.Models;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.ForViews;

namespace ServiceElectronicQueue.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(UserForView userForView)
    {
        if (ModelState.IsValid)
        {
            /*ManagerCreate<UserForView, User> managerCreate = new(new CheckCreate(), userForView, new User(), new UserRepository());
            managerCreate.Expansion();*/
        }
        return View();
    }

    public IActionResult RegisterOrganization()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}