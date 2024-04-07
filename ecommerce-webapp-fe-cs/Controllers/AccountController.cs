using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using ecommerce_webapp_fe_cs.Models.AccountModels;

public class AccountController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public IActionResult SignUp() => View();

    [HttpPost]
    public async Task<IActionResult> SignUp(UserRegistrationModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/signup", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
        }
        return View(model);
    }

    public IActionResult Login() => View();

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
                return RedirectToAction("Profile");  // Redirect to profile to immediately see user information
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed.");
            }
        }
        return View(model);
    }


    public async Task<IActionResult> Profile()
    {
        // Example: Fetching user email from session. Adjust based on your authentication mechanism.
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("User is not authenticated.");
        }

        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile?email={userEmail}");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var profileModel = JsonConvert.DeserializeObject<ProfileModel>(jsonString);
            return View(profileModel);
        }
        else
        {
            return NotFound("Profile not found.");
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clears the session, effectively logging out the user
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ProfileEdit() => View();

    [HttpPost]
    public async Task<IActionResult> ProfileEdit(ProfileEditModel model, IFormFile file)
    {
        //check if login or not
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("User is not authenticated.");
        }

        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                var filename = DateTime.Now.Ticks + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", filename);
                using (var stream = new FileStream(filePath, FileMode.Create)) //to upload the pic into the folder we've created
                {
                    await file.CopyToAsync(stream);
                }
                model.UserImg = filename;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "File upload failed");
                return View(model);
            }

            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7195/api/v1/accounts/profile/edit", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
        }
        return View(model);
    }

}
