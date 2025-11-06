using API_Edunas.Data;
using API_Edunas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Edunas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetAllQuiz()
        {
            var quizzes = await _context.Quizzes
                .Include(q => q.Subjects)
                .Include(q => q.Questions)
                .Include(q => q.quiz_Results)
                .ToListAsync();

            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuizById(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Subjects)
                .Include(q => q.Questions)
                .Include(q => q.quiz_Results)
                .FirstOrDefaultAsync(q => q.ID == id);

            if (quiz == null)
            {
                return NotFound(new { message = "Data quiz tidak ditemukan." });
            }

            return Ok(quiz);
        }

        [HttpPost]
        public async Task<ActionResult<Quiz>> CreateQuiz([FromBody] Quiz quiz)
        {
            // Validasi sederhana
            if (quiz == null)
            {
                return BadRequest(new { message = "Data quiz tidak valid." });
            }

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuizById), new { id = quiz.ID }, quiz);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] Quiz quiz)
        {
            if (id != quiz.ID)
            {
                return BadRequest(new { message = "ID tidak cocok." });
            }

            var existingQuiz = await _context.Quizzes.FindAsync(id);
            if (existingQuiz == null)
            {
                return NotFound(new { message = "Data quiz tidak ditemukan." });
            }

            existingQuiz.Subject_id = quiz.Subject_id;
            existingQuiz.Title = quiz.Title;
            existingQuiz.Total_questions = quiz.Total_questions;

            _context.Entry(existingQuiz).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data quiz berhasil diperbarui." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound(new { message = "Data quiz tidak ditemukan." });
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data quiz berhasil dihapus." });
        }
    }
}
