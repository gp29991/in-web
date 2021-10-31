using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [Display(Name = "Adres e-mail")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }
}
