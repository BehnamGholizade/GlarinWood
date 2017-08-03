using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "تکمیل فیلد پرووایدر الزامی است")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "تکمیل فیلد کد الزامی است")]
        [Display(Name = "کد")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "مرور گر را به خاطر بسپار؟")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
