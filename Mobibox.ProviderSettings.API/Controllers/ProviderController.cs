using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        public ProviderController(IProviderRepository providerRepository, IDynamoDBContext dynamoDBContext)
        {
            _providerRepository = providerRepository;
            _dynamoDBContext = dynamoDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProvider()
        {
            //var providers = await _providerRepository.GetAllProvidersAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> SaveProvider([FromBody] Provider objProvider)
        {
            //var sanitizedModel = SanitizeModel(providerData);

            // Provider objProvider = JsonConvert.DeserializeObject<Provider>(objProvider);
            objProvider.ID = Convert.ToString(Guid.NewGuid());
            objProvider.PK = "ProviderSettings";
            objProvider.SK = "ProviderSettings#" + objProvider.IDProvider + "#" + objProvider.IDService + "#" + objProvider.IDOperator + "#" + objProvider.ID;
            objProvider.RegDate = DateTime.Now;

            // Insert data into DynamoDB
            try
            {
                await _dynamoDBContext.SaveAsync(objProvider);
                return Ok("Record inserted successfully.");
            }
            catch (AmazonDynamoDBException ex)
            {
                return BadRequest($"DynamoDB Exception: {ex.Message}");
            }
            //await _providerRepository.AddProviderAsync(provider);
            return Ok();
        }
        // Utility function to remove empty or null values
        private T SanitizeModel<T>(T model)
        {
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(model);
                if (value == null || (value is string str && string.IsNullOrEmpty(str)))
                {
                    prop.SetValue(model, null); // Set empty values to null or remove them
                }
            }
            return model;
        }

    }


}
