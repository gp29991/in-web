using in_web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.Helpers
{
    public static class OptionsHelper
    {
        public static List<SelectListItem> Periods { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "-30", Text = "Ostatnie 30 dni" },
            new SelectListItem { Value = "-60", Text = "Ostatnie 60 dni" },
            new SelectListItem { Value = "-90", Text = "Ostatnie 90 dni" },
            new SelectListItem { Value = "30", Text = "Najbliższe 30 dni" },
            new SelectListItem { Value = "other", Text = "Inny okres" },
            new SelectListItem { Value = "all", Text = "Wszystko" }
        };

        public static Dictionary<string, string> ViewScopeValues = new Dictionary<string, string>
        {
            { "all", "Przychody i wydatki" },
            { "inc", "Przychody" },
            { "exp", "Wydatki" }
        };

        public static Dictionary<string, string> ViewTypeValues = new Dictionary<string, string>
        {
            { "det", "Szczegółowy" },
            { "cat", "Pogrupowany wg kategorii" }
        };

        public static Dictionary<string, string> ChartTypeValues = new Dictionary<string, string>
        {
            { "bartotals", "Przychody i wydatki" },
            { "pieinc", "Przychody wg kategorii" },
            { "pieexp", "Wydatki wg kategorii" }
        };

        public static Options VerifyOptions (Options options, string viewType)
        {
            if (!Periods.Any(item => item.Value == options.Period))
            {
                options.Period = "-30";
            }

            if(int.TryParse(options.Period, out int period))
            {
                if(period > 0)
                {
                    if(options.StartDate != DateTime.Today || options.EndDate != DateTime.Today.AddDays(period))
                    {
                        options.StartDate = DateTime.Today;
                        options.EndDate = DateTime.Today.AddDays(period);
                    }
                }
                else
                {
                    if (options.StartDate != DateTime.Today.AddDays(period) || options.EndDate != DateTime.Today)
                    {
                        options.StartDate = DateTime.Today.AddDays(period);
                        options.EndDate = DateTime.Today;
                    }
                }
            }
            else
            {
                if (options.StartDate == null || options.EndDate == null)
                {
                    options.StartDate = DateTime.Today.AddDays(-30);
                    options.EndDate = DateTime.Today;
                }
                else
                {
                    if (options.StartDate.Value.CompareTo(options.EndDate.Value) > 0)
                    {
                        options.StartDate = options.EndDate;
                    }
                }
            }

            if (!ViewScopeValues.ContainsKey(options.ViewScope))
            {
                options.ViewScope = "all";
            }

            if (!ViewTypeValues.ContainsKey(options.ViewType))
            {
                options.ViewType = viewType;
            }

            return options;
        }

        public static Options DefaultOptions(string viewType)
        {
            Options options = new();

            options.Period = "-30";
            options.StartDate = DateTime.Today.AddDays(-30);
            options.EndDate = DateTime.Today;
            options.ViewScope = "all";
            options.ViewType = viewType;

            return options;
        }

        public static Options GetOptions(HttpRequest request, string viewType)
        {
            var optionsFromCookie = request.Cookies["options"];
            Options options;

            if (optionsFromCookie != null)
            {
                options = JSONHelper.TryParseJson<Options>(optionsFromCookie);
                if (options == null)
                {
                    options = DefaultOptions(viewType);
                }
                else
                {
                    options = VerifyOptions(options, viewType);
                }
            }
            else
            {
                options = DefaultOptions(viewType);
            }

            return options;
        }

        public static Dictionary<string, string> SortingData(params string[] sortableBy)
        {
            var SortTypeForColumns = new Dictionary<string, string>();
            foreach (var i in sortableBy)
            {
                SortTypeForColumns.Add(i, "asc");
            }

            return SortTypeForColumns;
        }

        public static SortOptions GetSortOptions(HttpRequest request)
        {
            var sortOptionsFromCookie = request.Cookies["sortoptions"];
            SortOptions sortOptions = null;

            if (sortOptionsFromCookie != null)
            {
                sortOptions = JSONHelper.TryParseJson<SortOptions>(sortOptionsFromCookie);
            }

            return sortOptions;
        }

        public static ChartOptions VerifyChartOptions(ChartOptions chartOptions)
        {
            if (!Periods.Any(item => item.Value == chartOptions.Period))
            {
                chartOptions.Period = "-30";
            }

            if (int.TryParse(chartOptions.Period, out int period))
            {
                if (period > 0)
                {
                    if (chartOptions.StartDate != DateTime.Today || chartOptions.EndDate != DateTime.Today.AddDays(period))
                    {
                        chartOptions.StartDate = DateTime.Today;
                        chartOptions.EndDate = DateTime.Today.AddDays(period);
                    }
                }
                else
                {
                    if (chartOptions.StartDate != DateTime.Today.AddDays(period) || chartOptions.EndDate != DateTime.Today)
                    {
                        chartOptions.StartDate = DateTime.Today.AddDays(period);
                        chartOptions.EndDate = DateTime.Today;
                    }
                }
            }
            else
            {
                if (chartOptions.StartDate == null || chartOptions.EndDate == null)
                {
                    chartOptions.StartDate = DateTime.Today.AddDays(-30);
                    chartOptions.EndDate = DateTime.Today;
                }
                else
                {
                    if (chartOptions.StartDate.Value.CompareTo(chartOptions.EndDate.Value) > 0)
                    {
                        chartOptions.StartDate = chartOptions.EndDate;
                    }
                }
            }

            if (!ChartTypeValues.ContainsKey(chartOptions.ChartType))
            {
                chartOptions.ChartType = "bartotals";
            }

            return chartOptions;
        }

        public static ChartOptions DefaultChartOptions()
        {
            ChartOptions chartOptions = new();

            chartOptions.Period = "-30";
            chartOptions.StartDate = DateTime.Today.AddDays(-30);
            chartOptions.EndDate = DateTime.Today;
            chartOptions.ChartType = "bartotals";

            return chartOptions;
        }

        public static ChartOptions GetChartOptions(HttpRequest request)
        {
            var chartOptionsFromCookie = request.Cookies["chartoptions"];
            ChartOptions chartOptions;

            if (chartOptionsFromCookie != null)
            {
                chartOptions = JSONHelper.TryParseJson<ChartOptions>(chartOptionsFromCookie);
                if (chartOptions == null)
                {
                    chartOptions = DefaultChartOptions();
                }
                else
                {
                    chartOptions = VerifyChartOptions(chartOptions);
                }
            }
            else
            {
                chartOptions = DefaultChartOptions();
            }

            return chartOptions;
        }
    }
}
