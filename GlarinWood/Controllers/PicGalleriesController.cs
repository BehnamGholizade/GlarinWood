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
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

   //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
//var originalProduct = _context.Product.AsNoTracking().FirstOrDefault(p => p.Id == id);

namespace GlarinWood.Controllers

{
    public class PicGalleriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public PicGalleriesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public IActionResult List()
        {
            //, int pageid = 1
            //ViewData["PicgcViewId"] = id;
            //int skip = (pageid - 1) * 8;

            //var applicationDbContext = _context.PicGallery.ToList();

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

            return View(_context.PicGallery.ToList());
        }
        // GET: PicGalleries
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.PicGallery.ToListAsync());

        }
        [Authorize(Roles = "Administrator")]

        // GET: PicGalleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PGC = _context.PicCollectionGallery.Where(p => p.PicGalleryId == id).ToList();
            if (PGC == null)
            {
                return NotFound();
            }
            ViewBag.PGC = PGC;
            ViewBag.PGCCount = PGC.Count();
            //ViewData["PGC"] = PGC.ToList();
            //ViewData["PGCCount"] = PGC.Count();


            var picGallery = await _context.PicGallery
                .SingleOrDefaultAsync(m => m.Id == id);
            if (picGallery == null)
            {
                return NotFound();
            }

            return View(picGallery);
        }
        [Authorize(Roles = "Administrator")]

        // GET: PicGalleries/Create
        public IActionResult Create()
        {
            //PGCViewModel pgc = new PGCViewModel();
            //pgc.picCollectionGallery = new PicCollectionGallery();
            //pgc.picGallery = new PicGallery();

            //return View(pgc);
            return View();
        }
        [Authorize(Roles = "Administrator")]

        // POST: PicGalleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Description,Keywords,Tags")] PicGallery picGallery, IFormFile file, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {

                if (file == null || file.Length == 0 && files == null)
                {
                    picGallery.Image = "1.jpg";
                    //pgcViewModel.picCollectionGallery. = "2.jpg";

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
                    
                    picGallery.Image = guid+file.FileName;
                    using (Image img = Image.FromStream(file.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(470, 480).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, guid+file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }
                    _context.Add(picGallery);
                    //await _context.SaveChangesAsync();
                    //جهت جلوگیری از نام تکراری فایل و جایگزینی
                    String guid_picgall = Guid.NewGuid().ToString();
                    foreach (var fil in files)
                    {
                        var piccg = new PicCollectionGallery();
                         {

                            piccg.Image = guid_picgall + fil.FileName;
                            piccg.PicGalleryId = picGallery.Id;
                        }
                        //using (Image img = Image.FromStream(files.OpenReadStream()))
                        //{
                        //    Stream ms = new MemoryStream(img.Resize(1920, 585).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, guid_picgall+fil.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await fil.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                        _context.Add(piccg);
                    }
                }
              
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(picGallery);

        }


        [Authorize(Roles = "Administrator")]

        // GET: PicGalleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
            var PGC = _context.PicCollectionGallery.Where(p => p.PicGalleryId == id).ToList();
            if (PGC == null)
            {
                return NotFound();
            }
            ViewBag.PGC = PGC;
            ViewBag.PGCCount = PGC.Count();
            var picGallery = await _context.PicGallery.SingleOrDefaultAsync(m => m.Id == id);
        if (picGallery == null)
        {
            return NotFound();
        }
        return View(picGallery);
    }
        [Authorize(Roles = "Administrator")]

        // POST: PicGalleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Description,Keywords,Tags")] PicGallery picGallery, IFormFile file)
    {
        if (id != picGallery.Id)
        {
            return NotFound();
        }
         
            if (ModelState.IsValid)
        {
            try
            {
                                  //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var original = _context.PicGallery.AsNoTracking().FirstOrDefault(p => p.Id == id);
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
                        picGallery.Image = file.FileName;
                        using (Image img = Image.FromStream(file.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(470, 480).ToByteArray());

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
                        picGallery.Image = original.Image;
                    }


                    //var PGC = _context.PicCollectionGallery.Where(p => p.PicGalleryId == id).ToList();

                    //if (PGC == null)
                    //{
                    //    return NotFound();
                    //}

                    _context.Update(picGallery);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PicGalleryExists(picGallery.Id))
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
        return View(picGallery);
    }
        [Authorize(Roles = "Administrator")]

        // GET: PicGalleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var picGallery = await _context.PicGallery
            .SingleOrDefaultAsync(m => m.Id == id);
        if (picGallery == null)
        {
            return NotFound();
        }

        return View(picGallery);
    }
        [Authorize(Roles = "Administrator")]

        // POST: PicGalleries/Delete/5
        [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
            var PGC = _context.PicCollectionGallery.Where(p => p.PicGalleryId == id).ToList();

            if (PGC == null)
            {
                return NotFound();
            }

            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/PicGallery");
            DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
            //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);
            //var original = _context.PicGallery.AsNoTracking().FirstOrDefault(p => p.Id == id);
            //چک میکنیم تا عکس پیش فرض رو پاک نکند
           
                foreach (FileInfo fileInfo in directory.GetFiles())
                {

                    foreach (var item in PGC)
                    {

                    if (item.Image != "1.jpg")
                    {

                        if (fileInfo.Name.Equals(item.Image))
                        {
                            fileInfo.Delete();

                        }
                    }
                    }
                }
         

            var picGallery = await _context.PicGallery.SingleOrDefaultAsync(m => m.Id == id);
        _context.PicGallery.Remove(picGallery);
            //_context.PicCollectionGallery.RemoveRange(PGC);
         
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
        [Authorize(Roles = "Administrator")]

        private bool PicGalleryExists(int id)
    {
        return _context.PicGallery.Any(e => e.Id == id);
    }
}
}
