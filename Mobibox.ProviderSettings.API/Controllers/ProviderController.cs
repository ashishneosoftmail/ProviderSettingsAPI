using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mobibox.ProviderSettings.API.Model;
using Mobibox.ProviderSettings.API.Repository;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;


namespace Mobibox.ProviderSettings.API.Controllers
{
    [ApiController]
    [Route("api/provider-setting")]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IDynamoDBContext _dynamoDBContext;
        public readonly IConfiguration _configuration;
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public static string AccessKey;
        public static string SecretKey;


        public ProviderController(IProviderRepository providerRepository, IDynamoDBContext dynamoDBContext, IConfiguration configuration, IAmazonDynamoDB dynamoDbClient)
        {
            _providerRepository = providerRepository;
            _dynamoDBContext = dynamoDBContext;
            _configuration = configuration;
            _dynamoDbClient = dynamoDbClient;
        }

        [HttpPost]
        public async Task<ActionResult> SaveProvider([FromBody] Provider objProvider)

        {
            // Provider objProvider = JsonConvert.DeserializeObject<Provider>(objProvider);            
            try
            {
                // Insert data into DynamoDB
                if (string.IsNullOrEmpty(objProvider.ID))
                {
                    objProvider.ID = Convert.ToString(Guid.NewGuid());
                    objProvider.PK = "ProviderSettings";
                    objProvider.SK = "ProviderSettings#" + objProvider.IDProvider + "#" + objProvider.IDService + "#" + objProvider.IDOperator + "#" + objProvider.ID;
                    objProvider.RegDate = DateTime.Now;
                    objProvider.UpdateDate = objProvider.RegDate;

                    await _dynamoDBContext.SaveAsync(objProvider);
                    return Ok(objProvider);
                }
                else
                {
                    //update
                    objProvider.UpdateDate = DateTime.Now;
                   await _dynamoDBContext.SaveAsync(objProvider);
                    return Ok(objProvider);
                }
            }
            catch (AmazonDynamoDBException ex)
            {
                return BadRequest($"DynamoDB Exception: {ex.Message}");
            }
        }


        /// <summary>
        /// This method is used to fetch the data by provider id from Dynamo database
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{providerId}")]
        public async Task<object> GetProviderDetailsById(int providerId)
        {
            AccessKey = _configuration.GetSection("AWS:AccessKey").Value;
            SecretKey = _configuration.GetSection("AWS:SecretKey").Value;
            try
            {

                var client = new AmazonDynamoDBClient(AccessKey, SecretKey, RegionEndpoint.EUCentral1);
                DynamoDBContext context = new DynamoDBContext(client);
                string pk = "ProviderSettings";
                string sk = "ProviderSettings#" + providerId + "#";
                var values = new List<object> { sk };
                var objProvider = await context.QueryAsync<Provider>(pk, QueryOperator.BeginsWith, values).GetRemainingAsync();

                // Map to ProviderDto
                var providerDtoList = objProvider.Select(provider => new Provider
                {
                    ID = provider.ID,
                    PK = provider.PK,
                    SK = provider.SK,
                    IDClient = provider.IDClient,
                    IDService = provider.IDService,
                    IDCountry = provider.IDCountry,
                    IDOperator = provider.IDOperator,
                    IDProvider = provider.IDProvider,
                    RegDate = provider.RegDate,
                    Status = provider.Status,
                    ResponseSettings = provider.ResponseSettings,
                    RequestSettings = provider.RequestSettings

                }).ToList();

                return Ok(providerDtoList);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        /// <summary>
        ///  This method is get the data by Provider Id
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<object> GetProviderDetailsBySKId(string skId)
        {
            skId = skId.Replace("%23", "#");
            AccessKey = _configuration.GetSection("AWS:AccessKey").Value;
            SecretKey = _configuration.GetSection("AWS:SecretKey").Value;
            try
            {

                var client = new AmazonDynamoDBClient(AccessKey, SecretKey, RegionEndpoint.EUCentral1);
                DynamoDBContext context = new DynamoDBContext(client);
                string pk = "ProviderSettings";
                string sk = skId;
                var values = new List<object> { sk };
                var objProvider = await context.QueryAsync<Provider>(pk, QueryOperator.BeginsWith, values).GetRemainingAsync();

                // Map to ProviderDto
                var providerDtoList = objProvider.Select(provider => new Provider
                {

                    ID = provider.ID,
                    PK = provider.PK,
                    SK = provider.SK,
                    IDClient = provider.IDClient,
                    IDService = provider.IDService,
                    IDCountry = provider.IDCountry,
                    IDOperator = provider.IDOperator,
                    IDProvider = provider.IDProvider,
                    RegDate = provider.RegDate,
                    Status = provider.Status,
                    ResponseSettings = provider.ResponseSettings,
                    RequestSettings = provider.RequestSettings

                }).ToList();

                return Ok(providerDtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProviderBySkId(string skId)
        {
            skId = skId.Replace("%23", "#");
            AccessKey = _configuration.GetSection("AWS:AccessKey").Value;
            SecretKey = _configuration.GetSection("AWS:SecretKey").Value;

            try
            {
                var request = new DeleteItemRequest
                {
                    TableName = "MobiboxConfigurations",  // Replace with your table name
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "SK", new AttributeValue { S = skId } },
                        { "PK", new AttributeValue { S = "ProviderSettings" } }
                    }
                };

               await _dynamoDbClient.DeleteItemAsync(request);

                return Ok(new { message = $"Record with skid {skId}" });
            }
            catch (AmazonDynamoDBException ex)
            {
                return StatusCode(500, new { message = "Error deleting record.", error = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }





    }


}
