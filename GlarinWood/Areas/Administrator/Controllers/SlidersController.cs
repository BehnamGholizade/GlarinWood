using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Data;
using GlarinWood.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using GlarinWood.Areas.Administrator.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace GlarinWood.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;


        public SlidersController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        [Authorize(Roles = "Administrator")]
        // GET: Sliders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Slider.ToListAsync());
        }

        // GET: Sliders/Details/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Slider
                .SingleOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // GET: Sliders/Create
        [Authorize(Roles = "Administrator")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title_1,Description_1,Title_2,Description_2")] Slider slider, IFormFile file1, IFormFile file2)
        {
            if (ModelState.IsValid)
            {
 

                if (file1 == null || file1.Length == 0 && file2 == null || file2.Length == 0)
                {
                    slider.Image_1 = "1.jpg";
                    slider.Image_2 = "2.jpg";

                }
                else
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/Slider");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }

                    slider.Image_1 = file1.FileName;
                    slider.Image_2 = file2.FileName;
                    using (Image img = Image.FromStream(file1.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(1920, 585).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, file1.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }

                    using (Image img = Image.FromStream(file2.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(1920, 585).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, file2.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }



                }

                _context.Add(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Sliders/Edit/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Slider.SingleOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title_1,Description_1,Title_2,Description_2")] Slider slider, IFormFile file1, IFormFile file2)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
               //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var original = _context.Slider.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file1 != null && file2 !=null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/Slider");
                        DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
                        //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);
                        //چک میکنیم تا عکس پیش فرض رو پاک نکند
                        if (original.Image_1 != "1.jpg" && original.Image_2 != "2.jpg")
                        {
                            foreach (FileInfo fileInfo in directory.GetFiles())
                        {
                            if (fileInfo.Name.Equals(original.Image_1) || fileInfo.Name.Equals(original.Image_2))
                            {
                                fileInfo.Delete();
                            }
                        }
                           }
                        slider.Image_1 = file1.FileName;
                        slider.Image_2 = file2.FileName;
                        using (Image img = Image.FromStream(file1.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(1920, 585).ToByteArray());

                            //return new FileStreamResult(ms, "image/jpg");

                            var filePath = Path.Combine(uploadsRootFolder, file1.FileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                            }
                        }

                        using (Image img = Image.FromStream(file2.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(1920, 585).ToByteArray());

                            //return new FileStreamResult(ms, "image/jpg");

                            var filePath = Path.Combine(uploadsRootFolder, file2.FileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                            }
                        }


                    }
                    else
                    {
                        slider.Image_1 = original.Image_1;
                        slider.Image_2 = original.Image_2;
                    }



                    _context.Update(slider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.Id))
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
            return View(slider);
        }

        // GET: Sliders/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Slider
                .SingleOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Sliders/Delete/5
        [Authorize(Roles = "Administrator")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.Slider.AsNoTracking().FirstOrDefault(p => p.Id == id);

            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/Slider");
            DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
            //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);
            if (original.Image_1 != "1.jpg" && original.Image_2 != "2.jpg")
            {
                foreach (FileInfo fileInfo in directory.GetFiles())
            {
                if (fileInfo.Name.Equals(original.Image_1) || fileInfo.Name.Equals(original.Image_2))
                {
                    fileInfo.Delete();
                }
            }
                  }
            var slider = await _context.Slider.SingleOrDefaultAsync(m => m.Id == id);
            _context.Slider.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool SliderExists(int id)
        {
            return _context.Slider.Any(e => e.Id == id);
        }
    }
}
