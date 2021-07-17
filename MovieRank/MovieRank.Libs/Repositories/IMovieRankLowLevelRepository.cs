using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using System.Threading.Tasks;

namespace MovieRank.Libs.Repositories
{
    public interface IMovieRankLowLevelRepository
    {
        Task<ScanResponse> GetAllItems();

        Task<GetItemResponse> GetMovie(int userId, string movieName);

        Task<QueryResponse> GetUsersRankedMoviesByMovieTitle(int userId, string movieName);

        Task AddMovie(int userId, MovieRankRequest movieRankRequest);

        Task UpdateMovie(int userId, MovieUpdateRequest movieRankRequest);

        Task<QueryResponse> GetMovieRank(string movieName);

        Task CreateDynamoDbTable(string dynamoDbTablName);
        Task DeleteDynamoDbTable(string dynamoDbTablName);
    }
}
