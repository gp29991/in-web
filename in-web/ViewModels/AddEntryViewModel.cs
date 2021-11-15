using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class AddEntryViewModel
    {

        public FinancialData Data { get; set; }

        [Required(ErrorMessage = "Należy określić rodzaj wpisu")]
        [Display(Name = "Rodzaj")]
        public string DataType { get; set; }
    }
}
