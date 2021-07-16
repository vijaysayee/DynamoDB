using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRank.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ISetupService setupService;

        public SetupController(ISetupService setupService)
        {
            this.setupService = setupService;
        }

        [HttpPost("createTable/{dynamoDbTablName}")]
        public async Task<IActionResult> CreateDynamoDbTable(string dynamoDbTablName)
        {
            await setupService.CreateDynamoDbTable(dynamoDbTablName);

            return Ok();
        }

        [HttpDelete("deleteTable/{dynamoDbTablName}")]
        public async Task<IActionResult> DeleteDynamoDbTable(string dynamoDbTablName)
        {
            await setupService.DeleteDynamoDbTable(dynamoDbTablName);

            return Ok();
        }
    }
}
