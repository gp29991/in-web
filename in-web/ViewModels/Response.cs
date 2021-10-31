using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime? Expires { get; set; }
    }
}
