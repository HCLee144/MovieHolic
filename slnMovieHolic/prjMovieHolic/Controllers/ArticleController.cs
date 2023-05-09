using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class ArticleController : Controller
    {
        private readonly MovieContext _context;

        public ArticleController(MovieContext context)
        {
            _context = context;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            var movieContext = _context.TArticles.Include(t => t.FMember).Include(t => t.FMovie);
            return View(await movieContext.ToListAsync());
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles
                .Include(t => t.FMember)
                .Include(t => t.FMovie)
                .FirstOrDefaultAsync(m => m.FArticleId == id);
            if (tArticle == null)
            {
                return NotFound();
            }

            return View(tArticle);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId");
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FArticleId,FMemberId,FMovieId,FTitle,FTimeCreated,FTimeEdited,FBlockJson,FIsPublic")] TArticle tArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tArticle.FMovieId);
            return View(tArticle);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles.FindAsync(id);
            if (tArticle == null)
            {
                return NotFound();
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tArticle.FMovieId);
            return View(tArticle);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FArticleId,FMemberId,FMovieId,FTitle,FTimeCreated,FTimeEdited,FBlockJson,FIsPublic")] TArticle tArticle)
        {
            if (id != tArticle.FArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TArticleExists(tArticle.FArticleId))
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
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tArticle.FMovieId);
            return View(tArticle);
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles
                .Include(t => t.FMember)
                .Include(t => t.FMovie)
                .FirstOrDefaultAsync(m => m.FArticleId == id);
            if (tArticle == null)
            {
                return NotFound();
            }

            return View(tArticle);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TArticles == null)
            {
                return Problem("Entity set 'MovieContext.TArticles'  is null.");
            }
            var tArticle = await _context.TArticles.FindAsync(id);
            if (tArticle != null)
            {
                _context.TArticles.Remove(tArticle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TArticleExists(int id)
        {
          return (_context.TArticles?.Any(e => e.FArticleId == id)).GetValueOrDefault();
        }
    }
}
