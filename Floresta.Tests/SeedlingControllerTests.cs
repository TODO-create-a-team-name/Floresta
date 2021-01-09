using Floresta.Controllers;
using Floresta.Interfaces;
using Floresta.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ClassLibrary
{
    public class SeedlingsControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfTests()
        {

            // Arrange
            var mock = new Mock<IRepository<Seedling>>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestSeedlings());
            var controller = new SeedlingsController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Seedling>>(viewResult.Model);
            Assert.Equal(GetTestSeedlings().Count, model.Count());
        }
        private List<Seedling> GetTestSeedlings()
        {
            var seedlings = new List<Seedling>
            {
                new Seedling { Id=1, Name="pear", Amount = 200, Price = 100},
                new Seedling { Id=2, Name="apple", Amount = 100, Price = 100},
                new Seedling { Id=3, Name="oak", Amount = 20, Price = 300},
            };
            return seedlings;
        }
        [Fact]
        public void AddSeedlingReturnsViewResultWithSeedlingModel()
        {
            // Arrange
            var mock = new Mock<IRepository<Seedling>>();
            var controller = new SeedlingsController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Seedling newSeedling = new Seedling();

            // Act
            var result = controller.Create(newSeedling);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newSeedling, viewResult?.Model);
        }

        [Fact]
        public void AddSeedlingReturnsARedirectAndAddsSeedling()
        {
            // Arrange
            var mock = new Mock<IRepository<Seedling>>();
            var controller = new SeedlingsController(mock.Object);
            var newSeedling = new Seedling()
            {
                Name = "pear"
            };

            // Act
            var result = controller.Create(newSeedling);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.AddAsync(newSeedling));
        }

        [Fact]
        public void GetSeedlingReturnsNotFoundResultWhenSeedlingNotFound()
        {
            // Arrange
            int testSeedlingId = 1;
            var mock = new Mock<IRepository<Seedling>>();
            mock.Setup(repo => repo.GetById(testSeedlingId))
                .Returns(null as Seedling);
            var controller = new SeedlingsController(mock.Object);

            // Act
            var result = controller.Edit(testSeedlingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetSeedlingReturnsViewResultWithSeedling()
        {
            // Arrange
            int testSeedlingId = 1;
            var mock = new Mock<IRepository<Seedling>>();
            mock.Setup(repo => repo.GetById(testSeedlingId))
                .Returns(GetTestSeedlings().FirstOrDefault(p => p.Id == testSeedlingId));
            var controller = new SeedlingsController(mock.Object);

            // Act
            var result = controller.Edit(testSeedlingId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Seedling>(viewResult.ViewData.Model);
            Assert.Equal("pear", model.Name);
            Assert.Equal(200, model.Amount);
            Assert.Equal(testSeedlingId, model.Id);
        }
    }
}
