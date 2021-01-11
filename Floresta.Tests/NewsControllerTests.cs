using Floresta.Controllers;
using Floresta.Interfaces;
using Floresta.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace Floresta.Tests
{
    public class NewsControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfTests()
        {

            // Arrange
            var mock = new Mock<IRepository<News>>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestNews());
            var controller = new NewsController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<News>>(viewResult.Model);
            Assert.Equal(GetTestNews().Count, model.Count());
        }
        private List<News> GetTestNews()
        {
            var news = new List<News>
            {
                new News { Id=1, Title="First title", Content="some text"},
                new News { Id=2, Title="Second title", Content="some text"},
            };
            return news;
        }
        [Fact]
        public async void AddNewsReturnsViewResultWithNewsModel()
        {
            // Arrange
            var mock = new Mock<IRepository<News>>();
            var controller = new NewsController(mock.Object);
            controller.ModelState.AddModelError("Title", "Required");
            News newNews = new News();

            // Act
            var result = await controller.Create(newNews);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newNews, viewResult?.Model);
        }


        [Fact]
        public async void AddNewsReturnsARedirectAndAddsNews()
        {
            // Arrange
            var mock = new Mock<IRepository<News>>();
            var controller = new NewsController(mock.Object);
            var newNews = new News()
            {
                Title = "First title"
            };

            // Act
            var result = await controller.Create(newNews);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.AddAsync(newNews));
        }

        [Fact]
        public void GetNewsReturnsNotFoundResultWhenNewsNotFound()
        {
            // Arrange
            int testNewsId = 1;
            var mock = new Mock<IRepository<News>>();
            mock.Setup(repo => repo.GetById(testNewsId))
                .Returns(null as News);
            var controller = new NewsController(mock.Object);

            // Act
            var result = controller.Edit(testNewsId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetNewsReturnsViewResultWithNews()
        {
            // Arrange
            int testNewsId = 1;
            var mock = new Mock<IRepository<News>>();
            mock.Setup(repo => repo.GetById(testNewsId))
                .Returns(GetTestNews().FirstOrDefault(p => p.Id == testNewsId));
            var controller = new NewsController(mock.Object);

            // Act
            var result = controller.Edit(testNewsId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<News>(viewResult.ViewData.Model);
            Assert.Equal("First title", model.Title);
            Assert.Equal("some text", model.Content);
        }
        [Fact]
        public async void CreatePostAction_SaveModel()
        {
            // arrange
            var mock = new Mock<IRepository<News>>();
            var news = new News();
            var controller = new NewsController(mock.Object);
            // act
            RedirectToRouteResult result = await controller.Create(news) as RedirectToRouteResult;
            // assert
            mock.Verify(a => a.AddAsync(news));
        }
        [Fact]
        public async void DeletePostAction_SaveModel()
        {
            // arrange
            var mock = new Mock<IRepository<News>>();
            int newsId = 1;
            var controller = new NewsController(mock.Object);
            // act
            RedirectToRouteResult result = await controller.DeleteConfirmed(newsId) as RedirectToRouteResult;
            // assert
            mock.Verify(a => a.DeleteAsync(newsId));
        }
    }
}
