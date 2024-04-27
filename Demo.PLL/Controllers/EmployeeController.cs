using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositry;
using Demo.DAL.Models;
using Demo.PLL.Helpers;
using Demo.PLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PLL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        
        private readonly IUnitWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitWork unitOfWork, IMapper mapper)
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task< IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees =await _unitOfWork.employeeRepositry.GetAllAsync();
            }
            else
            {
                employees = _unitOfWork.employeeRepositry.GetEmployeeByName(SearchValue);
            }
            var mapperEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mapperEmployee);

        }
        public IActionResult Create()
        {
            //@ViewBag.Departments=_departmentRepositry.GetAll();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
              employeeVM .ImageName=  DocumentSetting.UploadFile(employeeVM.Image, "Images");
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
               await _unitOfWork.employeeRepositry.AddAsync(mappedEmp);
               await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            return View(employeeVM);
        }
        public async Task< IActionResult> Details(int? id, string viewname = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee =await _unitOfWork.employeeRepositry.GetByIdAsync(id.Value);

            if (employee is null)
                return NotFound();
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewname, mappedEmployee);

        }
        [HttpGet]
        public async Task< IActionResult> Edit(int? id)
        {

            //if (id is null)
            //    return BadRequest();
            //var department = _departmentRepositry.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.employeeRepositry.Update(mappedEmployee);
                   await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);


                }
            }
            return View(employeeVM);

        }
        [HttpGet]
        public async Task< IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task< IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.employeeRepositry.Delete(MappedEmployee);
                var result=await _unitOfWork.Complete();
                if (result > 0&& employeeVM .ImageName is not null)
                {
                    DocumentSetting.DeleteFile(employeeVM.ImageName, "Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
    }
}
