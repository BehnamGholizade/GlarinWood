using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class HeaderColumnsDes
    {
       
        [Key]
        [Required]

        [DisplayName("آی دی توضیحات سرستون")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = ("عرض سرستون را وارد نمائید"))]
        [DisplayName("عرض")]
        public int Width { get; set; }

        [Required(ErrorMessage = ("ارتفاع سرستون را وارد نمائید"))]
        [DisplayName("ارتفاع")]
        public int Height { get; set; }

        [Required(ErrorMessage = ("ضخامت سرستون را وارد نمائید"))]
        [DisplayName("ضخامت")]
        public int Thicnekss { get; set; }

        
        [DisplayName("آی دی سرستون")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int HeaderColumnId { get; set; }

        [DisplayName("سرستون")]
        public virtual HeaderColumn HeaderColumn { get; set; }

    }
}
