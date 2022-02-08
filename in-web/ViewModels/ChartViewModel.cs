using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class ChartViewModel
    {
        public decimal TotalInc { get; set; }
        public decimal TotalExp { get; set; }
        public List<FinancialDataGrouped> FinancialDataGrouped { get; set; }
        public ChartOptions ChartOptions { get; set; }
    }

    public class ChartOptions
    {
        public string Period { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ChartType { get; set; }
    }
}
