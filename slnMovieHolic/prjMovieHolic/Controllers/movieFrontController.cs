using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Controllers
{
    public class movieFrontController : SuperFrontController
    {
        private readonly MovieContext _context;

        public movieFrontController(MovieContext context)
        {
            _context = context;
        }

        // GET: movieFront
        public async Task<IActionResult> MovieIndex()
        {
            var now = DateTime.Now;
            var nowShowingMovies = await _context.TMovies
                .Where(m => m.FScheduleStart <= now && m.FScheduleEnd >= now)
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .Include(t=>t.TSessions)  // 05-25 Stanley
                .ToListAsync();

            var upcomingMovies = await _context.TMovies
                .Where(m => m.FScheduleStart > now)
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .Include(t => t.TSessions)  // 05-25 Stanley
                .ToListAsync();

            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var userName = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER_NAME);
            var isUserLoggedIn = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER) != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
            ViewBag.userName = userName;

            var nowShowingMovieIds = nowShowingMovies.Select(m => m.FId).ToList();
            var IsFavoriteNow = _context.TMemberActions.Where(m => m.FMemberId == userId & nowShowingMovieIds.Contains(m.FMovieId) & m.FActionTypeId == 1).ToList();

            var upcomingMoviesIds = upcomingMovies.Select(m => m.FId).ToList();
            var IsFavoriteComing = _context.TMemberActions.Where(m => m.FMemberId == userId & upcomingMoviesIds.Contains(m.FMovieId) & m.FActionTypeId == 1).ToList();
            var movieViewModel = new CMovieFrontViewModel
            {
                NowShowingMovies = nowShowingMovies,
                UpcomingMovies = upcomingMovies,
                isFavoriteNow = IsFavoriteNow,
                isFavotiteComing = IsFavoriteComing,
            };

            //已登入用
            //sessionCheck();
            string controller = "movieFront";
            string view = "MovieIndex"; 
            //string json=JsonSerializer.Serialize(new { controller, view });
            HttpContext.Session.SetString(CDictionary.SK_CONTROLLER, controller);
            HttpContext.Session.SetString(CDictionary.SK_VIEW, view);

            return View(movieViewModel);
        }

        // GET: movieFront/Details?id=
        //[HttpGet("movieFront/MovieDetails/{id}")]
        public async Task<IActionResult> MovieDetails(int? id)
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

            var tTypeListNames = _context.TTypeLists.Where(t => t.FMovieId == id).Select(t => t.FType.FNameCht).ToArray();
            var tDirectorListNames = _context.TDirectorLists.Where(t => t.FMovieId == id).Select(t => t.FDirector.FNameCht).ToArray();
            var tActorListNames = _context.TActorLists.Where(t => t.FMovieId == id).Select(t => t.FActor.FNameCht).ToArray();



            CMovieFrontViewModel movieViewModel = new CMovieFrontViewModel
            {
                tMovie = tMovie,
                TypeListNames = getNames(tTypeListNames),
                DirectorListNames = getNames(tDirectorListNames),
                ActorListNames = getNames(tActorListNames)
            };

            //已登入用
            sessionCheck();
            string controller = "movieFront";
            string view = "MovieDetails";
            int? parameter =id;
            //string json=JsonSerializer.Serialize(new { controller, view });
            HttpContext.Session.SetString(CDictionary.SK_CONTROLLER, controller);
            HttpContext.Session.SetString(CDictionary.SK_VIEW, view);
            HttpContext.Session.SetInt32(CDictionary.SK_PARAMETER, parameter ?? 0);

            //婷
            string[] paths=getImagesPath(tMovie.FImagePath);
            movieViewModel.movieImagePaths = paths;

            return View(movieViewModel);
        }

        string getNames(Array data)
        {
            string result = "";
            foreach (var item in data)
            {
                result += " " + item;
            }
            return result;
        }

        private bool TMovieExists(int id)
        {
          return (_context.TMovies?.Any(e => e.FId == id)).GetValueOrDefault();
        }

        [NonAction]
        string[] getImagesPath(string path)
        {
            try
            {
                string x = "wwwroot/" + path;
                string[] paths = Directory.GetFiles(x);
                string[] newPaths = new string[paths.Length];
                for(int i =0; i<paths.Length;i++)
                {
                    newPaths[i]=paths[i].Replace("\\", "/").Replace("wwwroot","");
                }
                return newPaths;
            }
            catch
            {
                return null;
            }
            
        }

    }

}
