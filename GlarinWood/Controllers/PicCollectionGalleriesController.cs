using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Data;
using GlarinWood.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace GlarinWood.Controllers

{
    public class PicCollectionGalleriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;


        public PicCollectionGalleriesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;  
            _environment = environment;

        }
        public IActionResult List(int? id)
        {
            //, int pageid = 1
            //ViewData["PicgcViewId"] = id;
            //int skip = (pageid - 1) * 8;

            var picGalleryCollection = _context.PicCollectionGallery.Where(p => p.PicGalleryId == id).ToList();
            ViewBag.count = picGalleryCollection.Count();
            if (picGalleryCollection==null)
            {
                return View();
            }
            //var picGalleryId = applicationDbContext.Where(p => p.CategoryId == id);
            //int Count = picGalleryId.Count();
            //ViewBag.PageID = pageid;

            //ViewBag.PageCount = Count / 8;
            //var list = picGalleryId.OrderBy(p => p.Id).Skip(skip).Take(8).ToList();


            //if (ProductId==null)
            //{
            //    RedirectToAction("Index");
            //}
            //var applicationDbContext = _context.Product.Include(p => p.Category);

            return View(picGalleryCollection);
        }
        [Authorize(Roles = "Administrator")]

        // GET: PicCollectionGalleries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PicCollectionGallery.Include(p => p.PicGallery);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "Administrator")]

        // GET: PicCollectionGalleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picCollectionGallery = await _context.PicCollectionGallery
                .Include(p => p.PicGallery)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (picCollectionGallery == null)
            {
                return NotFound();
            }

            return View(picCollectionGallery);
        }
        [Authorize(Roles = "Administrator")]

        // GET: PicCollectionGalleries/Create
        public IActionResult Create()
        {
            ViewData["PicGalleryId"] = new SelectList(_context.PicGallery, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Administrator")]

        // POST: PicCollectionGalleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PicGalleryId,Id,Image")] PicCollectionGallery picCollectionGallery, IFormFile file)
        {
            if (ModelState.IsValid)
            {
 
                if (file == null || file.Length == 0)
                {
                    picCollectionGallery.Image = "1.jpg";

                }
                else
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/PicGallery");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }
                    //جهت جلوگیری از نام تکراری فایل و جایگزینی
                    String guid = Guid.NewGuid().ToString();
                    picCollectionGallery.Image = guid+file.FileName;
                    using (Image img = Image.FromStream(file.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(272, 273).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, guid+file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }

                }


                _context.Add(picCollectionGallery);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PicGalleryId"] = new SelectList(_context.PicGallery, "Id", "Name", picCollectionGallery.PicGalleryId);
            return View(picCollectionGallery);
        }
        [Authorize(Roles = "Administrator")]

        // GET: PicCollectionGalleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picCollectionGallery = await _context.PicCollectionGallery.SingleOrDefaultAsync(m => m.Id == id);
            if (picCollectionGallery == null)
            {
                return NotFound();
            }
            ViewData["PicGalleryId"] = new SelectList(_context.PicGallery, "Id", "Name", picCollectionGallery.PicGalleryId);
            return View(picCollectionGallery);
        }
        [Authorize(Roles = "Administrator")]

        // POST: PicCollectionGalleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PicGalleryId,Id,Image")] PicCollectionGallery picCollectionGallery, IFormFile file)
        {
            if (id != picCollectionGallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
   //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var original = _context.PicCollectionGallery.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file != null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/PicGallery");
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
                        picCollectionGallery.Image = file.FileName;
                        using (Image img = Image.FromStream(file.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(272, 273).ToByteArray());

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
                        picCollectionGallery.Image = original.Image;
                    }

                    _context.Update(picCollectionGallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PicCollectionGalleryExists(picCollectionGallery.Id))
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
            ViewData["PicGalleryId"] = new SelectList(_context.PicGallery, "Id", "Name", picCollectionGallery.PicGalleryId);
            return View(picCollectionGallery);
        }
        [Authorize(Roles = "Administrator")]

        // GET: PicCollectionGalleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picCollectionGallery = await _context.PicCollectionGallery
                .Include(p => p.PicGallery)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (picCollectionGallery == null)
            {
                return NotFound();
            }

            return View(picCollectionGallery);
        }
        [Authorize(Roles = "Administrator")]

        // POST: PicCollectionGalleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.PicCollectionGallery.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/PicGallery");
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

            var picCollectionGallery = await _context.PicCollectionGallery.SingleOrDefaultAsync(m => m.Id == id);
            _context.PicCollectionGallery.Remove(picCollectionGallery);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool PicCollectionGalleryExists(int id)
        {
            return _context.PicCollectionGallery.Any(e => e.Id == id);
        }
    }
}
