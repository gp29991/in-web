using in_web.Helpers;
using in_web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace in_web.Controllers
{
    [Authorize]
    public class FinancesController : Controller
    {

        public async Task<IActionResult> MainPage()
        {
            Response.Cookies.Delete("options");
            Response.Cookies.Delete("sortoptions");
            Response.Cookies.Delete("chartoptions");

            HttpClient client = APIHelper.InitializeAuth(Request);
            
            if(client == null)
            {
                return RedirectToAction("Login", "Home");
            }
            
            HttpResponseMessage res = await client.GetAsync(string.Format("api/financialdata/getall?startDate={0}&endDate={1}&total=true", DateTime.Today.AddDays(-30).ToString("d", CultureInfo.InvariantCulture), DateTime.Today.ToString("d", CultureInfo.InvariantCulture)));
            var content = await res.Content.ReadAsStringAsync();
            var result1 = JSONHelper.TryParseJson<ResponseObj<Dictionary<string, decimal>>>(content);

            if (result1 == null || !res.IsSuccessStatusCode)
            {
                    return RedirectToAction("Error", "Home");
            }

            var model = new MainPageViewModel();
            
            model.TotalInc = result1.Obj["incsum"];
            model.TotalExp = result1.Obj["expsum"];

            res = await client.GetAsync(string.Format("api/financialdata/getall?startDate={0}&endDate={1}&scope=exp&minamount=1000", DateTime.Today.ToString("d", CultureInfo.InvariantCulture), DateTime.Today.AddDays(7).ToString("d", CultureInfo.InvariantCulture)));
            content = await res.Content.ReadAsStringAsync();
            var result2 = JSONHelper.TryParseJson<ResponseObj<List<FinancialData>>>(content);

            if (result2 == null || !res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            model.UpcomingSignificantExpenses = result2.Obj;
            model.UpcomingSignificantExpenses.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));

            ViewBag.title = "Strona główna";
            return View(model);
        }


        public async Task<IActionResult> DetailView(string sortBy = "Date", string sortType = "asc", string showAlert = "none")
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var options = OptionsHelper.GetOptions(Request, "det");

            var model = new DataViewModel();
            model.Options = options;

            HttpResponseMessage res;
            
            if (model.Options.Period == "all")
            {
                res = await client.GetAsync(string.Format("api/financialdata/getall?scope={0}", model.Options.ViewScope));
            }
            else
            {
                res = await client.GetAsync(string.Format("api/financialdata/getall?startDate={0}&endDate={1}&scope={2}", model.Options.StartDate?.ToString("d", CultureInfo.InvariantCulture), model.Options.EndDate?.ToString("d", CultureInfo.InvariantCulture), model.Options.ViewScope));
            }

            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<ResponseObj<List<FinancialData>>>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            model.FinancialDataList = result.Obj;

            model.SortTypeForColumns = OptionsHelper.SortingData("Amount", "Date", "CategoryName", "Name");
            
            if (sortType != "desc" && model.SortTypeForColumns.ContainsKey(sortBy))
            {
                model.SortTypeForColumns[sortBy] = "desc";
            }

            switch (sortBy)
            {
                case "Amount":
                    model.FinancialDataList.Sort((e1, e2) => e1.Amount.CompareTo(e2.Amount));
                    break;
                case "CategoryName":
                    model.FinancialDataList.Sort((e1, e2) => e1.CategoryName.CompareTo(e2.CategoryName));
                    break;
                case "Name":
                    model.FinancialDataList.Sort((e1, e2) => e1.Name.CompareTo(e2.Name));
                    break;
                default:
                    model.FinancialDataList.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));
                    break;
            }
            
            if (sortType == "desc")
            {
                model.FinancialDataList.Reverse();
            }

            var sortOptions = new SortOptions(sortBy, sortType, "det");
            string sortOptionsSerialized = JsonConvert.SerializeObject(sortOptions);
            Response.Cookies.Append("sortoptions", sortOptionsSerialized);

            model.ShowAlert = showAlert;

            ViewBag.title = "Widok szczegołowy";
            return View(model);
        }

        public async Task<IActionResult> CategoryView(string sortBy = "CategoryName", string sortType = "asc", string showAlert = "none")
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var options = OptionsHelper.GetOptions(Request, "cat");

            var model = new DataViewModel();
            model.Options = options;

            HttpResponseMessage res;

            if (model.Options.Period == "all")
            {
                res = await client.GetAsync(string.Format("api/financialdata/getall?scope={0}&aggregate=true", model.Options.ViewScope));
            }
            else
            {
                res = await client.GetAsync(string.Format("api/financialdata/getall?startDate={0}&endDate={1}&scope={2}&aggregate=true", model.Options.StartDate?.ToString("d", CultureInfo.InvariantCulture), model.Options.EndDate?.ToString("d", CultureInfo.InvariantCulture), model.Options.ViewScope));
            }

            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<ResponseObj<List<FinancialDataGrouped>>>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            model.FinancialDataGrouped = result.Obj;

            model.SortTypeForColumns = OptionsHelper.SortingData("Amount", "CategoryName");

            if (sortType != "desc" && model.SortTypeForColumns.ContainsKey(sortBy))
            {
                model.SortTypeForColumns[sortBy] = "desc";
            }

            switch (sortBy)
            {
                case "Amount":
                    model.FinancialDataGrouped.Sort((e1, e2) => e1.Amount.CompareTo(e2.Amount));
                    break;
                default:
                    model.FinancialDataGrouped.Sort((e1, e2) => e1.CategoryName.CompareTo(e2.CategoryName));
                    break;
            }

            if (sortType == "desc")
            {
                model.FinancialDataGrouped.Reverse();
            }

            var sortOptions = new SortOptions(sortBy, sortType, "cat");
            string sortOptionsSerialized = JsonConvert.SerializeObject(sortOptions);
            Response.Cookies.Append("sortoptions", sortOptionsSerialized);

            model.ShowAlert = showAlert;

            ViewBag.title = "Widok kategorii";
            return View(model);
        }

        [HttpPost]
        public IActionResult ViewRedirect(DataViewModel model)
        {
            string options = JsonConvert.SerializeObject(model.Options);
            Response.Cookies.Append("options", options);

            var sortOptions = OptionsHelper.GetSortOptions(Request);

            if(model.Options.ViewType == "cat")
            {
                if(sortOptions != null & sortOptions.ViewType == "cat")
                {
                    return RedirectToAction("CategoryView", new { sortBy = sortOptions.SortBy, sortType = sortOptions.SortType  });
                }
                return RedirectToAction("CategoryView");
            }

            if (sortOptions != null & sortOptions.ViewType == "det")
            {
                return RedirectToAction("DetailView", new { sortBy = sortOptions.SortBy, sortType = sortOptions.SortType });
            }
            return RedirectToAction("DetailView");
        }

        public IActionResult ReturnToView(string showAlert = "none")
        {
            var sortOptions = OptionsHelper.GetSortOptions(Request);

            if(sortOptions != null)
            {
                if(sortOptions.ViewType == "cat")
                {
                    return RedirectToAction("CategoryView", new { sortBy = sortOptions.SortBy, sortType = sortOptions.SortType, showAlert = showAlert });
                }
                return RedirectToAction("DetailView", new { sortBy = sortOptions.SortBy, sortType = sortOptions.SortType, showAlert = showAlert });
            }

            return RedirectToAction("DetailView", new { showAlert = showAlert });
        }

        public async Task<IActionResult> ChartView()
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var chartOptions = OptionsHelper.GetChartOptions(Request);

            var model = new ChartViewModel();
            model.ChartOptions = chartOptions;

            HttpResponseMessage res;

            if (model.ChartOptions.ChartType == "pieinc" || model.ChartOptions.ChartType == "pieexp")
            {
                if (model.ChartOptions.Period == "all")
                {
                    res = await client.GetAsync(string.Format("api/financialdata/getall?scope={0}&aggregate=true", model.ChartOptions.ChartType.Substring(3)));
                }
                else
                {
                    res = await client.GetAsync(string.Format("api/financialdata/getall?startDate={0}&endDate={1}&scope={2}&aggregate=true", model.ChartOptions.StartDate?.ToString("d", CultureInfo.InvariantCulture), model.ChartOptions.EndDate?.ToString("d", CultureInfo.InvariantCulture), model.ChartOptions.ChartType.Substring(3)));
                }

                var content = await res.Content.ReadAsStringAsync();
                var result = JSONHelper.TryParseJson<ResponseObj<List<FinancialDataGrouped>>>(content);

                if (result == null || !res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }

                model.FinancialDataGrouped = result.Obj;
            }
            else
            {
                if (model.ChartOptions.Period == "all")
                {
                    res = await client.GetAsync("api/financialdata/getall?total=true");
                }
                else
                {
                    res = await client.GetAsync(string.Format("api/financialdata/getall?startDate={0}&endDate={1}&total=true", model.ChartOptions.StartDate?.ToString("d", CultureInfo.InvariantCulture), model.ChartOptions.EndDate?.ToString("d", CultureInfo.InvariantCulture)));
                }

                var content = await res.Content.ReadAsStringAsync();
                var result = JSONHelper.TryParseJson<ResponseObj<Dictionary<string, decimal>>>(content);

                if (result == null || !res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error", "Home");
                }

                model.TotalInc = result.Obj["incsum"];
                model.TotalExp = result.Obj["expsum"];
            }

            ViewBag.title = "Widok wykresów";
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeChartView(ChartViewModel model)
        {
            string chartOptions = JsonConvert.SerializeObject(model.ChartOptions);
            Response.Cookies.Append("chartoptions", chartOptions);

            return RedirectToAction("ChartView");
        }

        [HttpGet]
        public async Task<IActionResult> AddEntry()
        {
            var categoriesForSelect = await AddingHelper.GetCategoriesForSelect(Request);

            if (categoriesForSelect.status == "AuthorisationError")
            {
                return RedirectToAction("Login", "Home");
            }
            else if(categoriesForSelect.status == "ServerError")
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.categories = categoriesForSelect.categories;
            ViewBag.title = "Nowy wpis";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEntry(AddEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categoriesForSelect = await AddingHelper.GetCategoriesForSelect(Request);

                if (categoriesForSelect.status == "AuthorisationError")
                {
                    return RedirectToAction("Login", "Home");
                }
                else if (categoriesForSelect.status == "ServerError")
                {
                    return RedirectToAction("Error", "Home");
                }
                ViewBag.categories = categoriesForSelect.categories;
                ViewBag.title = "Nowy wpis";
                return View();
            }
            
            if(model.DataType == "exp")
            {
                model.Data.Amount *= -1;
            }

            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.PostAsync("/api/financialdata/add", new StringContent(JsonConvert.SerializeObject(model.Data), Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("ReturnToView", new { showAlert = "add" });
        }

        [HttpGet]
        public async Task<IActionResult> EditEntry(int id)
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.GetAsync(string.Format("api/financialdata/get/{0}", id.ToString()));
            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<ResponseObj<FinancialData>>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            var categoriesForSelect = await AddingHelper.GetCategoriesForSelect(Request);

            if (categoriesForSelect.status == "AuthorisationError")
            {
                return RedirectToAction("Login", "Home");
            }
            else if (categoriesForSelect.status == "ServerError")
            {
                return RedirectToAction("Error", "Home");
            }

            var model = new AddEntryViewModel();
            model.Data = result.Obj;
            
            if(model.Data.Amount < 0)
            {
                model.Data.Amount *= -1;
                model.DataType = "exp";
            }
            else
            {
                model.DataType = "inc";
            }

            ViewBag.categories = categoriesForSelect.categories;
            ViewBag.title = "Edycja wpisu";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEntry(AddEntryViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                var categoriesForSelect = await AddingHelper.GetCategoriesForSelect(Request);

                if (categoriesForSelect.status == "AuthorisationError")
                {
                    return RedirectToAction("Login", "Home");
                }
                else if (categoriesForSelect.status == "ServerError")
                {
                    return RedirectToAction("Error", "Home");
                }
                ViewBag.categories = categoriesForSelect.categories;
                ViewBag.title = "Edycja wpisu";
                return View();
            }

            if (model.DataType == "exp")
            {
                model.Data.Amount *= -1;
            }

            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.PutAsync(string.Format("api/financialdata/edit/{0}", id.ToString()), new StringContent(JsonConvert.SerializeObject(model.Data), Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("ReturnToView", new { showAlert = "edit" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            HttpClient client = APIHelper.InitializeAuth(Request);

            if (client == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpResponseMessage res = await client.DeleteAsync(string.Format("api/financialdata/delete/{0}", id.ToString()));

            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction("Error", "Home");
            }

            var sortOptions = OptionsHelper.GetSortOptions(Request);
            
            if (sortOptions != null)
            {
                return RedirectToAction("DetailView", new { sortBy = sortOptions.SortBy, sortType = sortOptions.SortType, showAlert="delete" });
            }
            return RedirectToAction("DetailView", new { showAlert = "delete" });
        }
    }
}
