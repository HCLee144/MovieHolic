using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Controllers
{
    public class movieBackController : BackSuperController
    {
        private readonly MovieContext _db;
        private readonly IWebHostEnvironment _enviro;

        public movieBackController(MovieContext context, IWebHostEnvironment p)
        {
            _db = context;
            _enviro = p;
        }

        public IActionResult MovieView(int? messageCode)
        {
            if (messageCode != null)
                ViewBag.MessageCode = messageCode;
            return View();
        }

        public IActionResult queryMovieByName(string movieName)
        {
            try
            {
                var movie = _db.TMovies.Where(m => m.FNameCht == movieName).FirstOrDefault();
                return Json(movie);
            }
            catch (Exception ex)
            {
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 4 });
            }

        }

        public IActionResult loadRecentMovies()
        {
            try
            {
                var movies = _db.TMovies.AsEnumerable().OrderBy(movie => movie.FScheduleStart)
                                .Where(movie => movie.FScheduleEnd >= DateTime.Now).Select(movie => movie.FNameCht);

                return Json(movies);
            }
            catch (Exception ex)
            {
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 4 });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CMovieBackViewModel edition)
        {
            try
            {
                TMovie movie = _db.TMovies.Where(m => m.FId == edition.FId).FirstOrDefault();
                if (movie != null)
                {
                    if (edition.FNameCht != null)
                        movie.FNameCht = edition.FNameCht;
                    if (edition.FNameEng != null)
                        movie.FNameEng = edition.FNameEng;
                    movie.FScheduleStart = edition.FScheduleStart;
                    movie.FScheduleEnd = edition.FScheduleEnd;
                    movie.FRatingId = edition.FRatingId;
                    if (edition.FShowLength != null)
                        movie.FShowLength = edition.FShowLength;
                    if (edition.FInteroduce != null)
                        movie.FInteroduce = edition.FInteroduce;
                    if (edition.FTrailerLink != null)
                        movie.FTrailerLink = edition.FTrailerLink;
                    if (edition.FPrice != null)
                        movie.FPrice = edition.FPrice;
                    if (edition.image != null)
                    {
                        if (movie.FPosterPath != null)
                        {
                            string path = _enviro.WebRootPath + "/" + movie.FPosterPath;
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                edition.image.CopyTo(fileStream);
                            }
                        }
                        else
                        {
                            string photoName = $"images/moviePosters/{movie.FId}/" + Guid.NewGuid().ToString() + ".jpg";
                            string path = _enviro.WebRootPath + "/" + photoName;
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                edition.image.CopyTo(fileStream);
                            }
                            movie.FPosterPath = photoName;
                        }
                    }
                    _db.SaveChanges();
                }
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 2 });
            }
            catch (Exception e)
            {
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 4 });
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CMovieBackViewModel create)
        {
            if (create.FRatingId == -1)
            {
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 4 });
            }
            try
            {
                using (var tran = _db.Database.BeginTransaction())
                {
                    bool repeated = _db.TMovies.Where(movie => movie.FNameCht == create.FNameCht || movie.FNameEng == create.FNameEng).Any();
                    if (repeated)
                        return RedirectToAction("MovieView");

                    TMovie movie = new TMovie()
                    {
                        FNameCht = create.FNameCht,
                        FNameEng = create.FNameEng,
                        FScheduleStart = create.FScheduleStart,
                        FScheduleEnd = create.FScheduleEnd,
                        FShowLength = create.FShowLength,
                        FInteroduce = create.FInteroduce,
                        FTrailerLink = create.FTrailerLink,
                        FPrice = create.FPrice,
                        FRatingId = create.FRatingId,
                    };
                    _db.TMovies.Add(movie);
                    _db.SaveChanges();


                    //新增儲存資料夾、儲存圖片
                    var newMovie = _db.TMovies.OrderByDescending(movie => movie.FId).First();
                    string folderPath = $"images/moviePosters/{newMovie.FId}/";
                    Directory.CreateDirectory(_enviro.WebRootPath + "/" + folderPath);

                    if (create.image != null)
                    {
                        string photoPath = folderPath + Guid.NewGuid().ToString() + ".jpg";
                        using (var fileStream = new FileStream(_enviro.WebRootPath + "/" + photoPath, FileMode.Create))
                        {
                            create.image.CopyTo(fileStream);
                        }
                        newMovie.FPosterPath = photoPath;
                        _db.SaveChanges();
                    }
                    tran.Commit();
                    return RedirectToAction("MovieView", "movieBack", new { messageCode = 1 });
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 4 });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? FId)
        {
            try
            {
                var movie = _db.TMovies.Include(m => m.TSessions).AsEnumerable().FirstOrDefault(m => m.FId == FId);
                if (movie == null)
                    return RedirectToAction("MovieView", "movieBack", new { messageCode = 5 });
                if (movie.TSessions.Any())
                    return RedirectToAction("MovieView", "movieBack", new { messageCode = 5 });

                _db.TMovies.Remove(movie);
                _db.SaveChanges();

                return RedirectToAction("MovieView", "movieBack", new { messageCode = 3 });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MovieView", "movieBack", new { messageCode = 4 });
            }

        }
    }
}
