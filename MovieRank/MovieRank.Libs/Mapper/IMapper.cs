using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Contracts;
using MovieRank.Libs.Models;
using System.Collections.Generic;

namespace MovieRank.Libs.Mapper
{
    public interface IMapper
    {
        MovieResponse ToMovieContract(MovieDb response);

        MovieResponse ToMovieContract(Document item);

        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<MovieDb> response);

        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<Document> items);

        MovieDb ToMovieDbModel(int userId, MovieRankRequest movieRankRequest);

        Document ToDocumentModel(int userId, MovieRankRequest movieRankRequest);

        MovieDb ToMovieDbModel(int userId, MovieDb response, MovieRankRequest movieRankRequest);

        Document ToDocumentModel(int userId, Document document, MovieRankRequest movieRankRequest);
    }
}
