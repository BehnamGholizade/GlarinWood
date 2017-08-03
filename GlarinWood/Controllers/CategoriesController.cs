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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;

namespace GlarinWood.Controllers
{
    //[Area("Administrator")]

    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;


        public CategoriesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context; 
            _environment = environment;

        }
        [Authorize(Roles = "Administrator")]
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            //switch (switch_on)
            //{
            //    case "1": { break; }
            //    case "2": { break; }
            //    case "3": { break; }
            //    default:
            //        break;
            //}
            return View(await _context.Category.ToListAsync());
        }
        public async Task<IActionResult> List_Category()
        {
            return View(await _context.Category.ToListAsync());
        }
        [Authorize(Roles = "Administrator")]

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image")] Category category, IFormFile file)
        {
            if (ModelState.IsValid)
            {

                if (file == null || file.Length == 0)
                {
                    category.Image = "1.jpg";

                }
                else
                {
 
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/Category");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }
                    //جهت جلوگیری از نام تکراری فایل و جایگزینی
                    String guid = Guid.NewGuid().ToString();
                    category.Image = guid+file.FileName;

                    //جهت تعیین اندازه تصویر
                    using (Image img = Image.FromStream(file.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(372, 246).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, guid + file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }

                }



                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image")] Category category, IFormFile file)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
             //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var original = _context.Category.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file != null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/Category");
                        DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
                        //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);
                        //چک میکنیم تا عکس پیش فرض رو پاک نکند
                        if (original.Image != "1.jpg")
                        {

                            foreach (FileInfo fileEF in directory.GetFiles())
                            {
                                if (fileEF.Name.Equals(original.Image))
                                {

                                    fileEF.Delete();
                                }


                            }
                        }

                        category.Image = file.FileName;
                        using (Image img = Image.FromStream(file.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(372, 246).ToByteArray());

                            //return new FileStreamResult(ms, "image/jpg");

                            var filePath = Path.Combine(uploadsRootFolder, file.FileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                            }
                        }

                    }
                    else
                    {
                        category.Image = original.Image;
                    }


                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.Category.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/Category");
            DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
            //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);
            //چک میکنیم تا عکس پیش فرض رو پاک نکند
            if (original.Image != "1.jpg")
            {
                foreach (FileInfo fileInfo in directory.GetFiles())
            {
                if (fileInfo.Name.Equals(original.Image))
                {
                    fileInfo.Delete();
                }
            }
            }

            var category = await _context.Category.SingleOrDefaultAsync(m => m.Id == id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
