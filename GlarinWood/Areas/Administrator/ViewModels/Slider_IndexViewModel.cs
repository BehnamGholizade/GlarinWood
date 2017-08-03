using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlarinWood.Areas.Administrator.Models;
using GlarinWood.Models;


namespace GlarinWood.Areas.Administrator.ViewModels
{
    public class Slider_IndexViewModel
    {
        public Slider slider { get; set; }
        public IList<PicGallery> picGallery { get; set; }
    }
}
