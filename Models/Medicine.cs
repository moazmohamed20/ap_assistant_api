using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace APAssistantAPI.Models
{
    public class Medicine
    {
        public Guid Id { get; set; }

        [ValidateNever]
        public Guid PatientId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }
    }
}
