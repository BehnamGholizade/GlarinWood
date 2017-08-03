using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class PicGallery
    {
        public PicGallery()
        {
            this.PicCollectionGallerys = new HashSet<PicCollectionGallery>();
        }
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("آی دی گالری ")]
        public int Id { get; set; }
        [Required(ErrorMessage = ("نام گالری را وارد نمائید"))
       , DisplayName("نام گالری")]
        public string Name { get; set; }

        [DisplayName("تصویر گالری")]
        [Display(Name = "تصویر گالری")]
        //[DataType(DataType.ImageUrl)]
        ////IFormFile
        public string Image { get; set; }

        [Required(ErrorMessage = "توضیحات را وارد کنید", AllowEmptyStrings = false)]
        [DisplayName("توضیحات")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "کلمه کلید را وارد کنید", AllowEmptyStrings = false)]
        [DisplayName("کلمه کلیدی ")]
        [Display(Name = "کلمه کلیدی ")]
        public string Keywords { get; set; }

        [DisplayName("تگ")]
        [Display(Name = "تگ")]
        public string Tags { get; set; }

        [DisplayName("کالکشن گالری")]
        public virtual ICollection<PicCollectionGallery> PicCollectionGallerys { get; set; }


    }
}
