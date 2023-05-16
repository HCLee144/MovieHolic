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
            // �w�� (Arrange)
            var dbContextOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MovieContext(dbContextOptions))
            {
                var controller = new movieFrontController(context);

                // ��� (Act)
                var result = await controller.MovieIndex() as ViewResult;

                // �_�� (Assert)
                Assert.NotNull(result);
                Assert.IsType<CMovieFrontViewModel>(result.Model);

                var viewModel = result.Model as CMovieFrontViewModel;
                // �K�[�z���檺�Ȫ��_��
            }
        }

        [Fact]
        public async Task MovieDetails_WithValidId_ReturnsViewWithMovie()
        {
            // �w�� (Arrange)
            var dbContextOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MovieContext(dbContextOptions))
            {
                var controller = new movieFrontController(context);
                int movieId = 1;

                // ��� (Act)
                var result = await controller.MovieDetails(movieId) as ViewResult;

                // �_�� (Assert)
                Assert.NotNull(result);
                Assert.IsType<TMovie>(result.Model);

                var movie = result.Model as TMovie;
                // �K�[�z���檺�q�v�ԲӸ�ƪ��_��
            }
        }
    }
}