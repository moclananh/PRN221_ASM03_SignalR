using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment03.Models;
using PRN221_Assignment03.SignalR;

namespace PRN221_Assignment03.Controllers
{
    public class PostsController : Controller
    {
        private readonly Prn221Asm03Context _context;
        private readonly IHubContext<SignalrServer> _postHub;
        public PostsController(Prn221Asm03Context context, IHubContext<SignalrServer> postHub)
        {
            _context = context;
            _postHub = postHub;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var prn221Asm03Context = _context.Posts.Include(p => p.Author).Include(p => p.Category);
            return View(await prn221Asm03Context.ToListAsync());
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            var response = _context.Posts.ToList();
            return Ok(response);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.AppUsers, "UserId", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,AuthorId,CreatedDate,UpdatedDate,Title,Content,PublishStatus,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                DateTime currentDate = DateTime.Now;
                post.CreatedDate = currentDate;
                post.UpdatedDate = currentDate;
                post.AuthorId = HttpContext.Session.GetInt32("UserId");
                _context.Add(post);
                await _context.SaveChangesAsync();
                await _postHub.Clients.All.SendAsync("LoadPosts");
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.AppUsers, "UserId", "FullName", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.AppUsers, "UserId", "FullName", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,AuthorId,CreatedDate,UpdatedDate,Title,Content,PublishStatus,CategoryId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    await _postHub.Clients.All.SendAsync("LoadPosts");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.AppUsers, "UserId", "FullName", post.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.PostCategories, "CategoryId", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'Prn221Asm03Context.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            await _postHub.Clients.All.SendAsync("LoadPosts");
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
