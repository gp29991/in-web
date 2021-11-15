using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        [Display(Name = "Kategoria")]
        [MaxLength(30, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 30 znaków")]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
    }
}
