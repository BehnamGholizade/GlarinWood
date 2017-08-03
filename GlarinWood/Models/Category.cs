using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("آی دی گروه محصول")]
        public int Id { get; set; }
        [Required(ErrorMessage = ("گروه محصول را وارد نمائید")) 
       ,  DisplayName("گروه محصول")]
        public string Name { get; set; }

         [DisplayName("تصویر محصول")]
        [Display(Name = "تصویر محصول")]
        //[DataType(DataType.ImageUrl)]
        ////IFormFile
        public string Image { get; set; }

        [DisplayName("محصولات")]
        public virtual ICollection<Product> Products { get; set; }
      

    }
}
