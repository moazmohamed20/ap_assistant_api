using System.ComponentModel.DataAnnotations;

namespace APAssistantAPI.Models.Patient
{
    public class PatientLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }
    }
}
