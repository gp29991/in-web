using in_web.Helpers;
using in_web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace in_web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public async Task<IActionResult> CategoryList(string showAlert = "none")
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.GetAsync("/api/category/getall");
            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<ResponseObj<List<Category>>>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            result.Obj.Sort((e1, e2) => e1.CategoryName.CompareTo(e2.CategoryName));

            bool tomainpage = false;
            if(Request.Cookies["sortoptions"] == null)
            {
                tomainpage = true;
            }

            ViewBag.tomainpage = tomainpage;
            ViewBag.showalert = showAlert;
            ViewBag.title = "Lista kategorii";
            return View(result.Obj);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            ViewBag.title = "Nowa kategoria";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.title = "Nowa kategoria";
                return View();
            }

            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.PostAsync("/api/category/add", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var result = JSONHelper.TryParseJson<ResponseObj<Category>>(content);
                if (result != null && result.Message == "CategoryAlreadyExists")
                {
                    ModelState.AddModelError("Error", "Wprowadzona kategoria już istnieje");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
                return View();
            }

            return RedirectToAction("CategoryList", new { showAlert = "add" });
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.GetAsync(string.Format("api/category/get/{0}", id.ToString()));
            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<ResponseObj<Category>>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.title = "Edycja kategorii";
            return View(result.Obj);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model, int id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.title = "Edycja kategorii";
                return View();
            }

            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.PutAsync(string.Format("api/category/edit/{0}", id.ToString()), new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var result = JSONHelper.TryParseJson<ResponseObj<Category>>(content);
                if (result != null && result.Message == "CategoryAlreadyExists")
                {
                    ModelState.AddModelError("Error", "Wprowadzona kategoria już istnieje");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
                return View();
            }

            return RedirectToAction("CategoryList", new { showAlert = "edit" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.DeleteAsync(string.Format("api/category/delete/{0}", id.ToString()));

            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("CategoryList", new { showAlert = "delete" });
        }

        [HttpPost]
        public async Task<IActionResult> ClearDefaultCategoryEntries(string defcat)
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.DeleteAsync(string.Format("api/category/clear/{0}", defcat));

            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("CategoryList", new { showAlert = "clear" });
        }
    }
}
