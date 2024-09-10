using System.Drawing;

namespace Application.Service.PathfindingStrategies
{
    public interface IMazeStrategy
    {
        List<Point>? BuildSolution(char[,] maze, Point start, Point goal);
    }
}
