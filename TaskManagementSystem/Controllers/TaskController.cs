using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers;

public class TaskController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}