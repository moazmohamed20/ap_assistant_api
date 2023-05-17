using System.ComponentModel.DataAnnotations;

namespace APAssistantAPI.Models.Patient
{
    public class PatientCreateOrUpdateRequest
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }
    }
}
