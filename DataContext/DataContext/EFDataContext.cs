using DataContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataContext.DataContext
{
    public class EFDataContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=Deb; initial catalog=EFCrudDemo;persist security info=True;user id=sa;password=abc123;");
        }


        public Employee GetEmployeeById(int employeeId)
        {
            IQueryable<Employee> data = this.Employees.FromSql<Employee>(
                "Exec [dbo].uspGetEmployee " +
                    "@p_EmployeeId", new SqlParameter("p_EmployeeId", employeeId));

            if (data != null)
                return data.FirstOrDefault();
            else
                return new Employee();
        }
    }
}
