using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class DiscountSchedule
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int SubscriptionId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(SubscriptionId))]
        public SubscriptionType Subscription { get; set; }

        public double Discount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
