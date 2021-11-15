using in_web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace in_web.Helpers
{
    public static class AddingHelper
    {
        public static Dictionary<string, string> DataTypeValues = new Dictionary<string, string>
        {
            { "inc", "Przychód" },
            { "exp", "Wydatek" }
        };

        public static async Task<(List<SelectListItem> categories, string status)> GetCategoriesForSelect(HttpRequest request)
        {
            var categories = new List<SelectListItem>();

            HttpClient client = APIHelper.InitializeAuth(request);

            if (client == null)
            {
                return (categories, "AuthorisationError");
            }

            HttpResponseMessage res = await client.GetAsync("/api/category/getall");
            var content = await res.Content.ReadAsStringAsync();
            var result = JSONHelper.TryParseJson<ResponseObj<List<Category>>>(content);

            if (result == null || !res.IsSuccessStatusCode)
            {
                return (categories, "ServerError");
            }

            categories = result.Obj.OrderBy(cat => cat.CategoryName).Select(item => new SelectListItem() { Value = item.CategoryName, Text = item.CategoryName }).ToList();

            return (categories, "OK");
        }
    }
}