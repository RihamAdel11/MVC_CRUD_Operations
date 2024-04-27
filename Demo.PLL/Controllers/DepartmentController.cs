using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PLL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
	{

        private readonly IUnitWork _unitwork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitWork unitwork,IMapper mapper)
        {
            _unitwork = unitwork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
		{
			var departments=await _unitwork.departmentRepositry.GetAllAsync();
			var mappedDepartment=_mapper.Map <IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
			return View(mappedDepartment);	
		}
		public IActionResult Create()
		{
			
			return View();
		}
		[HttpPost ]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
		{
			if (ModelState.IsValid)
			{
				var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
				await _unitwork.departmentRepositry.AddAsync(mappedDepartment);
                await _unitwork.Complete();
                    return RedirectToAction(nameof(Index));
			}

            return View(departmentVM);
        }
        public async Task< IActionResult> Details(int? id,string viewname="Details")
        {
			if (id is null)
				return BadRequest();
		    var department=await _unitwork.departmentRepositry .GetByIdAsync (id.Value );
			if (department is null)
				return NotFound();
            var mappedDepartment = _mapper.Map< Department, DepartmentViewModel>(department);
            return View(viewname, mappedDepartment);
            
        }
		[HttpGet ]
        public async Task<IActionResult> Edit(int? id)
        {

			//if (id is null)
			//    return BadRequest();
			//var department = _unitwork.departmentRepositry.GetById(id.Value);
			//if (department is null)
			//    return NotFound();
			//return View(department);
			return await  Details(id,"Edit");
        }
		[HttpPost ]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVM, [FromRoute ] int id)
        {   if(id!=departmentVM.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
                    var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitwork.departmentRepositry.Update(mappedDepartment);
                   await _unitwork.Complete();
                    return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);

			
				}	
			}
            return View(departmentVM);
        }
		[HttpGet]
		public async Task< IActionResult> Delete(int ?id)
        {
			return await Details(id, "Delete");
        }
		[HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id !=departmentVM.Id )
                return BadRequest();
			try
			{
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitwork.departmentRepositry.Delete(mappedDepartment);
               await  _unitwork.Complete();
                return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{

				ModelState.AddModelError(string.Empty, ex.Message);
				return View(departmentVM);
			}
        }


    }
}
