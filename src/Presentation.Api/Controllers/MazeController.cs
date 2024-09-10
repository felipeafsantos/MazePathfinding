using Application.Service;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PresentationApi.Controllers
{

    [Route("api/mazes")]
    public class MazeController : ControllerBase
    {
        private readonly IMazeService mazeService;

        public MazeController(IMazeService mazeService)
        {
            this.mazeService = mazeService;
        }

        /// <summary>
        /// Add a new maze
        /// </summary>
        /// <param name="maze">The Maze represented as a string</param>
        /// <returns>Created maze with the solution</returns>
        /// <remarks>
        /// Sample request (stringfy the text before send):
        ///
        ///     POST /api/mazes
        ///     "S_________\n*XXXXXXXX_\n*X*_____X_\n*X*XXXX_X_\n*X*X__X_X_\n*X*X__X_X_\n*X*X____X_\n*X*XXXXXX_\n*X*_______\nXXXXXXXXG_"
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(Maze), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UploadMaze([FromBody] string maze)
        {
            if (maze is null || !IsValidMaze(maze))
            {
                return BadRequest("The maze format is invalid.");
            }

            var createdMaze = this.mazeService.AddMaze(maze);

            if (createdMaze is null)
            {
                return BadRequest("Maze is invalid. It is not possible to solve.");

            }

            return Created(string.Empty, createdMaze);
        }

        /// <summary>
        /// Get all mazes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Maze>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMazes()
        {
            var mazes = this.mazeService.GetAllMazes();

            return Ok(mazes);
        }

        private bool IsValidMaze(string maze)
        {
            var rows = maze.Split('\n');

            if (rows.Length > 20 || rows.Any(r => r.Length > 20)) return false;

            int startPoints = maze.Count(c => c == 'S');
            int goalPoints = maze.Count(c => c == 'G');

            return startPoints == 1 && goalPoints == 1;
        }
    }
}
