using DataContext.DataContext;
using DataContext.Models;
using DataContext.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EF_Crud_Samples.Controllers
{
    public class EmployeesController : Controller
    {
        EFDataContext _dbContext = new EFDataContext();

        public IActionResult Index(string sortField, string currentSortField, string currentSortOrder, string currentFilter, string SearchString, int? pageNo)
        {
            var employees = this.GetEmployeeList();
            if (SearchString != null)
            {
                pageNo = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewData["CurrentSort"] = sortField;
            ViewBag.CurrentFilter = SearchString;
            if (!String.IsNullOrEmpty(SearchString))
            {
                employees = employees.Where(s => s.EmployeeName.Contains(SearchString)).ToList();
            }
            employees = this.SortEmployeeData(employees, sortField, currentSortField, currentSortOrder);
            int pageSize = 10;
            return View(PagingList<Employee>.CreateAsync(employees.AsQueryable<Employee>(), pageNo ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            this.GetModelData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Employees.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            this.GetModelData();
            return View();
        }

        public IActionResult Edit(int id)
        {
            //Employee data = this._dbContext.Employees.Where(p => p.Id == id).FirstOrDefault();
            Employee data = this._dbContext.GetEmployeeById(id);
            this.GetModelData();
            return View("Create", data);
        }

        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Employees.Update(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            this.GetModelData();
            return View("Create", model);
        }