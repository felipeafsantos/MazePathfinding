using Domain.Model;

namespace Application.Service
{
    public interface IMazeService
    {
        Maze? AddMaze(string maze);

        IEnumerable<Maze> GetAllMazes();
    }
}
