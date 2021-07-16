using MovieRank.Libs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Services
{
    public class SetupService : ISetupService
    {
        private readonly IMovieRankLowLevelRepository movieRankRepository;

        public SetupService(IMovieRankLowLevelRepository movieRankRepository)
        {
            this.movieRankRepository = movieRankRepository;
        }

        public async Task CreateDynamoDbTable(string dynamoDbTablName)
        {
            await movieRankRepository.CreateDynamoDbTable(dynamoDbTablName);
        }

        public async Task DeleteDynamoDbTable(string dynamoDbTablName)
        {
            await movieRankRepository.DeleteDynamoDbTable(dynamoDbTablName);
        }
    }
}
