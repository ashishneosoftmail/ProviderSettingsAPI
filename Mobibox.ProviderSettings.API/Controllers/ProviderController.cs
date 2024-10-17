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
using System.Reflection;


namespace Mobibox.ProviderSettings.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IDynamoDBContext _dynamoDBContext;
        public readonly IConfiguration _configuration;
        public static string AccessKey;
        public static string SecretKey;

        public ProviderController(IProviderRepository providerRepository, IDynamoDBContext dynamoDBContext, IConfiguration configuration)
        {
            _providerRepository = providerRepository;
            _dynamoDBContext = dynamoDBContext;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> SaveProvider([FromBody] Provider objProvider)
        {
            // Provider objProvider = JsonConvert.DeserializeObject<Provider>(objProvider);            
            try
            {
                // Insert data into DynamoDB
                if (objProvider.ID == null)
                {
                    objProvider.ID = Convert.ToString(Guid.NewGuid());
                    objProvider.PK = "ProviderSettings";
                    objProvider.SK = "ProviderSettings#" + objProvider.IDProvider + "#" + objProvider.IDService + "#" + objProvider.IDOperator + "#" + objProvider.ID;
                    objProvider.RegDate = DateTime.Now;
                    objProvider.UpdateDate = objProvider.RegDate;

                    await _dynamoDBContext.SaveAsync(objProvider);
                    return Ok("Record inserted successfully.");
                }
                else
                {
                    //update
                    objProvider.UpdateDate = DateTime.Now;
                    // await _dynamoDBContext.SaveAsync(objProvider);
                    return Ok("Record inserted successfully.");
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
        [HttpGet("{id}")]
        public async Task<object> GetProviderDetailsById(int id)
        {
            AccessKey = _configuration.GetSection("AWS:AccessKey").Value;
            SecretKey = _configuration.GetSection("AWS:SecretKey").Value;
            try
            {

                var client = new AmazonDynamoDBClient(AccessKey, SecretKey, RegionEndpoint.EUCentral1);
                DynamoDBContext context = new DynamoDBContext(client);
                string pk = "ProviderSettings";
                string sk = "ProviderSettings#" + id + "#";
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
                    RegDate = provider.RegDate,
                    Status = provider.Status

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
                    RegDate = provider.RegDate,
                    Status = provider.Status

                }).ToList();

                return Ok(providerDtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
