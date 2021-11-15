using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace in_web.Helpers
{
    public static class APIHelper
    {
        public static HttpClient Initialize()
        {
            HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:44347/");
            return client;
        }

        public static HttpClient InitializeAuth(HttpRequest request)
        {
            var token = request.Cookies["token"];
            
            if(token == null)
            {
                return null;
            }

            HttpClient client = Initialize();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
