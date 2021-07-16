using Microsoft.AspNetCore.Mvc;
using MovieRank.Contracts;
using MovieRank.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRankService movieRankService;

        public MoviesController(IMovieRankService movieRankService)
        {
            this.movieRankService = movieRankService;
        }

        [HttpGet]
        public async Task<IEnumerable<MovieResponse>> GetAllItemsFromDatabase()
        {
            var result = await movieRankService.GetAllItemsFromDatabase();

            return result;
        }

        [HttpGet("{userId:int}/{movieName}")]
        public async Task<MovieResponse> GetMovie(int userId, string movieName)
        {
            var result = await movieRankService.GetMovie(userId, movieName);

            return result;
        }

        [HttpGet("{userId:int}/RankedMovies/{movieName}")]
        public async Task<IEnumerable<MovieResponse>> GetMoviesByUserRank(int userId, string movieName)
        {
            var result = await movieRankService.GetUsersRankedMoviesByMovieTitle(userId, movieName);

            return result;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddMovie(int userId, [FromBody] MovieRankRequest movieRankRequest)
        {
            await movieRankService.AddMovie(userId, movieRankRequest);

            return Ok();
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateMovie(int userId, [FromBody] MovieRankRequest movieRankRequest)
        {
            await movieRankService.UpdateMovie(userId, movieRankRequest);

            return Ok();
        }


        [HttpGet("{movieName}/ranking")]
        public async Task<MovieRankResponse> GetMoviesByUserRank(string movieName)
        {
            return await movieRankService.GetMovieRank(movieName);
        }
    }
}
