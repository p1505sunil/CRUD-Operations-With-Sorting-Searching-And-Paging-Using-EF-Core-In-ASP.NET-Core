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

        public IActionResult Delete(int id)
        {
            Employee data = this._dbContext.Employees.Where(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                _dbContext.Employees.Remove(data);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        #region Private method
        private void GetModelData()
        {
            ViewBag.Departments = this._dbContext.Departments.ToList();
            ViewBag.Designations = this._dbContext.Designations.ToList();
        }

        private List<Employee> GetEmployeeList()
        {
            return (from employee in this._dbContext.Employees
                    join desig in this._dbContext.Designations on employee.DepartmentId equals desig.DesignationId
                    join dept in this._dbContext.Departments on employee.DepartmentId equals dept.DepartmentId
                    select new Employee
                    {
                        Id = employee.Id,
                        EmployeeCode = employee.EmployeeCode,
                        EmployeeName = employee.EmployeeName,
                        DateOfBirth = employee.DateOfBirth,
                        JoinDate = employee.JoinDate,
                        Salary = employee.Salary,
                        Address = employee.Address,
                        State = employee.State,
                        City = employee.City,
                        ZipCode = employee.ZipCode,
                        DepartmentId = employee.DepartmentId,
                        DepartmentName = dept.DepartmentName,
                        DesignationId = employee.DesignationId,
                        DesignationName = desig.DesignationName
                    }).AsNoTracking().ToList();
        }

        private List<Employee> SortEmployeeData(List<Employee> employees, string sortField, string currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "EmployeeCode";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if (currentSortField == sortField)
                {
                    ViewBag.SortOrder = currentSortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }

            var propertyInfo = typeof(Employee).GetProperty(ViewBag.SortField);
            if (ViewBag.SortOrder == "Asc")
            {
                employees = employees.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
            }
            else
            {
                employees = employees.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
            }
            //int pageSize = 10;
            //int pageNumber = (pageNo ?? 1);
            //return employees.ToPagedList(pageNumber, pageSize).ToList();
            return employees;

        }
        #endregion
    }
}