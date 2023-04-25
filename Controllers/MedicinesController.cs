using APAssistantAPI.Data;
using APAssistantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APAssistantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MedicinesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // POST: api/Medicines
        [HttpPost]
        public async Task<ActionResult<Medicine>> PostMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return medicine;
        }

        // PUT: api/Medicines/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Medicine>> PutMedicine(Guid id, Medicine newMedicine)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
                return NotFound();

            medicine = newMedicine;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return newMedicine;
        }

        // DELETE: api/Medicines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(Guid id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
                return NotFound();

            if (medicine.ImageUrl != null && System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, medicine.ImageUrl)))
                System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, medicine.ImageUrl));

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Medicines/5/Image
        [HttpPut("{id}/Image")]
        public async Task<ActionResult<string>> PutImage(Guid id, IFormFile image)
        {
            string imgName = id + Path.GetExtension(image.FileName);
            string imgRelativePath = Path.Combine("images", "medicines", imgName);
            string imgFullPath = Path.Combine(_webHostEnvironment.WebRootPath, imgRelativePath);

            using (FileStream imgStream = new(imgFullPath, FileMode.Create))
                await image.CopyToAsync(imgStream);

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { throw; }

            return imgRelativePath;
        }
    }
}
