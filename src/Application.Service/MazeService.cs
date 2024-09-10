using System.Drawing;
using Application.Service.PathfindingStrategies;
using Data.Repository;
using Domain.Model;

namespace Application.Service
{
    public class MazeService : IMazeService
    {
        private readonly IMazeStrategy mazeStrategy;
        private readonly IRepository<Maze> mazeRepository;

        public MazeService(
            IMazeStrategy strategy,
            IRepository<Maze> mazeRepository)
        {
            this.mazeStrategy = strategy;
            this.mazeRepository = mazeRepository;
        }

        public IEnumerable<Maze> GetAllMazes()
        {
            return mazeRepository.GetAll();
        }

        public Maze? AddMaze(string maze)
        {
            try
            {
                char[,] parsedMaze = ParseMaze(maze.Split('\n'), out Point start, out Point goal);

                var solution = this.mazeStrategy.BuildSolution(parsedMaze, start, goal);

                if (solution is null)
                {
                    return null;
                }

                var createdMaze = new Maze()
                {
                    Id = Guid.NewGuid(),
                    MazeConfig = @maze,
                    Solution = solution,
                    Start = start,
                    Goal = goal
                };

                this.mazeRepository.Add(createdMaze);

                return createdMaze;
            }
            catch (Exception ex)
            {
                throw new Exception("Error trying to add a new Maze.", null) ;
            }
        }

        private char[,] ParseMaze(string[] mazeInput, out Point start, out Point goal)
        {
            int rows = mazeInput.Length;
            int cols = mazeInput[0].Length;

            char[,] maze = new char[rows, cols];

            start = new Point(-1, -1);
            goal = new Point(-1, -1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    maze[i, j] = mazeInput[i][j];

                    if (maze[i, j] == 'S')
                    {
                        start = new Point(i, j);
                    }
                    else if (maze[i, j] == 'G')
                    {
                        goal = new Point(i, j);
                    }
                }
            }

            return maze;
        }
    }
}
