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
    public class ArticlesController : ControllerBase
    {
        private readonly MyDBContext _context;

        #region Auto_Gen
        public ArticlesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Anthor)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        // POST: api/Articles
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
        #endregion

        [HttpGet("Page/{page}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetPage(int page)
        {
            return await _context.Articles
                .OrderByDescending(a => a.Time)
                .Skip((page - 1) * AppConst.BlogsPerPage)
                .Take(AppConst.BlogsPerPage)
                .ToListAsync();
        }

        [HttpGet("User/{id}/Page/{page}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetPage(int id, int page)
        {
            return await _context.Articles
                .Where(a => a.UserId == id)
                .OrderByDescending(a => a.Time)
                .Skip((page - 1) * AppConst.BlogsPerPage)
                .Take(AppConst.BlogsPerPage)
                .ToListAsync();
        }

        [HttpGet("TotalAmount")]
        public async Task<ActionResult<int>> GetTotalAmount()
        {
            return await _context.Articles.CountAsync();
        }

        [HttpGet("TotalAmount/{id}")]
        public async Task<ActionResult<int>> GetTotalAmountByUser(int id)
        {
            return await _context.Articles.Where(a => a.UserId == id).CountAsync();
        }
    }
}
