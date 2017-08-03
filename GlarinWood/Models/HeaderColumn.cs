using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class HeaderColumn
    {

        public HeaderColumn()
        {
            this.HeaderColumnsDesS = new HashSet<HeaderColumnsDes>();
           }

        [Required(ErrorMessage = "آی دی راوارد نمائید")]
        [Key]
        [DisplayName("آی دی سرستون")]
           public int Id { get; set; }
    

        [Required(ErrorMessage = ("توضیحات را وارد نمائید"))]
        [DisplayName("توضیحات")]
        //[Display(Name = "توضیحات")]
        public string dEs { get; set; }
        [Required(ErrorMessage = ("نام سرستون را وارد نمائید"))]
        [DisplayName("نام سرستون ")]
        //[Display(Name = "توضیحات")]
        public string Name { get; set; }




        [DisplayName("تصویر سرستون")]
        [Display(Name = "تصویر سرستون")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }
     
        [DisplayName("توضیحات سرستونها")]

        public virtual ICollection<HeaderColumnsDes> HeaderColumnsDesS { get; set; }





    }
}
