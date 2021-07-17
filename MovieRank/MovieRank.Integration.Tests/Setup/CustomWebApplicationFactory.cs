using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MovieRank.Integration.Tests.Setup
{
    public class CustomWebApplicationFactory<TStartUp> : WebApplicationFactory<TStartUp>
        where TStartUp : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => 
                services.AddSingleton<IAmazonDynamoDB>(cc =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
                    return new AmazonDynamoDBClient(clientConfig);
                })
            );
        }
    }
}
