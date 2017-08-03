using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "تکمیل فیلد رایانامه الزامی است")]
        [Display(Name = "رایانامه")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
