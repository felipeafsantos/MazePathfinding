namespace Application.Service.Tests
{
    using Xunit;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Domain.Model;
    using Data.Repository;
    using Application.Service.PathfindingStrategies;

    public class MazeServiceTests
    {
        private readonly Mock<IMazeStrategy> mazeStrategyMock;
        private readonly Mock<IRepository<Maze>> mazeRepositoryMock;
        private readonly MazeService mazeService;

        public MazeServiceTests()
        {
            mazeStrategyMock = new Mock<IMazeStrategy>();
            mazeRepositoryMock = new Mock<IRepository<Maze>>();

            mazeService = new MazeService(mazeStrategyMock.Object, mazeRepositoryMock.Object);
        }

        [Fact]
        public void GetAllMazes_ShouldReturnAllMazes()
        {
            // Arrange
            var mazes = new List<Maze>
        {
            new Maze { Id = Guid.NewGuid(), MazeConfig = "Maze1", Solution = new List<Point>() },
            new Maze { Id = Guid.NewGuid(), MazeConfig = "Maze2", Solution = new List<Point>() }
        };
            mazeRepositoryMock.Setup(repo => repo.GetAll()).Returns(mazes);

            // Act
            var result = this.mazeService.GetAllMazes();

            // Assert
            Assert.Equal(mazes, result);
        }

        [Fact]
        public void AddMaze_ShouldAddMazeAndReturnMaze_WhenValidMazeIsProvided()
        {
            // Arrange
            var inputMaze = "S__\n___\n__G";
            var start = new Point(0, 0);
            var goal = new Point(2, 2);
            var solution = new List<Point> { start, new Point(1, 1), goal };

            // Setup repository mock
            mazeRepositoryMock.Setup(repo => repo.Add(It.IsAny<Maze>()));

            // Setup Maze Strategy
            mazeStrategyMock
                .Setup(repo => repo.BuildSolution(It.IsAny<char[,]>(), It.IsAny<Point>(), It.IsAny<Point>()))
                .Returns(new List<Point> { new Point { X = 0, Y = 1 } });

            // Act
            var result = this.mazeService.AddMaze(inputMaze);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inputMaze, result.MazeConfig);
            Assert.Equal(solution.First(), result.Start);
            Assert.Equal(solution.Last(), result.Goal);
            mazeRepositoryMock.Verify(repo => repo.Add(It.IsAny<Maze>()), Times.Once);
        }

        [Fact]
        public void AddMaze_ShouldReturnNull_WhenNoSolutionExists()
        {
            // Arrange
            var inputMaze = "S__\n###\n__G";  // Blocked path

            // Act
            var result = this.mazeService.AddMaze(inputMaze);

            // Assert
            Assert.Null(result);
            mazeRepositoryMock.Verify(repo => repo.Add(It.IsAny<Maze>()), Times.Never);
        }

        [Fact]
        public void AddMaze_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var inputMaze = "S__\n___\n__G";
            mazeRepositoryMock.Setup(repo => repo.Add(It.IsAny<Maze>())).Throws(new Exception("Repository failure"));

            // Setup Maze Strategy
            mazeStrategyMock
                .Setup(repo => repo.BuildSolution(It.IsAny<char[,]>(), It.IsAny<Point>(), It.IsAny<Point>()))
                .Returns(new List<Point> { new Point { X = 0, Y = 1 } });

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => this.mazeService.AddMaze(inputMaze));
            Assert.Equal("Error trying to add a new Maze.", ex.Message);
        }

        [Fact]
        public void ParseMaze_ShouldReturnCorrectMaze_WhenValidInputIsProvided()
        {
            // Arrange
            string[] mazeInput = { "S__", "___", "__G" };
            var expectedStart = new Point(0, 0);
            var expectedGoal = new Point(2, 2);

            // Act
            var methodInfo = typeof(MazeService).GetMethod("ParseMaze", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var parameters = new object[] { mazeInput, null, null };
            var parsedMaze = (char[,])methodInfo?.Invoke(this.mazeService, parameters);

            // Assert
            Assert.Equal('S', parsedMaze[0, 0]);
            Assert.Equal('G', parsedMaze[2, 2]);
            Assert.Equal(expectedStart, (Point)parameters[1]);
            Assert.Equal(expectedGoal, (Point)parameters[2]);
        }
    }

}