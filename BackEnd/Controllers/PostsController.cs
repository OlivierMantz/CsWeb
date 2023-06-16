using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Models.DTOs;
using BackEnd.Services;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostContext _context;
        private readonly UserService _userService;

        public PostsController(PostContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
    }
    private static bool CheckInputInvalid(PostDTO postDTO) => postDTO == null || string.IsNullOrWhiteSpace(postDTO.Title) ||
            string.IsNullOrWhiteSpace(postDTO.Description) ||
            postDTO.AuthorId == null ||
            string.IsNullOrWhiteSpace(postDTO.IsAvailable);

        private static PostDTO PostToDTO(Post post) =>
           new()
           {
               Id = post.Id,
               Title = post.Title,
               Description = post.Description,
               AuthorId = post.AuthorId,
               IsAvailable = post.IsAvailable,
           };
        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts()
        {
            return await _context.Post
                .Select(x => PostToDTO(x))
                .ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(long id)
        {
            if (_context.Post == null)
            {
                return NotFound();
            }
            var post = await _context.Post.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return PostToDTO(post);
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(long id, PostDTO postDTO)
        {
            if (id != postDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(postDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO postDTO)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'PostContext.Posts'  is null.");
            }
            if (CheckInputInvalid(postDTO))
            {
                return Problem("One or more invalid inputs");
            }

            if (!await _userService.UserExistsAsync(postDTO.AuthorId))
            {
                return Problem("User does not exist");
            }

            var post = new Post
            {
                Title = postDTO.Title,
                Description = postDTO.Description,
                AuthorId = postDTO.AuthorId,
                IsAvailable = postDTO.IsAvailable,
            };
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, PostToDTO(post));
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(long id)
        {
            if (_context.Post == null)
            {
                return NotFound();
            }
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(long id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
