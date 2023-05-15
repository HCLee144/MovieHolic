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
    public class movieBackController : Controller
    {
        private readonly MovieContext _context;

        public movieBackController(MovieContext context)
        {
            _context = context;
        }

        // GET: movieBack
        public async Task<IActionResult> Index()
        {
            var movieContext = _context.TMovies.Include(t => t.FRating).Include(t => t.FSeries);
            return View(await movieContext.ToListAsync());
        }

        // GET: movieBack/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TMovies == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tMovie == null)
            {
                return NotFound();
            }

            return View(tMovie);
        }

        // GET: movieBack/Create
        public IActionResult Create()
        {
            ViewData["FRatingId"] = new SelectList(_context.TRatings, "FId", "FId");
            ViewData["FSeriesId"] = new SelectList(_context.TSeries, "FId", "FId");
            return View();
        }

        // POST: movieBack/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FId,FSeriesId,FRatingId,FNameCht,FNameEng,FScheduleStart,FScheduleEnd,FShowLength,FInteroduce,FTrailerLink,FPosterPath,FImagePath,FPrice")] TMovie tMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FRatingId"] = new SelectList(_context.TRatings, "FId", "FId", tMovie.FRatingId);
            ViewData["FSeriesId"] = new SelectList(_context.TSeries, "FId", "FId", tMovie.FSeriesId);
            return View(tMovie);
        }

        // GET: movieBack/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TMovies == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies.FindAsync(id);
            if (tMovie == null)
            {
                return NotFound();
            }
            ViewData["FRatingId"] = new SelectList(_context.TRatings, "FId", "FId", tMovie.FRatingId);
            ViewData["FSeriesId"] = new SelectList(_context.TSeries, "FId", "FId", tMovie.FSeriesId);
            return View(tMovie);
        }

        // POST: movieBack/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FId,FSeriesId,FRatingId,FNameCht,FNameEng,FScheduleStart,FScheduleEnd,FShowLength,FInteroduce,FTrailerLink,FPosterPath,FImagePath,FPrice")] TMovie tMovie)
        {
            if (id != tMovie.FId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TMovieExists(tMovie.FId))
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
            ViewData["FRatingId"] = new SelectList(_context.TRatings, "FId", "FId", tMovie.FRatingId);
            ViewData["FSeriesId"] = new SelectList(_context.TSeries, "FId", "FId", tMovie.FSeriesId);
            return View(tMovie);
        }

        // GET: movieBack/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TMovies == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tMovie == null)
            {
                return NotFound();
            }

            return View(tMovie);
        }

        // POST: movieBack/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TMovies == null)
            {
                return Problem("Entity set 'MovieContext.TMovies'  is null.");
            }
            var tMovie = await _context.TMovies.FindAsync(id);
            if (tMovie != null)
            {
                _context.TMovies.Remove(tMovie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TMovieExists(int id)
        {
          return (_context.TMovies?.Any(e => e.FId == id)).GetValueOrDefault();
        }
    }
}
