 using GlarinWood.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GlarinWood.Areas.Administrator.Controllers
{
    //[Area(AreaConstants.IdentityArea)]
    [Area("Administrator")]

    [Authorize(Roles = "Administrator")]
    public class RolesAdminController : Controller
    {
        #region
        private RoleManager<IdentityRole> _roleManager;

        private UserManager<ApplicationUser> _userManager;


        #endregion



        public RolesAdminController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    set
        //    {
        //        _userManager = value;
        //    }
        //}

        //private ApplicationRoleManager _roleManager;
        //public ApplicationRoleManager RoleManager
        //{
        //    get
        //    {
        //        return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
        //    }
        //    private set
        //    {
        //        _roleManager = value;
        //    }
        //}

        //
        // GET: /Roles/
        [Authorize(Roles = "Administrator")]

        public ViewResult Index() => View(_roleManager.Roles);
        [Authorize(Roles = "Administrator")]

        public IActionResult Create() => View();

        // GET: /Roles/Details/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "خطا رخ داده است");
                return View();
            }
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        // POST: /Roles/Details/5
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);

                var roleresult = await _roleManager.CreateAsync(role).ConfigureAwait(false);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", "خطای رخ داده است");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }


        //
        // GET: /Roles/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "خطا رخ داده است");
                return View();
            }
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
            if (role == null)
            {
                ModelState.AddModelError("", "نقش موجود نمی باشد");
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [Authorize(Roles = "Administrator")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                if (id == null)
                {
                    ModelState.AddModelError("", "خطا رخ داده است");
                    return View();
                }
                var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
                if (role == null)
                {
                    ModelState.AddModelError("", "نقش موجود نمی باشد");
                    return View();
                }
                if (deleteUser != null)
                {
                    result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
                }
                else
                {
                    result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "نقش حذف نشد");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }



        // GET: /Roles/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "خطا رخ داده است");
                return View();

            }

            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
            if (role == null)
            {
                ModelState.AddModelError("", "نقش موجود نمی باشد");
                return View();

            }
            //RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };

            //return View(roleModel);
            return View(new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            });
        }
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id).ConfigureAwait(false);
                //if (role == null)
                //{
                //    ModelState.AddModelError("", "نقش موجود نیست");
                //}
                //else
                //{
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");

                    }
                    //AddErrorsFromResult(result);
                    ModelState.AddModelError("", "آپدیت انجام نشد");
                //}

            }
            return View();
        }

        //private void AddErrorsFromResult(IdentityResult result)
        //{
        //    foreach (IdentityError error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error.Description);
        //    }
        //}
    }
}
