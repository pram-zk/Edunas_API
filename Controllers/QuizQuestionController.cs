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
    public class QuizQuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizQuestionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz_question>>> GetAllQuestions()
        {
            var questions = await _context.Quiz_questions
                .Include(q => q.Quiz)
                .ToListAsync();

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz_question>> GetQuestionById(int id)
        {
            var question = await _context.Quiz_questions
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.ID == id);

            if (question == null)
            {
                return NotFound(new { message = "Pertanyaan tidak ditemukan." });
            }

            return Ok(question);
        }

  
        [HttpPost]
        public async Task<ActionResult<Quiz_question>> CreateQuestion([FromBody] Quiz_question question)
        {
            if (question == null)
            {
                return BadRequest(new { message = "Data pertanyaan tidak valid." });
            }

            _context.Quiz_questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionById), new { id = question.ID }, question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] Quiz_question question)
        {
            if (id != question.ID)
            {
                return BadRequest(new { message = "ID tidak cocok." });
            }

            var existingQuestion = await _context.Quiz_questions.FindAsync(id);
            if (existingQuestion == null)
            {
                return NotFound(new { message = "Pertanyaan tidak ditemukan." });
            }

            existingQuestion.quiz_id = question.quiz_id;
            existingQuestion.question_text = question.question_text;
            existingQuestion.option_a = question.option_a;
            existingQuestion.option_b = question.option_b;
            existingQuestion.option_c = question.option_c;
            existingQuestion.option_d = question.option_d;
            existingQuestion.correct_option = question.correct_option;

            _context.Entry(existingQuestion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pertanyaan berhasil diperbarui." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Quiz_questions.FindAsync(id);
            if (question == null)
            {
                return NotFound(new { message = "Pertanyaan tidak ditemukan." });
            }

            _context.Quiz_questions.Remove(question);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pertanyaan berhasil dihapus." });
        }
    }
}
