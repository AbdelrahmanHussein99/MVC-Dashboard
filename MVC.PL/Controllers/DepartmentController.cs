using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using MVC.PL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.departmentRepository.GetAll();

            var res = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);
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
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);

                _unitOfWork.departmentRepository.Add(department);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
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
            var department = await _unitOfWork.departmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            var departmentViewModel = _mapper.Map<DepartmentViewModel>(department);
            return View(viewName, departmentViewModel);
        }

        #endregion



        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);

                _unitOfWork.departmentRepository.Update(department);
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
        public async Task<IActionResult> Delete(DepartmentViewModel model)
        {
            var department = _mapper.Map<Department>(model);


            _unitOfWork.departmentRepository.Delete(department);
            int count = await _unitOfWork.Complete();
            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }


            return View();
        } 
        #endregion
    }
}
