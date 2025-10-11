using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}