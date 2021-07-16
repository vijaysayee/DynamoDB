using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using MovieRank.Libs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRank.Libs.Repositories
{
    public class MovieRankRepository : IMovieRankRepository
    {
        private readonly DynamoDBContext dbContext;

        public MovieRankRepository(IAmazonDynamoDB dynamoDbClient)
        {
            this.dbContext = new DynamoDBContext(dynamoDbClient);
        }

        public async Task AddMovie(MovieDb movieDb)
        {
            await dbContext.SaveAsync(movieDb);
        }

        public async Task UpdateMovie(MovieDb movieDb)
        {
            await dbContext.SaveAsync(movieDb);
        }

        public async Task<IEnumerable<MovieDb>> GetAllItems()
        {
            return await dbContext.ScanAsync<MovieDb>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<MovieDb> GetMovie(int userId, string movieName)
        {
            return await dbContext.LoadAsync<MovieDb>(userId, movieName);
        }

        public async Task<IEnumerable<MovieDb>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var config = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                { 
                    new ScanCondition("MovieName", Amazon.DynamoDBv2.DocumentModel.ScanOperator.BeginsWith, movieName)
                }
            };

            return await dbContext.QueryAsync<MovieDb>(userId, config).GetRemainingAsync();
        }

        public async Task<IEnumerable<MovieDb>> GetMovieRank(string movieName)
        {
            var config = new DynamoDBOperationConfig
            {
                IndexName = "MovieName-index"
            };

            return await dbContext.QueryAsync<MovieDb>(movieName, config).GetRemainingAsync();
        }
    }
}
