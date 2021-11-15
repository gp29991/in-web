using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.Helpers
{
    public static class JSONHelper
    {
        public static T TryParseJson<T>(string obj)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.MissingMemberHandling = MissingMemberHandling.Error;

                return JsonConvert.DeserializeObject<T>(obj, settings);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
