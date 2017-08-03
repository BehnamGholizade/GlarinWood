using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "تکمیل فیلد رایانامه الزامی است")]
        [EmailAddress]
        [Display(Name = "رایانامه")]
        public string Email { get; set; }

        [Required(ErrorMessage = "تکمیل فیلد گذرواژه الزامی است")]
        [StringLength(100, ErrorMessage = " گذرواژه حداقل 6 کاراکتر باید باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تائید گذرواژه")]
        [Compare("Password", ErrorMessage = " گذرواژه و تائید آن یکسان نیست.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
