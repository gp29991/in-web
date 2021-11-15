using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class ResponseObj<T> : Response
    {
        public T Obj { get; set; }
    }
}
