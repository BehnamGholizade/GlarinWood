using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Payment
    {
        #region سازنده پیش فرض
        public Payment()
        {
            InsertDatetime = System.DateTimeOffset.Now.DateTime;
        }
        #endregion

        #region پراپرتی ها
        //[Required]
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Display(Name = "آی دی جدول")]
        public int PaymentId { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "شماره پیگیری")]
        public long ReferenceNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "شماره پرداخت")]
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string SaleReferenceId { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "وضعیت پرداخت")]
        [System.ComponentModel.DataAnnotations.MaxLength(5)]
        public string StatusPayment { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "مبلغ")]
        public int Amount { get; set; }

        //[System.ComponentModel.DataAnnotations.Display(Name = "آی دی کاربر")]
        //public string UserId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "تاریخ خرید")]
        public DateTime InsertDatetime { get; set; }
        #endregion
    }
}
