using MovieRank.Contracts;
using MovieRank.Libs.Mapper;
using MovieRank.Libs.Models;
using MovieRank.Libs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Services
{
    public class MovieRankService : IMovieRankService
    {
        //private readonly IMovieRankRepository<MovieDb> movieRankRepository;
        private readonly IMovieRankLowLevelRepository movieRankRepository;
        private readonly IMapper mapper;

        //public MovieRankService(IMovieRankRepository<MovieDb> movieRankRepository, IMapper mapper)
        public MovieRankService(IMovieRankLowLevelRepository movieRankRepository, IMapper mapper)
        {
            this.movieRankRepository = movieRankRepository;
            this.mapper = mapper;
        }

        public async Task AddMovie(int userId, MovieRankRequest movieRankRequest)
        {
            await movieRankRepository.AddMovie(userId, movieRankRequest);
        }

        public async Task<IEnumerable<MovieResponse>> GetAllItemsFromDatabase()
        {
            var response = await movieRankRepository.GetAllItems();

            return mapper.ToMovieContract(response);
        }

        public async Task<MovieResponse> GetMovie(int userId, string movieName)
        {
            var response = await movieRankRepository.GetMovie(userId, movieName);

            return mapper.ToMovieContract(response);
        }

        public async Task<MovieRankResponse> GetMovieRank(string movieName)
        {
            var response = await movieRankRepository.GetMovieRank(movieName);

            var overllMovieRanking = Math.Round(response.Items.Select(x => Convert.ToInt32(x["Ranking"].N)).Average());

            return new MovieRankResponse { MovieName = movieName, OverallRanking = overllMovieRanking };
        }

        public async Task<IEnumerable<MovieResponse>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var response = await movieRankRepository.GetUsersRankedMoviesByMovieTitle(userId, movieName);

            return mapper.ToMovieContract(response);
        }

        public async Task UpdateMovie(int userId, MovieRankRequest movieRankRequest)
        {
            await movieRankRepository.UpdateMovie(userId, movieRankRequest);
        }
    }
}
