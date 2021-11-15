using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class MainPageViewModel
    {
        public List<FinancialData> UpcomingSignificantExpenses { get; set; }
        public decimal TotalInc { get; set; }
        public decimal TotalExp { get; set; }
    }
}
