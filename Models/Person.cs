using System.ComponentModel.DataAnnotations;

namespace APAssistantAPI.Models
{
    public class Person
    {
        public Guid Id { get; set; }

        [Required]
        public Guid? PatientId { get; set; }

        public string Name { get; set; }

        public string Relation { get; set; }

        public Face Face { get; set; }
    }

    public class Face
    {

        public string Front { get; set; }

        public string? Left { get; set; }

        public string? Right { get; set; }
    }

    public enum FaceDirection { Front, Left, Right }
}
