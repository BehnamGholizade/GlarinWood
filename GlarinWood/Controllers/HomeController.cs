using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using GlarinWood.Models;
using GlarinWood.Services;
using Microsoft.AspNetCore.Http;
using GlarinWood.Data;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Areas.Administrator.ViewModels;
using GlarinWood.Areas.Administrator.Models;

namespace GlarinWood.Controllers
{
    public class HomeController : Controller

    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public HomeController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;


        }
        //تست خطا
        //public IActionResult throwError()
        //{
        //    //a New Exception For Test
        //    throw new Exception();
        //    return View();
        //}

        // GET: Sliders
        public IActionResult Index()
        {
            Slider_IndexViewModel slider_index = new Slider_IndexViewModel();

            slider_index.slider = _context.Slider.OrderByDescending(p => p.Id).FirstOrDefault();
            slider_index.picGallery = _context.PicGallery.OrderByDescending(p => p.Id).Take(6).ToList();
            if (slider_index.picGallery.Count != 0 && _context.Slider.Count() != 0)
            {
                return View(slider_index);

            }



            return View();
        }
        public IActionResult views()
        {
           


            return View();
        }
        public IActionResult About()
        {
            About_ProjectViewModels _AP = new About_ProjectViewModels();

            ViewData["Message"] = "درباره ما.";
            _AP.Project_Us = _context.Projects.ToList();

            _AP.About_Us = _context.About.FirstOrDefault();
            if (_AP.About_Us == null || _AP.Project_Us == null)
            {
                return View("Error");
            }

            return View(_AP);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ContactViewModel _CVM = new ContactViewModel();

            _CVM.Contacts = _context.Contact.FirstOrDefault();
           if (_CVM.Contacts == null)
            {
                return View("Error");
            }

            //ViewData["Message"] = "Your contact page.";

            return View(_CVM);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel _CVM)
        {
            if (ModelState.IsValid)
            {


                _emailSender.SendEmailAsync(_CVM.Email, _CVM.Subject, _CVM.Message);

                //await _emailSender.SendEmailAsync(email, subject, message);
                //ViewData["Message"] = "Thank you for Contacting us ";
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult View1()
        {
            return View();
        }
    }
}
