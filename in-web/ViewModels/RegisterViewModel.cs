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
        [MaxLength(30, ErrorMessage = "Adres e-mail nie może być dłuższy niż 30 znaków")]
        [RegularExpression("^.+@.+$", ErrorMessage = "Należy wprowadzić prawidłowy adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        [Display(Name = "Nazwa użytkownika")]
        [MaxLength(30, ErrorMessage = "Nazwa użytkownika nie może być dłuższa niż 30 znaków")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [MaxLength(30, ErrorMessage = "Hasło nie może być dłuższe niż 30 znaków")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [Display(Name = "Powtórz hasło")]
        [MaxLength(30, ErrorMessage = "Hasło nie może być dłuższe niż 30 znaków")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }
}
