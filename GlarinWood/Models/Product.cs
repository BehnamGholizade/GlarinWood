using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models
{
    public class Product
    {
        public Product()
        {
            //this.Orders = new HashSet<Order>();
            //this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Column(Order =1)]
        [DisplayName("آی دی گروه محصول")]
        public int CategoryId { get; set; }

        [Required]
        //[Required(ErrorMessage = "آی دی راوارد نمائید")]
        [Key]
        [DisplayName("آی دی محصول")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

     
        [Required(ErrorMessage = ("نام محصول را وارد نمائید"))]
        //[MaxLength(120)]
        ////[Column(TypeName = "NVarchar")]
        [DisplayName("نام محصول")]
        public string Name { get; set; }

        ////[Column(TypeName = "NVarchar")]
        //[Required(ErrorMessage = ("رنگ محصول را وارد نمائید"))]
        [DisplayName("رنگ")]
        ////[MaxLength(100)]
        public string Color { get; set; }

        //[Required(ErrorMessage = ("کد رنگ محصول را وارد نمائید "))]
        //[DisplayName("کد رنگ")]
        ////[Column(TypeName = "NVarchar")]
        ////[MaxLength(100)]
        //public string CodeColor { get; set; }

        ///// <summary>
        /////Fields Size 
        ///// </summary>  
        //[Required(ErrorMessage = ("طول محصول را وارد نمائید"))]
        //[DisplayName("طول")]
        //public int? Lenght { get; set; }

        ////[Required(ErrorMessage = ("عرض محصول را وارد نمائید"))]
        //[DisplayName("عرض")]
        //public int? Width { get; set; }

        ////[Required(ErrorMessage = ("ارتفاع محصول را وارد نمائید"))]
        //[DisplayName("ارتفاع")]
        //public int? Height { get; set; }

        ////[Required(ErrorMessage = ("ضخامت محصول را وارد نمائید"))]
        //[DisplayName("ضخامت")]
        //public int? Thicnekss { get; set; }

        ////[Required(ErrorMessage = ("عمق محصول را وارد نمائید"))]
        //[DisplayName("عمق")]
        //public int? Depth { get; set; }

        //[Required(ErrorMessage = ("حداقل سایز محصول را وارد نمائید"))]
        //[DisplayName("حداقل سایز")]
        //public int? MinSize { get; set; }

        //[DisplayName("حداکثر سایز")]
        //public int? MaxSize { get; set; }

        ///// <summary>
        ///// /End Fields Size
        ///// </summary>
        ////[Required(ErrorMessage = ("سری محصول را وارد نمائید"))]
        ////[DisplayName("سری محصول")]
        ////[MaxLength(120)]

        //////public decimal Series { get; set; }

        [Required(ErrorMessage = ("قیمت محصول را وارد نمائید"))]
        [DisplayName("قیمت محصول")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0 ريال}")]
        public decimal Price { get; set; }

        ////public string Url { get; set; }


        [Required(ErrorMessage = "توضیحات را وارد کنید", AllowEmptyStrings = false)]
        [DisplayName("توضیحات")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        //[Required(ErrorMessage = ("تصویر محصول را وارد نمائید"))]

        [DisplayName("تصویر محصول")]
        [Display(Name = "تصویر محصول")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }


        //[Required(ErrorMessage = "کلمه کلید را وارد کنید", AllowEmptyStrings = false)]
        [DisplayName("کلمه کلیدی ")]
        [Display(Name = "کلمه کلیدی ")]
        public string Keywords { get; set; }

        [DisplayName("تگ")]
        [Display(Name = "تگ")]
        public string Tags { get; set; }
        //public int Like { get; set; }
        //public int Dislike { get; set; }
        //public bool Enabled { get; set; }
        //[DisplayName("گالری محصولات")]
        //public virtual ICollection<ProductGallery> ProductGallerys { get; set; }

        [DisplayName("گروه محصول")]
        public virtual Category Category { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<CartItem> cartItems { get; set; }



    }
}
