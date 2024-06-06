using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models
{
    [PrimaryKey(nameof(Id))]
    public class Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id = Guid.NewGuid().ToString();

        [Required]
        public string BranchName { get; set; }

        public string? BranchCode {get; set;}

        [Required]
        public string OrganizationId { get; set;}


        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
