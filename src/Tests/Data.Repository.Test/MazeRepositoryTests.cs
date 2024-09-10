using Domain.Model;

namespace Data.Repository.Test
{
    namespace Data.Repository.Tests
    {
        public class MazeRepositoryTests
        {
            [Fact]
            public void Add_AddMazeToRepository()
            {
                // Arrange
                var repository = new MazeRepository();
                var maze = new Maze { Id = Guid.NewGuid() };

                // Act
                repository.Add(maze);
                var allMazes = repository.GetAll();

                // Assert
                Assert.Contains(maze, allMazes);
            }

            [Fact]
            public void GetAll_ReturnsAllMazes()
            {
                // Arrange
                var repository = new MazeRepository();
                var maze1 = new Maze { Id = Guid.NewGuid() };
                var maze2 = new Maze { Id = Guid.NewGuid() };

                repository.Add(maze1);
                repository.Add(maze2);

                // Act
                var allMazes = repository.GetAll();

                // Assert
                Assert.Equal(2, allMazes.Count());
                Assert.Contains(maze1, allMazes);
                Assert.Contains(maze2, allMazes);
            }
        }
    }
}