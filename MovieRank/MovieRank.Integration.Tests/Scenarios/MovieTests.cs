using MovieRank.Contracts;
using MovieRank.Integration.Tests.Setup;
using MovieRank.Libs.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.Integration.Tests.Scenarios
{
    [Collection("api")]
    public class MovieTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public MovieTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact]
        public async Task AddMovieRankDataReturnsOkStatus()
        {
            const int userId = 1;

            var response = await AddMovieRankData(userId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllItemsFromDatabaseReturnsNotNullMovieResponse()
        {
            const int userId = 2;

            await AddMovieRankData(userId);

            var response = await client.GetAsync("api/movies");

            MovieResponse[] results;
            using (var content = response.Content.ReadAsStringAsync())
            {
                results = JsonConvert.DeserializeObject<MovieResponse[]>(await content);
            }

            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetMovieReturnsExpectedMovieName()
        {
            const int userId = 3;
            const string movieName = "Test-GetMovieBack";

            await AddMovieRankData(userId, movieName);

            var response = await client.GetAsync($"api/movies/{userId}/{movieName}");

            MovieResponse results;
            using (var content = response.Content.ReadAsStringAsync())
            {
                results = JsonConvert.DeserializeObject<MovieResponse>(await content);
            }

            Assert.Equal(movieName, results.MovieName);
        }

        [Fact]
        public async Task UpdateMovieReturnsUpdatedMovieRankValue()
        {
            const int userId = 4;
            const string movieName = "Test-UpdateMovie";
            const int ranking = 6;

            var response = await AddMovieRankData(userId, movieName);

            var updateRequest = new MovieUpdateRequest
            {
                MovieName = movieName,
                Ranking = ranking
            };

            var json = JsonConvert.SerializeObject(updateRequest);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var updateResponse = await client.PatchAsync($"api/movies/{userId}", stringContent);
            
            var getResponse = await client.GetAsync($"api/movies/{userId}/{movieName}");

            MovieResponse result;
            using (var content = getResponse.Content.ReadAsStringAsync())
            {
                result = JsonConvert.DeserializeObject<MovieResponse>(await content);
            }

            Assert.Equal(ranking, result.Ranking);
        }

        [Fact]
        public async Task GetMovieReturnsReturnsOverallMovieRanking()
        {
            const int userId = 5;
            const string movieName = "Test-GetMovieOverallMovieRanking";

            await AddMovieRankData(userId, movieName);

            var response = await client.GetAsync($"api/movies/{movieName}/ranking");

            MovieResponse results;
            using (var content = response.Content.ReadAsStringAsync())
            {
                results = JsonConvert.DeserializeObject<MovieResponse>(await content);
            }

            Assert.NotNull(results);
        }

        private async Task<HttpResponseMessage> AddMovieRankData(int userId, string movieName = "test-MovieName")
        {
            var movieRankRequest = new MovieRankRequest
            {
                MovieName = movieName,
                Description = "test-Description",
                Actors = new List<string>
                {
                    "Actor 1", "Actor 2"
                },
                Ranking = 4
            };

            var json = JsonConvert.SerializeObject(movieRankRequest);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await client.PostAsync($"api/movies/{userId}", stringContent);
        }
    }
}
