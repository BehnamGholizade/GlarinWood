using GlarinWood.Areas.Administrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Areas.Administrator.ViewModels
{
    public class About_ProjectViewModels
    {
        public About About_Us { get; set; }
        public IList<Projects> Project_Us { get; set; }
    }
}
