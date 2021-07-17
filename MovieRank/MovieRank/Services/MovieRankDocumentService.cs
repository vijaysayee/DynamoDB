using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Contracts;
using MovieRank.Libs.Mapper;
using MovieRank.Libs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Services
{
    public class MovieRankDocumentService : IMovieRankService
    {
        private readonly IMovieRankRepository<Document> movieRankRepository;
        private readonly IMapper mapper;

        public MovieRankDocumentService(IMovieRankRepository<Document> movieRankRepository, IMapper mapper)
        {
            this.movieRankRepository = movieRankRepository;
            this.mapper = mapper;
        }

        public async Task AddMovie(int userId, MovieRankRequest movieRankRequest)
        {
            var document = mapper.ToDocumentModel(userId, movieRankRequest);
            await movieRankRepository.AddMovie(document);
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

            var overllMovieRanking = Math.Round(response.Select(x => x["Ranking"].AsInt()).Average());

            return new MovieRankResponse { MovieName = movieName, OverallRanking = overllMovieRanking };
        }

        public async Task<IEnumerable<MovieResponse>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var response = await movieRankRepository.GetUsersRankedMoviesByMovieTitle(userId, movieName);

            return mapper.ToMovieContract(response);
        }

        public async Task UpdateMovie(int userId, MovieUpdateRequest movieRankRequest)
        {
            var currentDocument = await movieRankRepository.GetMovie(userId, movieRankRequest.MovieName);

            var document = mapper.ToDocumentModel(userId, currentDocument, movieRankRequest);

            await movieRankRepository.UpdateMovie(document);
        }
    }
}
