using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataContext.Models
{
    [Table("Designation", Schema = "dbo")]
    public class Designation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(20)")]
        [Display(Name = "Designation Code")]
        public string DesignationCode { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }        
    }
}
