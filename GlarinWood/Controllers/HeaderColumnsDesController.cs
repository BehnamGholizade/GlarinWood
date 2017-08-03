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

namespace GlarinWood.Controllers
{
    public class HeaderColumnsDesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HeaderColumnsDesController(ApplicationDbContext context)
        {
            _context = context;    
        }
        [Authorize(Roles = "Administrator")]

        // GET: HeaderColumnsDes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HeaderColumnsDes.Include(h => h.HeaderColumn);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "Administrator")]

        // GET: HeaderColumnsDes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumnsDes = await _context.HeaderColumnsDes
                .Include(h => h.HeaderColumn)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumnsDes == null)
            {
                return NotFound();
            }

            return View(headerColumnsDes);
        }
        [Authorize(Roles = "Administrator")]

        // GET: HeaderColumnsDes/Create
        public IActionResult Create(int id)
        {
            ViewData["HeaderColumnName"] = _context.HeaderColumn.Where(p => p.Id == id).Select(i => i.Name).FirstOrDefault().ToString();
            ViewData["HeaderColumnId"] = id;
            return View();
        }
        [Authorize(Roles = "Administrator")]

        // POST: HeaderColumnsDes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("Width,Height,Thicnekss")] HeaderColumnsDes headerColumnsDes)
        {
            if (ModelState.IsValid)
            {
                headerColumnsDes.HeaderColumnId = id;
                //headerColumnsDes.HeaderColumn = id;
                _context.Add(headerColumnsDes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create",new {headerColumnId=id });
            }
            ViewData["HeaderColumnId"] = new SelectList(_context.HeaderColumn, "Id", "Name", headerColumnsDes.HeaderColumnId);
            return View(headerColumnsDes);
        }
        [Authorize(Roles = "Administrator")]

        // GET: HeaderColumnsDes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumnsDes = await _context.HeaderColumnsDes.SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumnsDes == null)
            {
                return NotFound();
            }
            ViewData["HeaderColumnId"] = new SelectList(_context.HeaderColumn, "Id", "Name", headerColumnsDes.HeaderColumnId);
            return View(headerColumnsDes);
        }
        [Authorize(Roles = "Administrator")]

        // POST: HeaderColumnsDes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Width,Height,Thicnekss,HeaderColumnId")] HeaderColumnsDes headerColumnsDes)
        {
            if (id != headerColumnsDes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headerColumnsDes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeaderColumnsDesExists(headerColumnsDes.Id))
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
            ViewData["HeaderColumnId"] = new SelectList(_context.HeaderColumn, "Id", "Name", headerColumnsDes.HeaderColumnId);
            return View(headerColumnsDes);
        }
        [Authorize(Roles = "Administrator")]

        // GET: HeaderColumnsDes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headerColumnsDes = await _context.HeaderColumnsDes
                .Include(h => h.HeaderColumn)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (headerColumnsDes == null)
            {
                return NotFound();
            }

            return View(headerColumnsDes);
        }
        [Authorize(Roles = "Administrator")]

        // POST: HeaderColumnsDes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var headerColumnsDes = await _context.HeaderColumnsDes.SingleOrDefaultAsync(m => m.Id == id);
            _context.HeaderColumnsDes.Remove(headerColumnsDes);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator")]

        private bool HeaderColumnsDesExists(int id)
        {
            return _context.HeaderColumnsDes.Any(e => e.Id == id);
        }
    }
}
