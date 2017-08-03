using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Order
    {
        public Order()

        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        //public int ProductId { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "تکمیل فیلد رایانامه الزامی است")]
        [EmailAddress(ErrorMessage = "لطفا رایانامه معتبر وارد نمایید")]
        [Display(Name = "رایانامه")]
        public string Email { get; set; }
        [Required(ErrorMessage = "شماره تماس را وارد نمایید")]
        [Phone(ErrorMessage ="شماره تماس را صحیح وارد نمایید")]
        [Display(Name = "شماره موبایل")]
        public string Phone { get; set; }
        [Display(Name = "تعداد")]
        public int Qty { get; set; }
        [Display(Name = "مبلغ کل")]
        public decimal Total { get; set; }
        [Required(ErrorMessage = "لطفا آدرس دقیق وارد نمایید")]
        [StringLength(100, MinimumLength = 10)]
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        //Phone = model.Phone,

        //[Required]
        //[StringLength(20, MinimumLength = 8)]
        //public string PostalCode { get; set; }
        //[Required]
        public string TransactionId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTimeOffset OrderDate { get; set; }
        public bool IsBuy { get; set; }

        //public virtual Product Product { get; set; }
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

  

    }
}



