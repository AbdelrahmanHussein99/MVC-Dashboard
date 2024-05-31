using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using MVC.PL.Helper;
using MVC.PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
	[Authorize]

	public class EmployeeController : Controller
	{
		
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		

		public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}




		#region index
		public async Task<IActionResult> Index(string SearchInput)
		{
			var employees = Enumerable.Empty<Employee>();
			if (string.IsNullOrWhiteSpace(SearchInput))
			{
				employees = await _unitOfWork.employeeRepository.GetAll();
			}
			else
			{
				employees = await _unitOfWork.employeeRepository.GetByName(SearchInput);
			}
			var res = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
			return View(res);
		}
		#endregion



		#region Create
		[HttpGet]

		public IActionResult Create()
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel model)
		{
			if (ModelState.IsValid)
			{
				model.ImageName = DocumentSettings.UploadFile(model.Image, "images");


				var employee = _mapper.Map<Employee>(model);

				_unitOfWork.employeeRepository.Add(employee);
				int count = await _unitOfWork.Complete();
				if (count > 0)

				{
					TempData["Message"] = "Employee Added";
				}
				else
				{
					TempData["Message"] = "Employee NOT Added";

				}
				return RedirectToAction(nameof(Index));
			}

			return View(model);
		}

        #endregion

        #region Details
        [HttpGet]
		public async Task<IActionResult> Details(int? id, string viewName = "Details")
		{
			if (id is null)
			{
				return BadRequest();
			}
			var employee = await _unitOfWork.employeeRepository.GetById(id.Value);

			if (employee is null)
				return NotFound();
			var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

			return View(viewName, employeeViewModel);
		} 
		#endregion

		#region Edit
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			return await Details(id, "Edit");
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model)
		{
			if (id != model.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				if (model.ImageName is not null)
				{
					DocumentSettings.DeleteFile(model.ImageName, "images");

				}
				model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
				var employee = _mapper.Map<Employee>(model);
				_unitOfWork.employeeRepository.Update(employee);
				int count = await _unitOfWork.Complete();
				if (count > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}

			return View();
		}

		#endregion

		#region Delete
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			return await Details(id, "Delete");

		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
		{
			if (id != model.Id)
				return BadRequest();


			var employee = _mapper.Map<Employee>(model);

			_unitOfWork.employeeRepository.Delete(employee);
			int count = await _unitOfWork.Complete();
			if (count > 0)
			{
				if (model.ImageName is not null)
				{
					DocumentSettings.DeleteFile(model.ImageName, "images");

				}
				return RedirectToAction(nameof(Index));
			}


			return View();
		}
	#endregion
	} 
}
