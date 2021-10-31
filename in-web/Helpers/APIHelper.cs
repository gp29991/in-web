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
    }
}
