using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Areas.Administrator.Models
{
    public class About
    {
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("آی دی")]
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "عنوان حداکثر 30 کارکتر می باشد")]
        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("توضیحات")]
        [Required(ErrorMessage = ("توضیحات را وارد نمائید"))]
        public string Description { get; set; }

       [DisplayName("تصویر")]
        [Display(Name = "تصویر")]
        [DataType(DataType.ImageUrl)]
        ////IFormFile
        public string Image { get; set; }
       


    }
}
