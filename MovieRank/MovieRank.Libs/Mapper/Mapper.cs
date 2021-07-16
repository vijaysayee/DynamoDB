using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Contracts;
using MovieRank.Libs.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieRank.Libs.Mapper
{
    public class Mapper : IMapper
    {
        public IEnumerable<MovieResponse> ToMovieContract(IEnumerable<MovieDb> items)
        {
            return items.Select(ToMovieContract);
        }

        public IEnumerable<MovieResponse> ToMovieContract(IEnumerable<Document> items)
        {
            return items.Select(ToMovieContract);
        }

        public MovieResponse ToMovieContract(MovieDb item)
        {
            return new MovieResponse
            {
                MovieName = item.MovieName,
                Description = item.Description,
                Actors = item.Actors,
                Ranking = item.Ranking,
                TimeRanked = item.RankedDateTime
            };
        }

        public MovieResponse ToMovieContract(Document item)
        {
            return new MovieResponse
            {
                MovieName = item["MovieName"],
                Description = item["Description"],
                Actors = item["Actors"].AsListOfString(),
                Ranking = Convert.ToInt32(item["Ranking"]),
                TimeRanked = item["RankedDateTime"]
            };
        }

        public MovieDb ToMovieDbModel(int userId, MovieRankRequest movieRankRequest)
        {
            return new MovieDb
            {
                UserId = userId,
                MovieName = movieRankRequest.MovieName,
                Description = movieRankRequest.Description,
                Actors = movieRankRequest.Actors,
                Ranking = movieRankRequest.Ranking,
                RankedDateTime = DateTime.UtcNow.ToString()
            };
        }

        public Document ToDocumentModel(int userId, MovieRankRequest movieRankRequest)
        {
            return new Document
            {
                ["UserId"] = userId,
                ["MovieName"] = movieRankRequest.MovieName,
                ["Description"] = movieRankRequest.Description,
                ["Actors"] = movieRankRequest.Actors,
                ["Ranking"] = movieRankRequest.Ranking,
                ["RankedDateTime"] = DateTime.UtcNow.ToString()
            };
        }

        public MovieDb ToMovieDbModel(int userId, MovieDb movieDb, MovieRankRequest movieRankRequest)
        {
            return new MovieDb
            {
                UserId = userId,
                MovieName = movieDb.MovieName,
                Description = movieDb.Description,
                Actors = movieDb.Actors,
                Ranking = movieRankRequest.Ranking,
                RankedDateTime = DateTime.UtcNow.ToString()
            };
        }

        public Document ToDocumentModel(int userId, Document document, MovieRankRequest movieRankRequest)
        {
            return new Document
            {
                ["UserId"] = userId,
                ["MovieName"] = document["MovieName"],
                ["Description"] = document["Description"],
                ["Actors"] = document["Actors"],
                ["Ranking"] = movieRankRequest.Ranking,
                ["RankedDateTime"] = DateTime.UtcNow.ToString()
            };
        }
    }
}
