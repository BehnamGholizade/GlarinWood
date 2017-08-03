using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(20,MinimumLength =5)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = ("رایانامه را وارد نمائید"))]
        [DisplayName("رایانامه")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = ("موضوع را وارد نمائید"))]
        [DisplayName("موضوع")]
        public string Subject { get; set; }
        [Required(ErrorMessage = ("پیام را وارد نمائید"))]
        [DisplayName("پیام")]
        public string Message { get; set; }
        public Contact Contacts { get; set; }

    }
}
