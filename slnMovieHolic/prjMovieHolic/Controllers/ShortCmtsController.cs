using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prjMovieHolic.Models;
using Xunit.Sdk;

namespace prjMovieHolic.Controllers
{
    public class ShortCmtsController : SuperFrontController
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
        [HttpGet("shortcmts/edit-or-create/{movieID}")]
        public IActionResult editOrCreate(int movieID)
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            if (sessionCheck())
            {
                //查詢是否有有既存的 ? 
                var q = _context.TShortCmts.Where(t => t.FMovieId == movieID).Where(t => t.FMemberId == userId);
                if (q.Count() > 0)
                {
                    int cmtID = q.FirstOrDefault().FCmtid;
                    return RedirectToAction("edit", "ShortCmts", new { id = cmtID });
                }
                else
                    return RedirectToAction("Create", "ShortCmts", new { movieID });
            }
            else
            {
                string controller = "moviefront";
                string view = "movieDetails";
                int parameter = movieID;
                HttpContext.Session.SetString(CDictionary.SK_CONTROLLER, controller);
                HttpContext.Session.SetString(CDictionary.SK_VIEW, view);
                HttpContext.Session.SetInt32(CDictionary.SK_PARAMETER, parameter);
                return RedirectToAction("memberLogin", "MemberFront", null);
            }
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
        [HttpGet("ShortCmts/Create/{movieID}")]
        public IActionResult Create(int movieID)
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            if (userId == null)
                return RedirectToAction("memberLogin", "MemberFront", null);
            sessionCheck();
            ViewData["FMemberId"] = userId;
            ViewData["FMovieId"] = movieID;
            ViewData["FMemberName"] = _context.TMembers.Where(t => t.FMemberId == userId).Select(t => t.FName).FirstOrDefault();
            ViewData["FMovieName"] = _context.TMovies.Where(t => t.FId == movieID).Select(t => t.FNameCht).FirstOrDefault();
            return View();
        }

        // POST: ShortCmts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FMovieId,FMemberId,FTitle,FRate")] TShortCmt tShortCmt)
        {

            if (tShortCmt != null)
            {
                if (tShortCmt.FTitle == null)
                    tShortCmt.FTitle = string.Empty;
                tShortCmt.FCreatedTime = DateTime.Now;
                tShortCmt.FVisible = true;
                _context.Add(tShortCmt);
                await _context.SaveChangesAsync();
            }
            sessionCheck();
            return RedirectToAction("movieDetails", "movieFront", new { id = tShortCmt.FMovieId });
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
            sessionCheck();
            ViewData["FMemberId"] = tShortCmt.FMemberId;
            ViewData["FMovieId"] = tShortCmt.FMovieId;
            ViewData["FMemberName"] = _context.TMembers.Where(t => t.FMemberId == tShortCmt.FMemberId).Select(t => t.FName).FirstOrDefault();
            ViewData["FMovieName"] = _context.TMovies.Where(t => t.FId == tShortCmt.FMovieId).Select(t => t.FNameCht).FirstOrDefault();
            return View(tShortCmt);
        }

        // POST: ShortCmts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TShortCmt tShortCmt)
        {
            if (id != tShortCmt.FCmtid)
            {
                return NotFound();
            }

            else
            {
                try
                {
                    tShortCmt.FEditedTime = DateTime.Now;
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
                sessionCheck();
                return RedirectToAction("movieDetails", "movieFront", new { id = tShortCmt.FMovieId }); ;

            }

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
            return RedirectToAction("movieDetails", "movieFront", new { id = tShortCmt.FMovieId });
            //return Ok(new { success=true});
        }
        
        [HttpPost]
        public async Task<IActionResult> remove(int id)
        {
            if (_context.TShortCmts == null)
            {
                return Problem("Entity set 'MovieContext.TShortCmts'  is null.");
            }
            var tShortCmt = await _context.TShortCmts.FindAsync(id);
            if (tShortCmt != null)
            {
                var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
                if (tShortCmt.FMemberId != userId)
                {
                    return Ok(new {success=false, error="帳號權限不符"});
                }
                    _context.TShortCmts.Remove(tShortCmt);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });

        }


        private bool TShortCmtExists(int id)
        {
            return (_context.TShortCmts?.Any(e => e.FCmtid == id)).GetValueOrDefault();
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
