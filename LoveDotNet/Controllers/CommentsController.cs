using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoveDotNet.Models;
using LoveDotNet.Helpers;

namespace LoveDotNet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly MyDBContext _context;

        #region Auto_Gen
        public CommentsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var Comment = await _context.Comments.FindAsync(id);

            if (Comment == null)
            {
                return NotFound();
            }

            return Comment;
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment Comment)
        {
            if (id != Comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(Comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<bool>> PostComment(Comment Comment)
        {
            _context.Comments.Add(Comment);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var Comment = await _context.Comments.FindAsync(id);
            if (Comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(Comment);
            await _context.SaveChangesAsync();

            return Comment;
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
        #endregion

        [HttpGet("InArticle/{id}/{page}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsPageInArticle(int id,int page)
        {
            return await _context.Comments
                .Include(a => a.User)
                .Where(a => a.ArticleId == id)
                .OrderByDescending(a=>a.Time)
                .Skip((page - 1) * AppConst.CommentsPerPage)
                .Take(AppConst.CommentsPerPage)
                .ToListAsync();
        }

        [HttpGet("TotalAmount/{id}")]
        public async Task<ActionResult<int>> GetTotalAmount(int id)
        {
            return await _context.Comments.Where(a=>a.ArticleId == id).CountAsync();
        }

    }
}
