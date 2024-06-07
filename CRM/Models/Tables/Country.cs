using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Tables
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
