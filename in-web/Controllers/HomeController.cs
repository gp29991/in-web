using in_web.Helpers;
using in_web.Models;
using in_web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace in_web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Test");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.title = "Logowanie";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.title = "Logowanie";
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            HttpClient client = APIHelper.Initialize();
            HttpResponseMessage res = await client.PostAsync("api/user/login", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            string content;
            Response result;

            if (!res.IsSuccessStatusCode)
            {
                content = await res.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<Response>(content);
                if (result.Message == "InvalidCredentials")
                {
                    ModelState.AddModelError("Error", "Nieprawidłowa nazwa użytkownika lub nieprawidłowe hasło");
                }
                else
                {
                    ModelState.AddModelError("Error", "Błąd po stronie serwera");
                }
                return View();
            }
            content = await res.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<Response>(content);
            
            Response.Cookies.Append("token", result.Message, new CookieOptions()
            {
                Expires = result.Expires
            });
            
            return RedirectToAction("Test");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.title = "Rejestracja";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.title = "Rejestracja";
            if (!ModelState.IsValid)
            {
                return View();
            }
            HttpClient client = APIHelper.Initialize();
            HttpResponseMessage res = await client.PostAsync("api/user/register", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            if (!res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                Response result = JsonConvert.DeserializeObject<Response>(content);
                if (result.Message == "UsernameAlreadyInUse")
                {
                    ModelState.AddModelError("Error", "Podana nazwa użytkownika już jest w użyciu");
                }
                else if (result.Message == "EmailAlreadyInUse")
                {
                    ModelState.AddModelError("Error", "Podany adres e-mail już jest w użyciu");
                }
                else
                {
                    ModelState.AddModelError("Error", "Błąd po stronie serwera");
                }
                return View();
            }
            return View("Test");
        }

        [Authorize]
        public IActionResult Test()
        {
            ViewBag.title = "Test placeholder for main page after login (will be in a different controller)";
            ViewBag.username = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
            return View();
        }
    }
}
