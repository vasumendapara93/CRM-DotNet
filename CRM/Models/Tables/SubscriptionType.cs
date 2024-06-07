using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Models.Tables
{
    public class SubscriptionType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public double Price { get; set; }

        public int FeatureId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(FeatureId))]
        public SubscriptionFeature Feature { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
