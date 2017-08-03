using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlarinWood.Models.AccountViewModels
{
    public class Login_RegisterViewModel
    {
        public virtual LoginViewModel LoginVM { get; set; }
        public virtual RegisterViewModel RegisterVM { get; set; }
       //public string ReturnUrl;


    }
}
