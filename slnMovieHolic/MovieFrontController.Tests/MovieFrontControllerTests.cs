using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Controllers;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using Xunit;

namespace MovieFrontController.Tests
{
    public class MovieFrontControllerTests
    {
        [Fact]
        public async Task MovieIndex_ReturnsViewWithMovieViewModel()
        {
            // 安排 (Arrange)
            var dbContextOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MovieContext(dbContextOptions))
            {
                var controller = new movieFrontController(context);

                // 行動 (Act)
                var result = await controller.MovieIndex() as ViewResult;

                // 斷言 (Assert)
                Assert.NotNull(result);
                Assert.IsType<CMovieFrontViewModel>(result.Model);

                var viewModel = result.Model as CMovieFrontViewModel;
                // 添加您期望的值的斷言
            }
        }

        [Fact]
        public async Task MovieDetails_WithValidId_ReturnsViewWithMovie()
        {
            // 安排 (Arrange)
            var dbContextOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MovieContext(dbContextOptions))
            {
                var controller = new movieFrontController(context);
                int movieId = 1;

                // 行動 (Act)
                var result = await controller.MovieDetails(movieId) as ViewResult;

                // 斷言 (Assert)
                Assert.NotNull(result);
                Assert.IsType<TMovie>(result.Model);

                var movie = result.Model as TMovie;
                // 添加您期望的電影詳細資料的斷言
            }
        }
    }
}