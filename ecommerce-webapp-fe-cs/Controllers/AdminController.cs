using ecommerce_webapp_fe_cs.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ecommerce_webapp_fe_cs.Models.AdminModels;

namespace ecommerce_webapp_fe_cs.Controllers;
[Route("[controller]")]
public class AdminController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public AdminController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("login-admin")]
    public IActionResult LoginAdmin()
    {
        return View();
    }

    [HttpPost("login-admin")]
    public async Task<IActionResult> LoginAdmin(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/login-admin", content); 

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Admin"); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials and try again.");
            }
        }
        return View(model);
    }

    [HttpGet("product-list")]
    public IActionResult ShowList()
    {
        return View();
    }


    [HttpGet("blog-list")]
    public IActionResult ShowBlogs()
    {
        return View();
    }


    [HttpGet("nego-list")]
    public IActionResult ShowNegotiations()
    {
        return View();
    }



    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); 
        Response.Cookies.Delete("JWTToken");
        return RedirectToAction("Login"); 
    }

}
