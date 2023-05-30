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
    public class ArticlesController : Controller
    {
        private readonly MovieContext _context;

        public ArticlesController(MovieContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            sessionCheck();
            var movieContext = _context.TArticles.Include(t => t.FMember).Include(t => t.FMovie);
            return View(await movieContext.ToListAsync());
        }

        // GET: Articles/Details/5
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
            sessionCheck();
            return View(tArticle);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            if (userId == null)
            {
                return RedirectToAction("memberLogin", "MemberFront", null);
            }
            //ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            sessionCheck();
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FNameCht");
            ViewBag.MemberID = (int)userId;
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FArticleId,FMemberId,FMovieId,FScore,FTitle,FTimeCreated,FTimeEdited,FBlockJson,FIsPublic")] TArticle tArticle)
        {
            sessionCheck();
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            if (userId == null || userId != tArticle.FMemberId)
            {
                return RedirectToAction("memberLogin", "MemberFront", null);
            }

            tArticle.FMemberId = (int)userId;
            tArticle.FTimeCreated = DateTime.Now;
            _context.Add(tArticle);
            await _context.SaveChangesAsync();
            var myLatest = getMyLatestArtID();
            if (myLatest != null)
                return RedirectToAction("Details", "Articles", new { id = myLatest });
            
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FNameCht");
            ViewBag.FMemberId = (int)userId;
            ViewBag.Readonly = true;
            return View(tArticle);
        }

        private int? getMyLatestArtID()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            if (userId == null)
                return null;
            var q = _context.TArticles.Where(t => t.FMemberId == userId).OrderBy(t=>t.FArticleId).LastOrDefault();
            if (q == null)
                return null;
            return q.FArticleId;
        }

        // GET: Articles/Edit/5
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
            sessionCheck();
            return View(tArticle);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FArticleId,FMemberId,FMovieId,FScore,FTitle,FTimeCreated,FTimeEdited,FBlockJson,FIsPublic")] TArticle tArticle)
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
            sessionCheck();
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tArticle.FMovieId);
            return View(tArticle);
        }

        // GET: Articles/Delete/5
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
            sessionCheck();
            return View(tArticle);
        }

        // POST: Articles/Delete/5
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
            sessionCheck();
            return RedirectToAction(nameof(Index));
        }

        private bool TArticleExists(int id)
        {
            return (_context.TArticles?.Any(e => e.FArticleId == id)).GetValueOrDefault();
        }
        private bool sessionCheck()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var userName = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER_NAME);
            bool isUserLoggedIn = userId != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
            ViewBag.userName = userName;
            return isUserLoggedIn;
        }
    }
}
