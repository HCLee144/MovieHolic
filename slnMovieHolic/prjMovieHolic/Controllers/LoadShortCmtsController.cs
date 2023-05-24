using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadShortCmtsController : ControllerBase
    {
        private readonly MovieContext _context;

        public LoadShortCmtsController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet("average-star-level/{movieID}")]
        public async Task<IActionResult> GetAverageRating(int movieID)
        {
            var q = from cmt in _context.TShortCmts.Where(t => t.FMovieId == movieID) select cmt.FRate;
            if (q.Count() == 0)
                return Ok(new { starLevel = 0 });
            else return Ok(new { starLevel = q.Average()});
        }
    

    // GET: api/LoadShortCmts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TShortCmt>>> GetTShortCmts()
    {
        if (_context.TShortCmts == null)
        {
            return NotFound();
        }
        return await _context.TShortCmts.ToListAsync();
    }

    // GET: api/LoadShortCmts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TShortCmt>> GetTShortCmt(int id)
    {
        if (_context.TShortCmts == null)
        {
            return NotFound();
        }
        var tShortCmt = await _context.TShortCmts.FindAsync(id);

        if (tShortCmt == null)
        {
            return NotFound();
        }

        return tShortCmt;
    }

    // PUT: api/LoadShortCmts/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTShortCmt(int id, TShortCmt tShortCmt)
    {
        if (id != tShortCmt.FCmtid)
        {
            return BadRequest();
        }

        _context.Entry(tShortCmt).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TShortCmtExists(id))
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

    // POST: api/LoadShortCmts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TShortCmt>> PostTShortCmt(TShortCmt tShortCmt)
    {
        if (_context.TShortCmts == null)
        {
            return Problem("Entity set 'MovieContext.TShortCmts'  is null.");
        }
        _context.TShortCmts.Add(tShortCmt);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTShortCmt", new { id = tShortCmt.FCmtid }, tShortCmt);
    }

    // DELETE: api/LoadShortCmts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTShortCmt(int id)
    {
        if (_context.TShortCmts == null)
        {
            return NotFound();
        }
        var tShortCmt = await _context.TShortCmts.FindAsync(id);
        if (tShortCmt == null)
        {
            return NotFound();
        }

        _context.TShortCmts.Remove(tShortCmt);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TShortCmtExists(int id)
    {
        return (_context.TShortCmts?.Any(e => e.FCmtid == id)).GetValueOrDefault();
    }
}
}
