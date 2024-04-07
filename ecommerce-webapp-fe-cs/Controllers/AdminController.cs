using ecommerce_webapp_fe_cs.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ecommerce_webapp_fe_cs.Controllers;
public class AdminController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public IActionResult Index() => View();
    public async Task<IActionResult> Dashboard()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail)) return Unauthorized("User is not authenticated.");
        
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile?email={userEmail}");

        if (response.IsSuccessStatusCode)
        {
            return View();
        }
        else
        {
            return NotFound("Profile not found.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/login", content);

            if (response.IsSuccessStatusCode)
            {
                // On successful login, set the user email in the session
                HttpContext.Session.SetString("UserEmail", model.Email);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed.");
            }
        }
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clears the session, effectively logging out the user
        return RedirectToAction("Login", "Account");
    }
}