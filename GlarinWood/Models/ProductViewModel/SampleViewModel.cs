using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace GlarinWood.Models.ProductViewModel
{
    public class SampleViewModel
    {

        [Required]
        //[Required(ErrorMessage = "آی دی راوارد نمائید")]
        [Key]
        [DisplayName("آی دی محصول")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("آی دی گروه محصول")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = ("نام محصول را وارد نمائید"))]
        //[MaxLength(120)]
        ////[Column(TypeName = "NVarchar")]
        [DisplayName("نام محصول")]
        public string Name { get; set; }

        ////[Column(TypeName = "NVarchar")]
        [Required(ErrorMessage = ("رنگ محصول را وارد نمائید"))]
        [DisplayName("رنگ")]
        ////[MaxLength(100)]
        public string Color { get; set; }

        [Required(ErrorMessage = ("کد رنگ محصول را وارد نمائید "))]
        [DisplayName("کد رنگ")]
        //[Column(TypeName = "NVarchar")]
        //[MaxLength(100)]
        public string CodeColor { get; set; }


    }
}



