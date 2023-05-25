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
    public class ShortCmtsController : Controller
    {
        private readonly MovieContext _context;

        public ShortCmtsController(MovieContext context)
        {
            _context = context;
        }

        // GET: ShortCmts
        public async Task<IActionResult> Index()
        {
            var movieContext = _context.TShortCmts.Include(t => t.FMember).Include(t => t.FMovie);
            return View(await movieContext.ToListAsync());
        }
        [HttpGet("shortcmts/movie/{movieID}")]
        public async Task<IActionResult> Movie(int movieID)
        {
            var movieContext = _context.TShortCmts.Include(t => t.FMember).Include(t => t.FMovie).Where(t=>t.FMovieId==movieID);
            return View(await movieContext.ToListAsync());
        }

        // GET: ShortCmts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TShortCmts == null)
            {
                return NotFound();
            }

            var tShortCmt = await _context.TShortCmts
                .Include(t => t.FMember)
                .Include(t => t.FMovie)
                .FirstOrDefaultAsync(m => m.FCmtid == id);
            if (tShortCmt == null)
            {
                return NotFound();
            }

            return View(tShortCmt);
        }

        // GET: ShortCmts/Create
        public IActionResult Create()
        {
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId");
            return View();
        }

        // POST: ShortCmts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FCmtid,FMovieId,FMemberId,FTitle,FRate,FCreatedTime,FEditedTime,FVisible")] TShortCmt tShortCmt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tShortCmt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tShortCmt.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tShortCmt.FMovieId);
            return View(tShortCmt);
        }

        // GET: ShortCmts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TShortCmts == null)
            {
                return NotFound();
            }

            var tShortCmt = await _context.TShortCmts.FindAsync(id);
            if (tShortCmt == null)
            {
                return NotFound();
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tShortCmt.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tShortCmt.FMovieId);
            return View(tShortCmt);
        }

        // POST: ShortCmts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FCmtid,FMovieId,FMemberId,FTitle,FRate,FCreatedTime,FEditedTime,FVisible")] TShortCmt tShortCmt)
        {
            if (id != tShortCmt.FCmtid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tShortCmt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TShortCmtExists(tShortCmt.FCmtid))
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
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tShortCmt.FMemberId);
            ViewData["FMovieId"] = new SelectList(_context.TMovies, "FId", "FId", tShortCmt.FMovieId);
            return View(tShortCmt);
        }

        // GET: ShortCmts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TShortCmts == null)
            {
                return NotFound();
            }

            var tShortCmt = await _context.TShortCmts
                .Include(t => t.FMember)
                .Include(t => t.FMovie)
                .FirstOrDefaultAsync(m => m.FCmtid == id);
            if (tShortCmt == null)
            {
                return NotFound();
            }

            return View(tShortCmt);
        }

        // POST: ShortCmts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TShortCmts == null)
            {
                return Problem("Entity set 'MovieContext.TShortCmts'  is null.");
            }
            var tShortCmt = await _context.TShortCmts.FindAsync(id);
            if (tShortCmt != null)
            {
                _context.TShortCmts.Remove(tShortCmt);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TShortCmtExists(int id)
        {
          return (_context.TShortCmts?.Any(e => e.FCmtid == id)).GetValueOrDefault();
        }
    }
}
