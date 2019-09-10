using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataContext.Models
{
    [Table("Employee", Schema = "dbo")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Employee Id")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(20)")]
        [Display(Name = "Code")]
        public string EmployeeCode { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(100)")]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [Required]
        [Column(TypeName = "Decimal(18,2)")]
        public decimal Salary { get; set; }
        
        [Column(TypeName = "NVarchar(100)")]
        public string Address { get; set; }

        [Column(TypeName = "NVarchar(100)")]
        public string State { get; set; }

        [Column(TypeName = "NVarchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "NVarchar(20)")]
        public string ZipCode { get; set; }

        [ForeignKey("DepartmentInfo")]
        [Required]
        public int DepartmentId { get; set; }

        [Display(Name = "Department")]
        [NotMapped]
        public string DepartmentName { get; set; }

        public virtual Department DepartmentInfo { get; set; }

        [ForeignKey("DesignationInfo")]
        [Required]
        public int DesignationId { get; set; }

        [Display(Name = "Designation")]
        [NotMapped]
        public string DesignationName { get; set; }

        public virtual Designation DesignationInfo { get; set; }
    }
}
