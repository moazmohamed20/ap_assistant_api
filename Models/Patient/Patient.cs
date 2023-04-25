using Newtonsoft.Json;

namespace APAssistantAPI.Models.Patient
{
    public class Patient
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = new byte[32];

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = new byte[32];

        public ICollection<Medicine> Medicines { get; set; }
    }
}
