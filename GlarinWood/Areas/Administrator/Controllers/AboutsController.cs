using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Areas.Administrator.Models;
using GlarinWood.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;

namespace GlarinWood.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class AboutsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public AboutsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        // GET: Administrator/Abouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.About.ToListAsync());
        }

        // GET: Administrator/Abouts/Details/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.About
                .SingleOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Administrator/Abouts/Create
        [Authorize(Roles = "Administrator")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/Abouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Image")] About about, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                 if (file == null || file.Length == 0)
                {
                    about.Image = "1.jpg";

                }
                else
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/About");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }

                    about.Image = file.FileName;
                    using (Image img = Image.FromStream(file.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(432, 520).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }

                }
                _context.Add(about);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        // GET: Administrator/Abouts/Edit/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.About.SingleOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        // POST: Administrator/Abouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image")] About about, IFormFile file)
        {
            if (id != about.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var original = _context.About.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file != null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/About");
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
                        about.Image = file.FileName;
                        using (Image img = Image.FromStream(file.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(432, 520).ToByteArray());

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
                        about.Image = original.Image;
                    }

                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id))
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
            return View(about);
        }

        // GET: Administrator/Abouts/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.About
                .SingleOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Administrator/Abouts/Delete/5
        [Authorize(Roles = "Administrator")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.About.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/About");
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
            var about = await _context.About.SingleOrDefaultAsync(m => m.Id == id);
            _context.About.Remove(about);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool AboutExists(int id)
        {
            return _context.About.Any(e => e.Id == id);
        }
    }
}
