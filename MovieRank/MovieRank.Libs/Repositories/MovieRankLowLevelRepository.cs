using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using MovieRank.Libs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieRank.Libs.Repositories
{
    public class MovieRankLowLevelRepository : IMovieRankLowLevelRepository
    {
        private const string TableName = "MovieRank";
        private readonly IAmazonDynamoDB dynamoDbClient;

        public MovieRankLowLevelRepository(IAmazonDynamoDB dynamoDbClient)
        {
            this.dynamoDbClient = dynamoDbClient;
        }

        public async Task<ScanResponse> GetAllItems()
        {
            var scanRequest = new ScanRequest(TableName);

            return await dynamoDbClient.ScanAsync(scanRequest);
        }

        public async Task<GetItemResponse> GetMovie(int userId, string movieName)
        {
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue { N = userId.ToString() } },
                    { "MovieName", new AttributeValue { S = movieName } }
                }
            };

            return await dynamoDbClient.GetItemAsync(request);
        }

        public async Task<QueryResponse> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                KeyConditionExpression = "UserId = :userId and begins_with (MovieName, :movieName)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":userId", new AttributeValue{ N = userId.ToString() } },
                    { ":movieName", new AttributeValue{ S = movieName } }
                }
            };

            return await dynamoDbClient.QueryAsync(queryRequest);
        }

        public async Task AddMovie(int userId, MovieRankRequest movieRankRequest)
        {
            var putRequest = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue { N = userId.ToString() } },
                    { "MovieName", new AttributeValue { S = movieRankRequest.MovieName } },
                    { "Description", new AttributeValue { S = movieRankRequest.Description } },
                    { "Actors", new AttributeValue { SS = movieRankRequest.Actors } },
                    { "Ranking", new AttributeValue { N = movieRankRequest.Ranking.ToString() } },
                    { "RankedDateTime", new AttributeValue { S = DateTime.UtcNow.ToString() } }
                }
            };

            await dynamoDbClient.PutItemAsync(putRequest);
        }

        public async Task UpdateMovie(int userId, MovieUpdateRequest movieRankRequest)
        {
            var updateRequest = new UpdateItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue { N = userId.ToString() } },
                    { "MovieName", new AttributeValue { S = movieRankRequest.MovieName } },
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    { "Ranking", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { N = movieRankRequest.Ranking.ToString() } } },
                }
            };

            await dynamoDbClient.UpdateItemAsync(updateRequest);
        }

        public async Task<QueryResponse> GetMovieRank(string movieName)
        {
            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                IndexName = "MovieName-index",
                KeyConditionExpression = "MovieName = :movieName",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":movieName", new AttributeValue{ S = movieName } }
                }
            };

            return await dynamoDbClient.QueryAsync(queryRequest);
        }

        public async Task CreateDynamoDbTable(string dynamoDbTablName)
        {
            var createRequest = new CreateTableRequest
            {
                TableName = dynamoDbTablName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement { AttributeName = "Id", KeyType = "HASH" }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };

            await dynamoDbClient.CreateTableAsync(createRequest);
        }

        public async Task DeleteDynamoDbTable(string dynamoDbTablName)
        {
            var deleteRequest = new DeleteTableRequest
            {
                TableName = dynamoDbTablName,
            };

            await dynamoDbClient.DeleteTableAsync(deleteRequest);
        }
    }
}
