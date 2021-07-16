using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Services
{
    public interface ISetupService
    {
        Task CreateDynamoDbTable(string dynamoDbTablName);
        Task DeleteDynamoDbTable(string dynamoDbTablName);
    }
}
