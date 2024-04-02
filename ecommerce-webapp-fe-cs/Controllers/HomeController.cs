using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ecommerce_webapp_fe_cs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}