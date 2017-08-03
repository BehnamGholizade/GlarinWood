using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class PicCollectionGallery
    {
        //[Column(Order = 1)]
        [DisplayName("آی دی گروه محصول")]
        public int PicGalleryId { get; set; }

        [Required]
        [Key]
        [DisplayName("آی دی محصول")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("تصویر محصول")]
        [Display(Name = "تصویر محصول")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }
        [DisplayName("گروه محصول")]
        public virtual PicGallery PicGallery { get; set; }
    }
}
