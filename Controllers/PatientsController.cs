using APAssistantAPI.Data;
using APAssistantAPI.Models.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Cryptography;

namespace APAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            return await _context.Patients.AsNoTracking().ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            await _context.Entry(patient)
                .Collection(p => p.Medicines)
                .LoadAsync();

            return patient;
        }

        // POST: api/Patients/Register
        [HttpPost("[action]")]
        public async Task<ActionResult<Patient>> Register(PatientRegisterRequest request)
        {
            if (_context.Patients.Any(p => p.Email == request.Email))
                return Conflict("Email address already used.");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var patient = new Patient
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        // POST: api/Patients/Login
        [HttpPost("[action]")]
        public async Task<ActionResult<Patient>> Login(PatientLoginRequest request)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == request.Email);

            if (patient == null)
                return BadRequest("Incorrect email address or password.");

            if (!VerifyPasswordHash(request.Password, patient.PasswordHash, patient.PasswordSalt))
                return BadRequest("Incorrect email address or password.");

            await _context.Entry(patient)
                .Collection(p => p.Medicines)
                .LoadAsync();

            return patient;
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Patient>> PutPatient(Guid id, PatientUpdateRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            patient.Name = request.Name;
            patient.Email = request.Email;
            patient.PasswordHash = passwordHash;
            patient.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return patient;
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
