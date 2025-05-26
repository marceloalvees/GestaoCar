using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;

namespace WebInterface.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Login", "User");
    }
   
}
