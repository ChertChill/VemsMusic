﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VemsMusic.Controllers;
using VemsMusic.Interfaces;
using VemsMusic.Models;
using VemsMusic.Models.ViewModels;
using VemsMusic.Other_Data.Interfaces;
using VemsMusic.Other_Data.ViewModels;
using Xunit;

namespace UnitTests
{
    public class HomeControllerTest
    {
        [Fact]
        public async void IndexViewTestWithNonZeroGenres()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            mockGenre.Setup(repo => repo.GetAllGenresAsync()).ReturnsAsync(TestData.GetTestGenres);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GenreViewModel>(viewResult.Model);
            Assert.NotEmpty(model.AllGenres);
        }
        [Fact]
        public async void IndexViewTestWithZeroGenres()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            mockGenre.Setup(repo => repo.GetAllGenresAsync()).ReturnsAsync(TestData.GetZeroGenres);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.Index();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/Home/NoItems/Жанры не добавлены", redirectResult.Url);
        }

        //TODO The test doesn't work. Problems getting id value from cookies
        [Fact]
        public async void NewMusicTest()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            var mockHttpContex = new Mock<HttpContext>();
            mockHttpContex.Setup(repo => repo.Request.Cookies["id"]).Returns("1");
            mockMusic.Setup(repo => repo.GetAllMusicAsync()).ReturnsAsync(TestData.GetTestMusics);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.NewMusic();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MusicViewModel>(viewResult.Model);
            Assert.NotEmpty(model.AllMusic);
        }

        [Fact]
        public async void NewMusicTestWithZeroMusic()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            mockMusic.Setup(repo => repo.GetAllMusicAsync()).ReturnsAsync(TestData.GetZeroMusic);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.NewMusic();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/Home/NoItems/Музыка не добавлена", redirectResult.Url);
        }

        [Fact]
        public async void InGenreTest()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            mockGenre.Setup(repo => repo.GetGenreByIdAsync(1)).ReturnsAsync(TestData.HipHop);
            mockGroup.Setup(repo => repo.GetMusicalGroupsAsync()).ReturnsAsync(TestData.GetTestGroups);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.InGenre(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GroupsViewModel>(viewResult.Model);
            Assert.NotEmpty(model.AllGroups);
        }

        [Fact]
        public async void InGenreTestWithZeroGenresAsync()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            mockGenre.Setup(repo => repo.GetGenreByIdAsync(1)).ReturnsAsync(new Genre
            {
                Name = "Рок",
                Description = ""
            });
            mockGroup.Setup(repo => repo.GetMusicalGroupsAsync()).ReturnsAsync(TestData.GetZeroGroup);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.InGenre(3);

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/Home/NoItems/Группы не добавлены", redirectResult.Url);
        }
        [Fact]
        public async void ExecutorsTest()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            mockGroup.Setup(repo => repo.GetMusicalGroupsAsync()).ReturnsAsync(TestData.GetTestGroups);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.Executors();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<GroupsViewModel>(viewResult.Model);
            Assert.NotEmpty(model.AllGroups);
        }
        //TODO The test doesn't work. Problems getting id value from cookies
        [Fact]
        public async void AllMusicTest()
        {
            var mockGenre = new Mock<IAllGenre>();
            var mockGroup = new Mock<IAllGroups>();
            var mockMusic = new Mock<IAllMusic>();
            var mockHttpContex = new Mock<HttpContext>();
            mockHttpContex.Setup(repo => repo.Request.Cookies["id"]).Returns("1");
            mockMusic.Setup(repo => repo.GetAllMusicAsync()).ReturnsAsync(TestData.GetTestMusics);
            var controller = new HomeController(mockGenre.Object, mockGroup.Object, mockMusic.Object);

            var result = await controller.AllMusic();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MusicViewModel>(viewResult.Model);
            Assert.NotEmpty(model.AllMusic);
        }
    }
}
