using System.ComponentModel.DataAnnotations;

namespace APAssistantAPI.Models
{
    public class Medicine
    {
        public Guid Id { get; set; }

        [Required]
        public Guid? PatientId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }
    }
}
