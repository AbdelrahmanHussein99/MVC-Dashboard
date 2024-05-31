using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DAL.Models;
using MVC.PL.Helper;
using MVC.PL.ViewModels;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MVC.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		#region Index
		public async Task<IActionResult> Index(string SearchInput)
		{
			var Users = Enumerable.Empty<UserViewModel>();

			if (string.IsNullOrEmpty(SearchInput))
			{
				Users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
			}
			else
			{
				Users = await _userManager.Users.Where(E => E.Email.ToLower().Contains(SearchInput.ToLower()))
																			.Select(U => new UserViewModel()
																			{
																				Id = U.Id,
																				FirstName = U.FirstName,
																				LastName = U.LastName,
																				Email = U.Email,
																				Roles = _userManager.GetRolesAsync(U).Result
																			}).ToListAsync();
			}
			return View(Users);
		}
        #endregion



        #region Details
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
          var userFormDb = await _userManager.FindByIdAsync(id);
			if (userFormDb is null)
				return BadRequest();
			var user = new UserViewModel()
			{
				Id = userFormDb.Id,
				FirstName = userFormDb.FirstName,
				LastName = userFormDb.LastName,
				Email = userFormDb.Email,
				Roles = _userManager.GetRolesAsync(userFormDb).Result
			};

            return View(viewName, user);
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
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {

                var userFormDb = await _userManager.FindByIdAsync(id);
                if (userFormDb is null)
                    return BadRequest();

                userFormDb.FirstName = model.FirstName;
                userFormDb.LastName = model.LastName;
                userFormDb.Email = model.Email;
               await _userManager.UpdateAsync(userFormDb);

                return RedirectToAction(nameof(Index));
                
            }

            return View();
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

        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();


            if (ModelState.IsValid)
            {

                var userFormDb = await _userManager.FindByIdAsync(id);
                if (userFormDb is null)
                    return BadRequest();

                await _userManager.DeleteAsync(userFormDb);
                return RedirectToAction(nameof(Index));
            } 
            


            return View();
        }
        #endregion
    }
}
