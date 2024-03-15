using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models.DTOs
{
    public class BranchCreateDTO
    {
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string OrganizationId { get; set; }

        public DateTime? UpdateDate;
    }
}
