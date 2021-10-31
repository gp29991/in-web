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
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
