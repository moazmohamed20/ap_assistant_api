using APAssistantAPI.Models;
using APAssistantAPI.Models.Patient;
using Microsoft.EntityFrameworkCore;
namespace APAssistantAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Location> Locations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
    }
}
