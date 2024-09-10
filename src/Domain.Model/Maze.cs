using System.Drawing;

namespace Domain.Model
{
    public class Maze
    {
        public Guid Id { get; set; }

        public string MazeConfig { get; set; }

        public Point Start { get; set; }

        public Point Goal { get; set; }

        public List<Point>? Solution { get; set; }
    }
}
