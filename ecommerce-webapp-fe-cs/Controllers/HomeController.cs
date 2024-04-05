using Microsoft.AspNetCore.Mvc;

namespace ecommerce_webapp_fe_cs.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}