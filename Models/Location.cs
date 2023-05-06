using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace APAssistantAPI.Models
{
    public class Location
    {
        public Guid Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [ValidateNever]
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}
