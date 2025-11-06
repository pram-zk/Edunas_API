using API_Edunas.Data;
using API_Edunas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace API_Edunas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubjectController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subjects>>> GetSubject()
        {
            return await _context.Subjects.ToListAsync();
        }

        [HttpGet("Mapel")]
        public async Task<ActionResult<Subjects>> GetSubject(int id)
        {
            var mapel = await _context.Subjects.FindAsync(id);

            if (mapel == null)
            {
                return BadRequest();
            } else
            {
                return mapel;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, Subjects subject)
        {
            if (id != subject.ID)
            {
                return BadRequest(new { message = "ID tidak cocok." });
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Data berhasil diperbarui." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Subjects.Any(e => e.ID == id))
                {
                    return NotFound(new { message = "Data tidak ditemukan." });
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound(new { message = "Data tidak ditemukan." });
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil dihapus." });
        }
    }
}
