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
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using GlarinWood.Services;
using Microsoft.Net.Http.Headers;
//using ResizeImageASPNETCore;


namespace GlarinWood.Controllers
{
    //[Area("Administrator")]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
 
        private readonly IHostingEnvironment _environment;
        public ProductsController(ApplicationDbContext context, IHostingEnvironment environment, IHttpContextAccessor httpcontext)
        {
            _contextAccessor = httpcontext;
            _environment = environment;
            _context = context;
        }
        //[Area("Administrator")]
        [Authorize(Roles = "Administrator")]

        // GET: Products
        public async Task<IActionResult> Index(string ProductGener, string searchString, int pageid = 1)
        {

            //ViewData["CategoryViewId"] = id;
            int skip = (pageid - 1) * 8;

            //var filterQuery = from m in _context.Product.Include(p => p.Category) orderby m.Category select m.Category;
            //var genreQuery = _context.Product.Include(k => k.Category).Select(kp => kp.Category.Name);
            var genreQuery = _context.Category.Select(p => p.Name);

            //var filterQuery = new SelectList(_context.Category, "Id", "Name");

            var products = await _context.Product.Include(j => j.Category).ToListAsync();


            //var applicationDbContext = _context.Product.Include(p => p.Category);
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString)).ToList();
            }

            if (!String.IsNullOrEmpty(ProductGener))
            {
                products = products.Where(x => x.Category.Name == ProductGener).ToList();
            }

            int Count = products.Count();
            ViewBag.PageID = pageid;
            ViewBag.searchString = searchString;
            //با ویومدل هم می شود ارسال نمود
            ViewBag.ProductGener = ProductGener;
            ViewBag.PageCount = Count / 8;
            var list = products.OrderBy(p => p.Id).Skip(skip).Take(8).ToList();

            var productFilterViewModelVM = new ProductFilterViewModel();
            productFilterViewModelVM.geners = new SelectList(await genreQuery.ToListAsync());
            //ViewBag.Name = filterQuery;

            //productFilterViewModelVM.products = new SelectList(await filterQuery.Distinct().ToListAsync());
            productFilterViewModelVM.products = list;
            return View(productFilterViewModelVM);
        }

        public IActionResult List(int? id, int pageid = 1)
        {
        
            ViewData["CategoryViewId"] = id;
             int skip = (pageid - 1) * 8;

            var ProductId = _context.Product.Include(p => p.Category).Where(p => p.CategoryId == id);

            //var ProductId = applicationDbContext.Where(p => p.CategoryId == id);
            int Count = ProductId.Count();
            ViewBag.PageID = pageid;

            ViewBag.PageCount = Count / 8;
            var list = ProductId.OrderBy(p => p.Id).Skip(skip).Take(8).ToList();


         

            return View(list);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        //public ActionResult AddToCart(int id, int quantity,int idc)
        //{

        //    var Cart = new ShoppingCart(_contextAccessor, _context);
        //    Cart.Add(id, quantity);
        //    var product = _context.Product
        //         .Include(p => p.Category)
        //         .SingleOrDefault(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return PartialView(product);
        //    //return RedirectToAction("List", "Products",  p);
        //}
        // GET: Products/ModalDetails
        [HttpGet]
        public async Task<IActionResult> ModalDetails(int id)
        {    //string referer = _contextAccessor.HttpContext.Request.Query["ReturnUrl"]; //getting null
            //ViewData["referer"] = _contextAccessor.HttpContext.Request.Headers["Referer"].ToString(); //getting ""
            //string urlReferrer = _headerDictionary[HeaderNames.Referer].ToString();
            //string referer1 = _contextAccessor.HttpContext.Request.Query["ReturnUrl"]; //getting null
            //ViewData["referer"] = _contextAccessor.HttpContext.Request.Headers["Referer"].ToString(); //getting ""
            //بازگشت به صفحه ی کنونی ، بعد از افزودن
            //string referer = Request.Path.Value;
            //string pathLocal= Request.Headers["Referer"].ToString();



            var product = await _context.Product
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView(product);
        }




        [HttpPost]
        public IActionResult ModalDetails(int id, int quantity)
        {
 

            var Cart = new ShoppingCart(_contextAccessor, _context);

            Cart.Add(id, quantity);
         //  var n =  _context.Product
         //.Include(p => p.Category).Where(p=>p.Id == id).Select(o=>o.CategoryId);
            //return View(productCat);

            //if (id==null)
            //{
            //   return NotFound();
            //}

            //var product = await _context.Product
            //    .Include(p => p.Category)
            //    .SingleOrDefaultAsync(m => m.Id == id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            return new NoContentResult();
         }

        [Authorize(Roles = "Administrator")]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Name,Color,Price,CategoryId,Keywords,Description,Tags")] Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {


                //        image.Resize(yourWidth, yourHeight)
                //.Save(output);
                //** FileName returns "fileName.ext"(with double quotes) in beta 3 **//
                //var fileName = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue
                // .Parse(file.ContentDisposition)
                //  .FileName
                //   .Trim('"');
                if (file == null || file.Length == 0)
                {
                    product.Image = "1.jpg";

                }

                else
                {


                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }
                    //جهت جلوگیری از نام تکراری فایل و جایگزینی
                    String guid = Guid.NewGuid().ToString();
                    product.Image = guid + file.FileName;
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

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Color,Price,CategoryId,Keywords,Description,Tags")] Product product, IFormFile file)
        {

            //Product p = db.Products.Single(e => e.TagID == id);
            //یا این یکی
            //Entity1 objEntity1 = _context.Entity1.AsNoTracking().Single(s => s.Id == entity1.Id);

            if (id != product.Id)
            {
                return NotFound();
            }
            //_fileProvider = new PhysicalFileProvider(Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage"));
            ////physicalProvider.GetDirectoryContents(uploadsRootFolder);
            //IFileInfo j = _fileProvider.GetFileInfo(product.Image);
            //FileInfo f = new FileInfo(j.PhysicalPath);
            //f.Delete();
            if (ModelState.IsValid)
            {
                try
                {
                    //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var original = _context.Product.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file != null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage");
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
                        product.Image = file.FileName;
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
                        product.Image = original.Image;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.Product.AsNoTracking().FirstOrDefault(p => p.Id == id);

            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/img/product/AllProductImage");
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
            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
