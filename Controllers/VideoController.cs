using API_Edunas.Data;
using API_Edunas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace API_Edunas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoController : ControllerBase
    {
        private readonly AppDbContext _context;
       
        public VideoController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> getVideo()
        {
            return await _context.Video.Include(f => f.Subjects).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> getVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);

            return video == null ? BadRequest() : video;
        }

        [HttpPost]
        public async Task<IActionResult> postVideo([FromBody] Video dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var video = new Video
            {
                Subject_id = dto.Subject_id,
                Title = dto.Title,
                Description = dto.Description,
                Video_url = dto.Video_url
            };

            _context.Video.Add(video);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Video berhasil ditambahkan",
                data = video
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Video>> putVideo(int id, Video video)
        {
            if(id != video.ID)
            {
                return BadRequest();
            } else
            {
                _context.Entry(video).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Video>> deleteVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);

            if (video == null)
                return BadRequest();

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
