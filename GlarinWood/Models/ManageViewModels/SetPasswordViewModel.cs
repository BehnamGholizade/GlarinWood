using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.ManageViewModels
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = " گذرواژه حداقل 6 کاراکتر باید باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تایید گذرواژه جدید")]
        [Compare("NewPassword", ErrorMessage = " گذرواژه و تایید آن یکسان نیست.")]
        public string ConfirmPassword { get; set; }
    }
}
