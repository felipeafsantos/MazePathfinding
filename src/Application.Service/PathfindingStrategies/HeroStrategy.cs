using System.Drawing;

namespace Application.Service.PathfindingStrategies
{
    public class HeroStrategy: IMazeStrategy
    {
        public List<Point>? BuildSolution(char[,] maze, Point start, Point goal)
        {
            List<Point> path = new List<Point>();
            List<Point> visited = new List<Point>();

            // Step 1: Start from the starting point and define an empty path
            if (BuildPath(maze, start, goal, path, visited))
            {
                return path;
            }

            return null;
        }

        private bool BuildPath(char[,] maze, Point current, Point goal, List<Point> path, List<Point> visited)
        {
            // Step 2: Check if we reached the goal
            if (current == goal)
            {
                path.Add(current);

                return true;
            }

            // Mark the current cell as visited
            visited.Add(current);

            // Add the current position to the path
            path.Add(current);

            // Step 3: Get possible moves (up, down, left, right)
            List<Point> moves = GetPossibleMoves(maze, current);

            // Step 4: Remove already visited moves
            moves.RemoveAll(move => visited.Contains(move));

            // Step 5 & 6: Try all remaining moves
            foreach (var move in moves)
            {
                if (BuildPath(maze, move, goal, path, visited))
                {
                    return true;
                }
            }

            // No solution found on this path
            path.RemoveAt(path.Count - 1);

            visited.Remove(current);

            return false;
        }

        private List<Point> GetPossibleMoves(char[,] maze, Point current)
        {
            List<char> validCharMoves = new List<char>() { 'S', 'G', '_' };

            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            List<Point> moves = new List<Point>();

            // Try Move Up
            if (current.X > 0 && validCharMoves.Contains(maze[current.X - 1, current.Y]))
                moves.Add(new Point(current.X - 1, current.Y));

            // Try Move Down
            if (current.X < rows - 1 && validCharMoves.Contains(maze[current.X + 1, current.Y]))
                moves.Add(new Point(current.X + 1, current.Y));

            // Try Move Left
            if (current.Y > 0 && validCharMoves.Contains(maze[current.X, current.Y - 1]))
                moves.Add(new Point(current.X, current.Y - 1));

            // Try Move Right
            if (current.Y < cols - 1 && validCharMoves.Contains(maze[current.X, current.Y + 1]))
                moves.Add(new Point(current.X, current.Y + 1));

            return moves;
        }
    }
}
