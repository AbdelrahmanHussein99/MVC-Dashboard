using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DAL.Models;
using MVC.PL.Helper;
using MVC.PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        #region Index
        public async Task<IActionResult> Index(string SearchInput)
        {
            var Roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                Roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
            }
            else
            {
                Roles = await _roleManager.Roles.Where(E => E.Name.ToLower().Contains(SearchInput.ToLower()))
                                                                            .Select(R => new RoleViewModel()
                                                                            {
                                                                                Id = R.Id,
                                                                                RoleName = R.Name

                                                                            }).ToListAsync();
            }
            return View(Roles);
        }
        #endregion

        #region Create
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role =new IdentityRole()
                { 
                    Name=model.RoleName
                };
              var result=  await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                     return RedirectToAction(nameof(Index));

                }
                ModelState.AddModelError(string.Empty, "Invalid!! ");
            }

            return View(model);
        }

        #endregion

        #region Details
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var RoleFormDb = await _roleManager.FindByIdAsync(id);
            if (RoleFormDb is null)
                return BadRequest();
            var role = new RoleViewModel()
            {
                Id = RoleFormDb.Id,
                RoleName = RoleFormDb.Name,
               
            };

            return View(viewName, role);
        }
        #endregion



        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var RoleFormDb = await _roleManager.FindByIdAsync(id);
                if (RoleFormDb is null)
                    return BadRequest();

                RoleFormDb.Name = model.RoleName;
               
                await _roleManager.UpdateAsync(RoleFormDb);

                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();


            if (ModelState.IsValid)
            {

                var RoleFormDb = await _roleManager.FindByIdAsync(id);
                if (RoleFormDb is null)
                    return BadRequest();

                RoleFormDb.Name = model.RoleName;

                await _roleManager.DeleteAsync(RoleFormDb);
                return RedirectToAction(nameof(Index));
            }



            return View();
        }
        #endregion



        #region Add Or Remove Users
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role =await _roleManager.FindByIdAsync(roleId);


            if (role is null)
                return BadRequest();

            ViewData["id"]=roleId;
            var Users =await _userManager.Users.ToListAsync();

            var UsersInRole = new List<UserInRoleViewModel>();
            foreach (var user in Users)
            {
                var UserInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    UserInRole.IsSelected=true;

                }
                else
                {
                    UserInRole.IsSelected=false;
                }
                UsersInRole.Add(UserInRole);
            }
            return View(UsersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId,List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);


            if (role is null)
                return BadRequest();

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
               
                    var AppUser =await _userManager.FindByIdAsync(user.UserId);
                    if (AppUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {

                            await _userManager.AddToRoleAsync(AppUser, role.Name);
                        }
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(AppUser, role.Name);
                        }
                    }
                    
                }
                   return RedirectToAction(nameof(Edit),new {id= roleId});

            }
           
            return View(users);
        }
        #endregion
    }
}
