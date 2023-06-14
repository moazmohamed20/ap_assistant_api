using APAssistantAPI.Data;
using APAssistantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APAssistantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PeopleController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // POST: api/People
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return person;
        }

        // PUT: api/People
        [HttpPut]
        public async Task<ActionResult<Person>> PutPerson(Person person)
        {
            if (!await _context.People.AnyAsync(p => p.Id == person.Id))
                return NotFound();

            _context.Entry(person).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return person;
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
                return NotFound();

            if (person.Face.Front != null && System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, person.Face.Front)))
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, person.Face.Front));

            if (person.Face.Left != null && System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, person.Face.Left)))
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, person.Face.Left));

            if (person.Face.Right != null && System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, person.Face.Right)))
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, person.Face.Right));

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/People/5/Photo
        [HttpPut("{id}/Photo")]
        public async Task<ActionResult<string>> PutPhoto(Guid id, IFormFile photo, FaceDirection faceDirection)
        {
            string imgName = id.ToString() + faceDirection.ToString() + Path.GetExtension(photo.FileName);
            string imgRelativePath = Path.Combine("images", "faces", imgName);
            string imgFullPath = Path.Combine(_webHostEnvironment.WebRootPath, imgRelativePath);

            using (FileStream imgStream = new(imgFullPath, FileMode.Create))
                await photo.CopyToAsync(imgStream);

            return imgRelativePath;
        }
    }
}
