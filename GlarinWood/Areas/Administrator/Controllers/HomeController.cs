using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Data;
using GlarinWood.Models;
 using Microsoft.AspNetCore.Authorization;

namespace GlarinWood.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administrator")]

    public class HomeController : Controller
    {
        //private readonly ApplicationDbContext _context;

        public HomeController( )
        {
            //_context = context;
        }
        [Authorize(Roles = "Administrator")]

        public ViewResult Index() => View();



    
    }
}
