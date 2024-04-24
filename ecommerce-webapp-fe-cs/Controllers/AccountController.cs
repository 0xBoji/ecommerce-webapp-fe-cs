using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using ecommerce_webapp_fe_cs.Models;
using Microsoft.AspNetCore.Http;
using ecommerce_webapp_fe_cs.Models.AccountModels;
using System.Security.Claims;
using System.Net.Http;

public class AccountController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public IActionResult SignUp()
    {
        return View();
    }

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
    public IActionResult GoogleLogin()
    {
        return Redirect("https://localhost:7195/google/login");
    }

    public IActionResult GoogleLoginCallback(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set to true if using HTTPS, which is recommended
            Expires = DateTime.UtcNow.AddDays(7) // Set the cookie to expire when the token does
        };
        Response.Cookies.Append("JWTToken", token, cookieOptions);

        return RedirectToAction("Profile");
    }


    public IActionResult Login()
    {
        return View();
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
                HttpContext.Session.SetString("UserEmail", model.Email);
                return RedirectToAction("Profile");
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
        ProfileModel profileModel = null;

        var userEmail = HttpContext.Session.GetString("UserEmail");

        var client = _clientFactory.CreateClient();

        if (!string.IsNullOrEmpty(userEmail))
        {
            var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile?email={userEmail}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                profileModel = JsonConvert.DeserializeObject<ProfileModel>(jsonString);
            }
        }
        else
        {
            // If no email, try to get JWT token (Google login)
            var token = Request.Cookies["JWTToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:7195/api/v1/accounts/profile");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    profileModel = JsonConvert.DeserializeObject<ProfileModel>(jsonString);
                }
            }
        }

        if (profileModel != null)
        {
            return View(profileModel); // Return the profile view with the model
        }
        else
        {
            return Unauthorized("User is not authenticated."); // Or redirect to login
        }
    }



    public IActionResult CaptureToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            Response.Cookies.Append("JWTToken", token, new CookieOptions { HttpOnly = true, Secure = true });
            return RedirectToAction("Profile");
        }

        return Unauthorized("Token is missing or invalid.");
    }



    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clears the session
        Response.Cookies.Delete("JWTToken"); // Clears the JWT token cookie
        return RedirectToAction("Login"); // Redirects to the login page
    }

    public async Task<IActionResult> ProfileEdit()
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7195/api/v1/accounts/profile/edit");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var profileModel = JsonConvert.DeserializeObject<ProfileEditModel>(jsonString);
            return View(profileModel);
        }
        else
        {
            return NotFound("Profile not found.");
        }
    }

	[HttpPost]
	public async Task<IActionResult> ProfileEdit(ProfileEditModel model, IFormFile file)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		// Prepare HTTP client
		var client = _clientFactory.CreateClient();
		using var content = new MultipartFormDataContent();

		// Add model data to the multipart form data content
		content.Add(new StringContent(model.Username), nameof(model.Username));
		content.Add(new StringContent(model.FirstName), nameof(model.FirstName));
		content.Add(new StringContent(model.LastName), nameof(model.LastName));
		content.Add(new StringContent(model.PhoneNum), nameof(model.PhoneNum));
		content.Add(new StringContent(model.CompanyName ?? ""), nameof(model.CompanyName));
		content.Add(new StringContent(model.AddressLine1 ?? ""), nameof(model.AddressLine1));
		content.Add(new StringContent(model.Country ?? ""), nameof(model.Country));
		content.Add(new StringContent(model.Province ?? ""), nameof(model.Province));
		content.Add(new StringContent(model.City ?? ""), nameof(model.City));
		content.Add(new StringContent(model.PostalCode ?? ""), nameof(model.PostalCode));

		// Handle file upload
		if (file != null && file.Length > 0)
		{
			var streamContent = new StreamContent(file.OpenReadStream());
			streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
			content.Add(streamContent, "UserImg", file.FileName);
		}

		// Make the API call to update the profile
		var response = await client.PutAsync("https://localhost:7195/api/v1/accounts/profile/edit", content);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Profile", "Account");
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Profile update failed.");
			return View(model);
		}
	}

}