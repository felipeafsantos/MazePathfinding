using Domain.Model;

namespace Data.Repository
{
    public class MazeRepository : IRepository<Maze>
    {
        private List<Maze> mazes;

        public MazeRepository()
        {
            this.mazes = new List<Maze>();
        }

        public void Add(Maze entity)
        {
            this.mazes.Add(entity);
        }

        public void Delete(Maze entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Maze> GetAll()
        {
           return this.mazes;
        }

        public Maze GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Maze entity)
        {
            throw new NotImplementedException();
        }
    }
}
