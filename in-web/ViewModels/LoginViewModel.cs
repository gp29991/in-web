using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        [Display(Name = "Nazwa użytkownika")]
        [MaxLength(30, ErrorMessage = "Nazwa użytkownika nie może być dłuższa niż 30 znaków")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [MaxLength(30, ErrorMessage = "Hasło nie może być dłuższe niż 30 znaków")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
