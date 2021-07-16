using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using MovieRank.Libs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieRank.Libs.Repositories
{
    public class MovieRankDocumentRepository : IMovieRankRepository<Document>
    {
        private const string TableName = "MovieRank";
        private readonly Table table;

        public MovieRankDocumentRepository(IAmazonDynamoDB dynamoDbClient)
        {
            table = Table.LoadTable(dynamoDbClient, TableName);
        }

        public async Task AddMovie(Document movieDb)
        {
            await table.PutItemAsync(movieDb);
        }

        public async Task<IEnumerable<Document>> GetAllItems()
        {
            return await table.Scan(new ScanOperationConfig()).GetRemainingAsync();
        }

        public async Task<Document> GetMovie(int userId, string movieName)
        {
            return await table.GetItemAsync(userId, movieName);
        }

        public async Task<IEnumerable<Document>> GetMovieRank(string movieName)
        {
            var filter = new QueryFilter("MovieName", QueryOperator.Equal, movieName);
            var config = new QueryOperationConfig()
            {
                IndexName = "MovieName-index",
                Filter = filter
            };

            return await table.Query(config).GetRemainingAsync();
        }

        public async Task<IEnumerable<Document>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var filter = new QueryFilter("UserId", QueryOperator.Equal, userId);
            filter.AddCondition("MovieName", QueryOperator.BeginsWith, movieName);

            return await table.Query(filter).GetRemainingAsync();
        }

        public async Task UpdateMovie(Document movieDb)
        {
            await table.UpdateItemAsync(movieDb);
        }
    }
}
