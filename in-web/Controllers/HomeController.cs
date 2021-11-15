using in_web.Helpers;
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
            return RedirectToAction("MainPage", "Finances");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(Request.Cookies["token"] != null && User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name) != null)
            {
                return RedirectToAction("MainPage", "Finances");
            }
            Response.Cookies.Delete("token");
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
            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<Response>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                if (result != null && result.Message == "InvalidCredentials")
                {
                    ModelState.AddModelError("Error", "Nieprawidłowa nazwa użytkownika lub nieprawidłowe hasło");
                }
                else
                {
                    //ModelState.AddModelError("Error", "Błąd po stronie serwera");
                    return RedirectToAction("Error");
                }
                return View();
            }
            
            Response.Cookies.Append("token", result.Message, new CookieOptions()
            {
                Expires = result.Expires
            });
            
            return RedirectToAction("MainPage", "Finances");
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
                Response result = JSONHelper.TryParseJson<Response>(content);
                if (result != null && result.Message == "UsernameAlreadyInUse")
                {
                    ModelState.AddModelError("Error", "Podana nazwa użytkownika już jest w użyciu");
                }
                else if (result != null && result.Message == "EmailAlreadyInUse")
                {
                    ModelState.AddModelError("Error", "Podany adres e-mail już jest w użyciu");
                }
                else
                {
                    //ModelState.AddModelError("Error", "Błąd po stronie serwera");
                    return RedirectToAction("Error");
                }
                return View();
            }
            
            return View("RegistrationSuccess");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
