using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "تکمیل فیلد رایانامه الزامی است")]
        [EmailAddress(ErrorMessage = "لطفا رایانامه معتبر وارد نمایید")]
        [Display(Name = "رایانامه")]
        public string Email { get; set; }

        [Required(ErrorMessage = "تکمیل فیلد گذرواژه الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه")]
        [StringLength(100, ErrorMessage = "گذرواژه حداقل 6 کاراکتر باید باشد.", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار؟")]
        public bool RememberMe { get; set; }
    }
}
