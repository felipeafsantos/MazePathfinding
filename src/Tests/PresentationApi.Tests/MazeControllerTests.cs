using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Application.Service;
using PresentationApi.Controllers;
using Domain.Model;
using System.Drawing;

namespace PresentationApi.Tests
{
    public class MazeControllerTests
    {
        private readonly Mock<IMazeService> mazeServiceMock;
        private readonly MazeController mazeController;

        public MazeControllerTests()
        {
            this.mazeServiceMock = new Mock<IMazeService>();
            this.mazeController = new MazeController(mazeServiceMock.Object);
        }

        [Fact]
        public void UploadMaze_WithNullMaze_ReturnsBadRequest()
        {
            // Act
            var result = this.mazeController.UploadMaze(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The maze format is invalid.", badRequestResult.Value);
        }

        [Fact]
        public void UploadMaze_WithInvalidMaze_ReturnsBadRequest()
        {
            // Arrange
            var invalidMaze = "InvalidMaze";
            mazeServiceMock.Setup(s => s.AddMaze(It.IsAny<string>())).Returns(null as Maze);

            // Act
            var result = this.mazeController.UploadMaze(invalidMaze);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The maze format is invalid.", badRequestResult.Value);
        }

        [Fact]
        public void UploadMaze_WithValidUnsolvableMaze_ReturnsBadRequest()
        {
            // Arrange
            var unsolvableMaze = "SXG";

            mazeServiceMock.Setup(s => s.AddMaze(It.IsAny<string>())).Returns(null as Maze);

            // Act
            var result = this.mazeController.UploadMaze(unsolvableMaze);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Maze is invalid. It is not possible to solve.", badRequestResult.Value);
        }

        [Fact]
        public void UploadMaze_WithValidSolvableMaze_ReturnsCreated()
        {
            // Arrange
            var solvableMaze = "SG";
            var createdMaze = new Maze 
            { 
                Solution = new List<Point>() 
                { 
                    new Point(0,0),
                    new Point(0,1)
                }
            };

            mazeServiceMock.Setup(s => s.AddMaze(It.IsAny<string>())).Returns(createdMaze);

            // Act
            var result = this.mazeController.UploadMaze(solvableMaze);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(createdMaze, createdResult.Value);
        }
    }
}
