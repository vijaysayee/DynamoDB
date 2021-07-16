using MovieRank.Contracts;
using MovieRank.Libs.Models;
using System.Collections.Generic;

namespace MovieRank.Libs.Mapper
{
    public interface IMapper
    {
        MovieResponse ToMovieContract(MovieDb response);

        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<MovieDb> response);

        MovieDb ToMovieDbModel(int userId, MovieRankRequest movieRankRequest);

        MovieDb ToMovieDbModel(int userId, MovieDb response, MovieRankRequest movieRankRequest);
    }
}
