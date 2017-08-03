using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.CheckoutViewModel
{
    public class CheckoutViewModel
    {
        [Required (ErrorMessage ="لطفا نام را وارد نمایید")]
        [StringLength(100)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی را وارد نمایید")]
        [StringLength(100)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا آدرس را وارد نمایید")]
        [Display(Name = "آدرس")]
        [StringLength(100, MinimumLength = 10,ErrorMessage ="حداقل متن آدرس کمتر از 10 کاراکتر می باشد!")]
         public string Address { get; set; }
        [Required(ErrorMessage = "لطفا تلفن همراه را وارد نمایید")]
        [Display(Name = "تلفن همراه")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "لطفا رایانامه را وارد نمایید")]
        [Display(Name = "رایانامه")]
        [RegularExpression(pattern:
        @"\w+([-+.’]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "رایانامه نا معتبر می باشد، لطفا ایمیل صحیح وارد نمایید!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int Qty { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public decimal Total { get; set; }
        public virtual IList<CartItem> cartItems { get; set; }

    }
}
