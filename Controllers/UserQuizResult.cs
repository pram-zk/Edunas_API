using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Edunas.Data;
using API_Edunas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Edunas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserQuizResultController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserQuizResultController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User_quiz_result>>> GetAllResults()
        {
            var results = await _context.user_quiz_results
                .Include(r => r.Quiz)
                .ToListAsync();

            return Ok(results);
        }

  
        [HttpGet("{id}")]
        public async Task<ActionResult<User_quiz_result>> GetResultById(int id)
        {
            var result = await _context.user_quiz_results
                .Include(r => r.Quiz)
                .FirstOrDefaultAsync(r => r.id == id);

            if (result == null)
            {
                return NotFound(new { message = "Hasil kuis tidak ditemukan." });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User_quiz_result>> CreateResult([FromBody] User_quiz_result result)
        {
            if (result == null)
            {
                return BadRequest(new { message = "Data hasil kuis tidak valid." });
            }

            _context.user_quiz_results.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResultById), new { id = result.id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(int id, [FromBody] User_quiz_result result)
        {
            if (id != result.id)
            {
                return BadRequest(new { message = "ID tidak cocok." });
            }

            var existingResult = await _context.user_quiz_results.FindAsync(id);
            if (existingResult == null)
            {
                return NotFound(new { message = "Data hasil kuis tidak ditemukan." });
            }

            existingResult.user_id = result.user_id;
            existingResult.quiz_id = result.quiz_id;
            existingResult.score = result.score;
            existingResult.total_correct = result.total_correct;
            existingResult.total_wrong = result.total_wrong;
            existingResult.submitted_at = result.submitted_at;

            _context.Entry(existingResult).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data hasil kuis berhasil diperbarui." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.user_quiz_results.FindAsync(id);
            if (result == null)
            {
                return NotFound(new { message = "Data hasil kuis tidak ditemukan." });
            }

            _context.user_quiz_results.Remove(result);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data hasil kuis berhasil dihapus." });
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<User_quiz_result>>> GetResultsByUser(int userId)
        {
            var results = await _context.user_quiz_results  
                .Include(r => r.Quiz)
                .Where(r => r.user_id == userId)
                .ToListAsync();

            if (results == null || results.Count == 0)
            {
                return NotFound(new { message = "Belum ada hasil kuis untuk user ini." });
            }

            return Ok(results);
        }
    }
}
