using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Contact
    {
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("آی دی  ")]
        public int Id { get; set; }
        [Required(ErrorMessage = ("آدرس را وارد نمائید"))
       , DisplayName("آدرس")]
        public string Address { get; set; }

        [Required(ErrorMessage = ("شماره تماس شرکت را وارد نمائید"))
      , DisplayName("شماره شرکت")]
        public string PhoneOffice { get; set; }
        [Required(ErrorMessage = ("شماره فکس را وارد نمائید"))
      , DisplayName("شماره فکس")]
        public string Fax { get; set; }
        [Phone]
        [Required(ErrorMessage = ("شماره موبایل را وارد نمائید"))
      , DisplayName("شماره موبایل")]
        public string Mobile { get; set; }
        [DisplayName("شماره پشتیبانی")]
        public string PhoneSupport { get; set; }
       [EmailAddress(ErrorMessage ="لطفا رایانامه را صحیح وارد نمائید")]
        [Required(ErrorMessage = ("رایانامه را وارد نمائید"))
      , DisplayName("رایانامه")]
        public string Email { get; set; }
        [DisplayName("اینستاگرام")]
        public string Instagram { get; set; }
        [DisplayName("تلگرام")]
        public string Telegram { get; set; }
    }
}
