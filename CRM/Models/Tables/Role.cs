using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class Role
    {
        [Key]
        public  int Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
