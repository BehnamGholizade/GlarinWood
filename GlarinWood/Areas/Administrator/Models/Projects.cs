using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Areas.Administrator.Models
{
    public class Projects
    {
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("آی دی")]
        public int Id { get; set; }
        [StringLength(25, ErrorMessage ="عنوان پروژه حداکثر 25 کارکتر می باشد")]
        [DisplayName("عنوان پروژه")]
        [Required(ErrorMessage = ("عنوان پروژه را وارد نمائید"))]
        public string Title_Project { get; set; }

        [StringLength(35, ErrorMessage = "توضیحات پروژه حداکثر 35 کارکتر می باشد")]
        [DisplayName("توضیحات")]
        [Required(ErrorMessage = ("توضیحات را وارد نمائید"))]
        public string Description { get; set; }

        [DisplayName("تصویر")]
        [Display(Name = "تصویر")]
        [DataType(DataType.ImageUrl)]
        ////IFormFile
        public string Image { get; set; }

        [StringLength(20, ErrorMessage = "توضیحات عکس حداکثر 20 کارکتر می باشد")]
        [DisplayName("توضیحات عکس")]
        [Required(ErrorMessage = ("توضیحات عکس را وارد نمائید"))]
        public string Image_Description { get; set; }
    }
}
