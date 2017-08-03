using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Download_Files
    {

        [Required]
        //[Required(ErrorMessage = "آی دی راوارد نمائید")]
        [Key]
        [DisplayName("آی دی فایل")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = ("نام فایل را وارد نمائید"))]
        [DisplayName("نام فایل")]
        public string Name { get; set; }
        [DisplayName("فایل")]
        [Display(Name = "فایل")]
        public string File { get; set; }

    }
}
