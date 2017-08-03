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
using Microsoft.AspNetCore.Authorization;

namespace GlarinWood.Controllers
{
    public class Download_FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public Download_FilesController(ApplicationDbContext context, IHostingEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;

        }
        [Authorize(Roles = "Administrator")]

        // GET: Download_Files
        public async Task<IActionResult> Index()
        {
            return View(await _context.Download_Files.ToListAsync());
        }
        [Authorize(Roles = "Administrator")]

        // GET: Download_Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var download_Files = await _context.Download_Files
                .SingleOrDefaultAsync(m => m.Id == id);
            if (download_Files == null)
            {
                return NotFound();
            }

            return View(download_Files);
        }
        [Authorize(Roles = "Administrator")]

        // GET: Download_Files/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]

        // POST: Download_Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,File")] Download_Files download_Files, IFormFile file)
        {
            if (ModelState.IsValid)
            {
 
                if (file == null || file.Length == 0)
                {
                    download_Files.File = "emptyFile";

                }
                else
                {
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/Files");
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }

                    download_Files.File = file.FileName;
                    var filePath = Path.Combine(uploadsRootFolder, file.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream).ConfigureAwait(false);
                    }

                }



                _context.Add(download_Files);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(download_Files);
        }
        [Authorize(Roles = "Administrator")]

        // GET: Download_Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var download_Files = await _context.Download_Files.SingleOrDefaultAsync(m => m.Id == id);
            if (download_Files == null)
            {
                return NotFound();
            }
            return View(download_Files);
        }
        [Authorize(Roles = "Administrator")]

        // POST: Download_Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,File")] Download_Files download_Files,IFormFile file)
        {
            if (id != download_Files.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  //جهت برطرف کردن خطای سازنده باز بودن ترک قبل از عملیات پردازش EF7 
                    var originaldownload_Files = _context.Download_Files.AsNoTracking().FirstOrDefault(p => p.Id == id);
                    if (file != null)
                    {
                        var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/Files");
                        DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
                        //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);

                        foreach (FileInfo fileEF in directory.GetFiles())
                        {
                            if (fileEF.Name.Equals(originaldownload_Files.File))
                            {
                                fileEF.Delete();
                            }
                        }

                        download_Files.File = file.FileName;
                        var filePath = Path.Combine(uploadsRootFolder, file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        download_Files.File = originaldownload_Files.File;
                    }


                    _context.Update(download_Files);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Download_FilesExists(download_Files.Id))
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
            return View(download_Files);
        }
        [Authorize(Roles = "Administrator")]

        // GET: Download_Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var download_Files = await _context.Download_Files
                .SingleOrDefaultAsync(m => m.Id == id);
            if (download_Files == null)
            {
                return NotFound();
            }

            return View(download_Files);
        }
        [Authorize(Roles = "Administrator")]

        // POST: Download_Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var original = _context.Download_Files.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/Files");
            DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
            //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);

            foreach (FileInfo fileInfo in directory.GetFiles())
            {
                if (fileInfo.Name.Equals(original.File))
                {
                    fileInfo.Delete();
                }
            }

            var download_Files = await _context.Download_Files.SingleOrDefaultAsync(m => m.Id == id);
            _context.Download_Files.Remove(download_Files);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool Download_FilesExists(int id)
        {
            return _context.Download_Files.Any(e => e.Id == id);
        }

        //[Authorize(Roles = "User")]
        public ActionResult Download(int? id)
        {
            var originaldownload_Files = _context.Download_Files.Find(id);

             //DirectoryInfo dirInfo = new DirectoryInfo(_environment.MapPath("~/MyFiles/"));

            //VIP & PaymentUser can be one condition!


            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "metro/Files");
            DirectoryInfo directory = new DirectoryInfo(uploadsRootFolder);
            //Directory.GetFiles(uploadsRootFolder + originalPerson.Image);

            //foreach (FileInfo fileEF in directory.GetFiles())
            //{
            //    if (fileEF.Name.Equals(originaldownload_Files.File))
            //    {
            //        fileEF.Delete();
            //    }
            //}
            #region Free


            string strFileExtension = string.Empty;
                string strLength = string.Empty;

                foreach (FileInfo fileEF in directory.GetFiles())
                {
                    if (fileEF.Name.Equals(originaldownload_Files.File))
                    {
                        strFileExtension = fileEF.Extension;
                        strLength = fileEF.Length.ToString();
                    }
                }

                if (id == null)
                {
                    return View("Error");
                }

                int index = Convert.ToInt32(id);

                //اگر قصد داشته باشیم فایل های دانلود فقط در پوشه و نه دیتابیس ذخیره گردند
                //var filesCol = obj.GetFiles();
                //string CurrentFileName = (from hARCHI in filesCol
                //                          where hARCHI.FileId == id
                //                          select hARCHI.FilePath).First();

                string CurrentFileName = Path.Combine(uploadsRootFolder, originaldownload_Files.File);


            string contentType = string.Format("application/" + strFileExtension);
            byte[] fileBytes = System.IO.File.ReadAllBytes(CurrentFileName);

            return File(fileBytes, contentType, CurrentFileName);

            #endregion Free

            //if (User.Identity.IsAuthenticated)
            //{
            //    ViewBag.IsPaymentUser = downLoadFileInformation.IsPaymentUser;
            //    ViewBag.IsVIP = downLoadFileInformation.IsVIP;
            //    ViewBag.User = User.Identity.Name;
            //    ViewBag.ID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //    return View("Authorize");
            //}

            //ViewBag.IsPaymentUser = false;
            //ViewBag.IsVIP = false;
            //return View("Authorize");

            //VIP & PaymentUser can be one condition!

            #region VIP
            //if (User.IsInRole("VIP") && downLoadFileInformation.IsVIP == true)
            //{
            //    string strFileExtension = string.Empty;
            //    string strLength = string.Empty;

            //    foreach (var item in dirInfo.GetFiles())
            //    {
            //        if (downLoadFileInformation.FilePathName == item.Name)
            //        {
            //            strFileExtension = item.Extension;
            //            strLength = item.Length.ToString();
            //        }
            //    }

            //    if (id == null)
            //    {
            //        return View("Error");
            //    }

            //    int index = Convert.ToInt32(id);
 

            //    string CurrentFileName = dirInfo + downLoadFileInformation.FilePathName;

            //    string contentType = string.Format("application/" + strFileExtension);

            //    return File(CurrentFileName, contentType, CurrentFileName);
            //}
            #endregion VIP

            #region PaymentUser

            //if (User.IsInRole("PaymentUser") && downLoadFileInformation.IsPaymentUser == true)
            //{
            //    string strFileExtension = string.Empty;
            //    string strLength = string.Empty;

            //    foreach (var item in dirInfo.GetFiles())
            //    {
            //        if (downLoadFileInformation.FilePathName == item.Name)
            //        {
            //            strFileExtension = item.Extension;
            //            strLength = item.Length.ToString();
            //        }
            //    }

            //    if (id == null)
            //    {
            //        return View("Error");
            //    }

            //    int index = Convert.ToInt32(id);

               

                //string CurrentFileName = dirInfo + downLoadFileInformation.FilePathName;

                //string contentType = string.Format("application/" + strFileExtension);

                //return File(CurrentFileName, contentType, CurrentFileName);
            }
            #endregion PaymentUser





        }

    }
 
