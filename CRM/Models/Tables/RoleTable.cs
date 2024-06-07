using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Tables
{
    public class RoleTable
    {
        [Key]
        public int Id { get; set; }

        public string Rolename { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
