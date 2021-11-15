using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class DataViewModel
    {
        public List<FinancialData> FinancialDataList { get; set; }
        public List<FinancialDataGrouped> FinancialDataGrouped { get; set; }
        public Options Options { get; set; }

        public Dictionary<string, string> SortTypeForColumns;

        public string ShowAlert;
    }

    public class Options
    {
        public string Period { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ViewScope { get; set; }

        public string ViewType { get; set; }
    }

    public class SortOptions
    {
        public SortOptions(string sortBy, string sortType, string viewType)
        {
            SortBy = sortBy;
            SortType = sortType;
            ViewType = viewType;
        }

        public string SortBy { get; set; }

        public string SortType { get; set; }

        public string ViewType { get; set; }
    }
}