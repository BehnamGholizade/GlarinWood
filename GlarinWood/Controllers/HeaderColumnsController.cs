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
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace GlarinWood.Controllers
{
    public class HeaderColumnsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        public HeaderColumnsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        // GET: HeaderColumns
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.HeaderColumn.ToListAsync());
        }
        //List
        public IActionResult List(int pageid = 1)
        {
            //ViewData["CategoryViewId"] = id;
            int skip = (pageid - 1) * 8;

            var applicationDbContext = _context.HeaderColumn.Include(p => p.HeaderColumnsDesS).ToList();

            //var HeaderId = applicationDbContext.Where(p => p.CategoryId == id);
            int Count = _context.HeaderColumn.Count();
            ViewBag.PageID = pageid;
            //var ProductId = applicationDbContext.Where(p => p.CategoryId == id);

            ViewBag.PageCount = Count / 8;
            var list = applicationDbContext.OrderBy(p => p.Id).Skip(skip).Take(8).ToList();


            //if (ProductId==null)
            //{
            //    RedirectToAction("Index");
            //}
            //var applicationDbContext = _context.Product.Include(p => p.Category);

            return View(list);
        }

        // GET: Products/ModalDetails
        public async Task<IActionResult> ModalDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumn = await _context.HeaderColumn
                .Include(p => p.HeaderColumnsDesS)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumn == null)
            {
                return NotFound();
            }
            var listDes = new List<HeaderColumnsDes>();

            // Get the list of Users in this Role
            foreach (var item in _context.HeaderColumnsDes.ToList())
            {
                if (item.HeaderColumnId == id)
                {
                    listDes.Add(item);


                }
            }

            ViewBag.ListDes = listDes;
            ViewBag.ListDesCount = listDes.Count();
            return PartialView(headerColumn);
        }
        // GET: HeaderColumns/Details/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumn = await _context.HeaderColumn
                .SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumn == null)
            {
                return NotFound();
            }
            var listDes = new List<HeaderColumnsDes>();

            // Get the list of Users in this Role
            foreach (var item in _context.HeaderColumnsDes.ToList())
            {
                if (item.HeaderColumnId==id)
                {
                    listDes.Add(item);
                  
                        
                }
            }

            ViewBag.ListDes = listDes;
            ViewBag.ListDesCount = listDes.Count();

            return View(headerColumn);
        }
        [Authorize(Roles = "Administrator")]

        // GET: HeaderColumns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HeaderColumns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,dEs,Name")] HeaderColumn headerColumn, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                 
                if (file == null || file.Length == 0)
                {
                    headerColumn.Image = "1.jpg";

                }
                else
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage/ProductColumn/");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }
                    //جهت جلوگیری از نام تکراری فایل و جایگزینی
                    String guid = Guid.NewGuid().ToString();
                    headerColumn.Image = guid + file.FileName;
                    using (Image img = Image.FromStream(file.OpenReadStream()))
                    {
                        Stream ms = new MemoryStream(img.Resize(272, 334).ToByteArray());

                        //return new FileStreamResult(ms, "image/jpg");

                        var filePath = Path.Combine(uploadsRootFolder, guid + file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }

                }

                _context.Add(headerColumn);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create","HeaderColumnsDes",new { id=headerColumn.Id});
            }
            return View(headerColumn);
        }

        // GET: HeaderColumns/Edit/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumn = await _context.HeaderColumn.SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumn == null)
            {
                return NotFound();
            }
            return View(headerColumn);
        }

        // POST: HeaderColumns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,dEs,Name")] HeaderColumn headerColumn, IFormFile file)
        {
            if (id != headerColumn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //                    //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 باز بودن ترک قبل از عملیات پردازش EF7
                    var original = _context.HeaderColumn.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file != null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage/ProductColumn/");
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
                        headerColumn.Image = file.FileName;
                        using (Image img = Image.FromStream(file.OpenReadStream()))
                        {
                            Stream ms = new MemoryStream(img.Resize(272, 334).ToByteArray());

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
                        headerColumn.Image = original.Image;
                    }

                    _context.Update(headerColumn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeaderColumnExists(headerColumn.Id))
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
            return View(headerColumn);
        }

        // GET: HeaderColumns/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumn = await _context.HeaderColumn
                .SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumn == null)
            {
                return NotFound();
            }

            return View(headerColumn);
        }

        // POST: HeaderColumns/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.HeaderColumn.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage/ProductColumn/");
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
            var headerColumn = await _context.HeaderColumn.SingleOrDefaultAsync(m => m.Id == id);
            _context.HeaderColumn.Remove(headerColumn);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool HeaderColumnExists(int id)
        {
            return _context.HeaderColumn.Any(e => e.Id == id);
        }
    }
}
