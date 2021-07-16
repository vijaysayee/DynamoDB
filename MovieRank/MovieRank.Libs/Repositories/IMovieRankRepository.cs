using MovieRank.Libs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRank.Libs.Repositories
{
    public interface IMovieRankRepository<T>
    {
        Task<IEnumerable<T>> GetAllItems();
        Task<T> GetMovie(int userId, string movieName);
        Task<IEnumerable<T>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName);
        Task AddMovie(T movieDb);
        Task UpdateMovie(T movieDb);
        Task<IEnumerable<T>> GetMovieRank(string movieName);
    }
}
