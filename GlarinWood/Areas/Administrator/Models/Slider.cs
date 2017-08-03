using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Areas.Administrator.Models
{
    public class Slider
    {
        [Key]
        [Required]
        [DisplayName("آی دی گروه محصول")]

        public int Id { get; set; }
        [Required(ErrorMessage = (" عنوان اسلایدر اول را وارد نمائید"))]
        [DisplayName("عنوان اسلایدر اول")]
        [Display(Name = "عنوان اسلایدر اول")]
        public string Title_1 { get; set; }
        [Required(ErrorMessage = (" توضیحات اسلایدر اول را وارد نمائید"))]
        [DisplayName("توضیحات اسلایدر اول")]
        [Display(Name = "توضیحات اسلایدر اول")]
        public string Description_1 { get; set; }
        //[Required(ErrorMessage = ("تصویر محصول را وارد نمائید"))]
        [DisplayName("تصویر اسلایدر اول")]
        [Display(Name = "تصویر اسلایدر اول")]
        [DataType(DataType.ImageUrl)]
        ////IFormFile
        public string Image_1 { get; set; }
        ////
        [Required(ErrorMessage = (" عنوان اسلایدر دوم را وارد نمائید"))]
        [DisplayName("عنوان اسلایدر دوم")]
        [Display(Name = "عنوان اسلایدر دوم")]
        public string Title_2 { get; set; }
        [Required(ErrorMessage = (" توضیحات اسلایدر دوم را وارد نمائید"))]
        [DisplayName("توضیحات اسلایدر دوم")]
        [Display(Name = "توضیحات اسلایدر دوم")]
        public string Description_2 { get; set; }
        //[Required(ErrorMessage = ("تصویر محصول را وارد نمائید"))]
        [DisplayName("تصویر اسلایدر دوم")]
        [Display(Name = "تصویر اسلایدر دوم")]
        [DataType(DataType.ImageUrl)]
        ////IFormFile
        public string Image_2 { get; set; }
    }
}
