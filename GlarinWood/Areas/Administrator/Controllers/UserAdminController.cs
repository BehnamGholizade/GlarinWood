using GlarinWood.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlarinWood.Models.AccountViewModels;

namespace GlarinWood.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    [Authorize(Roles = "Administrator")]
    public class UsersAdminController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;


        ////public UsersAdminController()
        ////{
        ////}

        public UsersAdminController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

       
         

        //
        // GET: /Users/
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "خطای رخ داده است");
            }
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);

            ViewBag.RoleNames = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<IActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                var adminresult = await _userManager.CreateAsync(user, userViewModel.Password).ConfigureAwait(false);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await _userManager.AddToRolesAsync(user, selectedRoles).ConfigureAwait(false);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", "کاربر ثبت نشد");
                            ViewBag.RoleId = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "خطا رخ داده است");
                    ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "خطا رخ داده است");
                return View();

            }
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false); 
            if (user == null)
            {
                ModelState.AddModelError("", "نقش موجود نمی باشد");
                return View();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(editUser.Id).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("", "نقش موجود نیست");
                    return View();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                selectedRole = selectedRole ?? new string[] { };

                var result = await _userManager.AddToRolesAsync(user, selectedRole.Except(userRoles).ToArray<string>()).ConfigureAwait(false);

                if (!result.Succeeded)
                {

                    ModelState.AddModelError("", "خطا رخ داده است");
                    return View();
                }
                result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRole).ToArray<string>()).ConfigureAwait(false);

                if (!result.Succeeded)
                {

                    ModelState.AddModelError("", "خطا رخ داده است");
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "خطای رخ داده است");
                return View();
            }
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                ModelState.AddModelError("", "نقش موجود نمی باشد");
                return View();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        //
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    ModelState.AddModelError("", "خطای رخ داده است");
                    return View();
                }

                var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("", "نقش موجود نمی باشد");
                    return View();

                }
                var result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "حذف انجام نشد");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
