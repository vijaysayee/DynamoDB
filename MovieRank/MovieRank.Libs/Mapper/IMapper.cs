using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using MovieRank.Libs.Models;
using System.Collections.Generic;

namespace MovieRank.Libs.Mapper
{
    public interface IMapper
    {
        MovieResponse ToMovieContract(MovieDb response);

        MovieResponse ToMovieContract(Document item);

        MovieResponse ToMovieContract(Dictionary<string, AttributeValue> item);

        MovieResponse ToMovieContract(GetItemResponse response);

        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<MovieDb> response);

        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<Document> items);

        IEnumerable<MovieResponse> ToMovieContract(ScanResponse response);

        IEnumerable<MovieResponse> ToMovieContract(QueryResponse response);

        MovieDb ToMovieDbModel(int userId, MovieRankRequest movieRankRequest);

        Document ToDocumentModel(int userId, MovieRankRequest movieRankRequest);

        MovieDb ToMovieDbModel(int userId, MovieDb response, MovieRankRequest movieRankRequest);

        Document ToDocumentModel(int userId, Document document, MovieRankRequest movieRankRequest);
    }
}
