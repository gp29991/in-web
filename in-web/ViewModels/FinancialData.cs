using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace in_web.ViewModels
{
    public class FinancialData
    {
        public int FinancialDataId { get; set; }

        [Required(ErrorMessage = "Kwota jest wymagana")]
        [Display(Name = "Kwota")]
        [Range(0.00, 9999999.99, ErrorMessage = "Kwota musi mieścić się w zakresie od 0,00 do 9999999,99")]
        [RegularExpression("^[0-9]+,[0-9][0-9]$", ErrorMessage = "Należy wprowadzić kwotę w formacie \"0,00\"")]
        [Column(TypeName = "decimal(9,2)")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Data jest wymagana")]
        [Display(Name = "Data")]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [Display(Name = "Nazwa")]
        [MaxLength(30, ErrorMessage = "Nazwa nie może być dłuższa niż 30 znaków")]
        public string Name { get; set; }

        [Display(Name = "Opis (opcjonalnie)")]
        [MaxLength(90, ErrorMessage = "Opis nie może być dłuższy niż 90 znaków")]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        [Display(Name = "Kategoria")]
        [MaxLength(30)]
        public string CategoryName { get; set; }
    }
}
