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

        public IActionResult MovieView()
        {
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
                return Json(null);
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
                return Json(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CMovieBackViewModel edition)
        {
            string error = "";
            if (edition.FNameCht == null)
                error += "電影中文名稱，";
            if (edition.FNameEng == null)
                error += "電影英文名稱，";
            if (edition.FRatingId == -1)
                error += "電影分級，";
            if (error != "")
                return Json(new { isError = true, message = $"請輸入{error}謝謝！" });
            try
            {
                var movie = _db.TMovies.Where(m => m.FId == edition.FId).FirstOrDefault();
                if (movie == null)
                    return Json(new { isError = true, message = "查無電影，請重新再試！" });
                movie.FNameCht = edition.FNameCht;
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
                return Json(new { isError = false, message = "電影修改完成！" });
            }
            catch (Exception e)
            {
                return Json(new { isError = true, message = "電影修改異常，請重新再試！" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CMovieBackViewModel create)
        {
            string error = "";
            if (create.FNameCht == null)
                error += "電影中文名稱，";
            if (create.FNameEng == null)
                error += "電影英文名稱，";
            if (create.FRatingId == -1)
                error += "電影分級，";
            if (error != "")
                return Json(new { isError = true, message = $"請輸入{error}謝謝！" });
            try
            {
                using (var tran = _db.Database.BeginTransaction())
                {
                    bool repeated = _db.TMovies.Where(movie => movie.FNameCht == create.FNameCht || movie.FNameEng == create.FNameEng).Any();
                    if (repeated)
                        return Json(new { isError = true, message = "電影中文或英文名稱有重複，請重新確認！" });

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
                    return Json(new { isError = false, message = "電影新增成功！" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { isError = true, message = "電影新增異常，請重新再試！" });
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
                    return Json(new { isError = true, message = "查無電影，請重新再試！" });
                if (movie.TSessions.Any())
                    return Json(new { isError = true, message = "該電影有場次，無法刪除" });

                _db.TMovies.Remove(movie);
                _db.SaveChanges();

                return Json(new { isError = false, message = "電影刪除成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, message = "電影刪除異常，請重新再試！" });
            }

        }
    }
}
