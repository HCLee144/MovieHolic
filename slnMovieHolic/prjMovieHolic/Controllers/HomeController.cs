using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Diagnostics;
using System.Text.Json;
using ZXing.QrCode.Internal;

namespace prjMovieHolic.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieContext _context;

        public HomeController(MovieContext context)
        {
            _context = context;
        }

        // GET: movieFront
        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;
            var nowShowingMovies = await _context.TMovies
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .Where(m => m.FScheduleStart <= now && m.FScheduleEnd >= now)
                .ToListAsync();

            var upcomingMovies = await _context.TMovies
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .Where(m => m.FScheduleStart > now)
                .ToListAsync();

            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var isUserLoggedIn = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER) != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;

            
            var nowShowingMovieIds = nowShowingMovies.Select(m => m.FId).ToList();
            var IsFavoriteNow = _context.TMemberActions.Where(m => m.FMemberId == userId & nowShowingMovieIds.Contains(m.FMovieId) & m.FActionTypeId == 1).ToList();

            var upcomingMoviesIds = upcomingMovies.Select(m => m.FId).ToList();
            var IsFavoriteComing = _context.TMemberActions.Where(m => m.FMemberId == userId & upcomingMoviesIds.Contains(m.FMovieId) & m.FActionTypeId == 1).ToList();
            var movieViewModel = new CMovieFrontViewModel
            {
                NowShowingMovies = nowShowingMovies,
                UpcomingMovies = upcomingMovies,
                isFavoriteNow = IsFavoriteNow,
                isFavotiteComing=IsFavoriteComing,
            };
            //登入用
           
            //todo 製作Session把Action和Controller存進去
            string controller = "Home";
            string view = "Index";
            //string json=JsonSerializer.Serialize(new { controller, view });
            HttpContext.Session.SetString(CDictionary.SK_CONTROLLER, controller);
            HttpContext.Session.SetString(CDictionary.SK_VIEW, view);


            //快速訂票--Ting
            var sessions = _context.TSessions.Where(s => s.FStartTime.Date > DateTime.Now.Date);



            return View(movieViewModel);
        }

        // GET: movieFront/Details?id=
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

            // 指定视图的路径
            return View("~/Views/movieFront/MovieDetails.cshtml", tMovie);
        }

        private bool TMovieExists(int id)
        {
            return (_context.TMovies?.Any(e => e.FId == id)).GetValueOrDefault();
        }


        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //收藏功能
        
        public IActionResult favoriteChange(int FMemberId, int FMovieId)
        {
            //step1. 判斷有無資料
            var IsFavorite = _context.TMemberActions.Any(m => m.FMemberId == FMemberId & m.FMovieId == FMovieId);
            //step2. 判斷喜歡或不喜歡
            if (IsFavorite)
            {   //喜歡=>取消收藏
                var favorite = _context.TMemberActions.OrderByDescending(m=>m.FTimeStamp).FirstOrDefault(m=>m.FActionTypeId==1 & m.FMemberId == FMemberId & m.FMovieId == FMovieId);
               
                if (favorite != null)
                {
                    favorite.FActionTypeId = 2;
                    favorite.FTimeStamp = DateTime.Now;
                    _context.SaveChanges();
                    return Content("取消收藏");
                }

                //不喜歡 => 加入收藏
                var Notfavorite = _context.TMemberActions.OrderByDescending(m => m.FTimeStamp).FirstOrDefault(m => m.FActionTypeId == 2 & m.FMemberId == FMemberId & m.FMovieId == FMovieId);
                if (Notfavorite != null)
                {

                    Notfavorite.FActionTypeId = 1;
                    Notfavorite.FTimeStamp = DateTime.Now;
                    _context.SaveChanges();
                    return Content("取消後加入收藏");
                }
                return Content("無資料");

            }
            else
            { //加入收藏
                TMemberAction action = new TMemberAction();
                action.FMemberId = FMemberId;
                action.FMovieId = FMovieId;
                action.FActionTypeId = 1;
                action.FTimeStamp = DateTime.Now;
                _context.TMemberActions.Add(action);
                _context.SaveChanges();
                return Content("新加入收藏");
            }
            
        }

    }
}