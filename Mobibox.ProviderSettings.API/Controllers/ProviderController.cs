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

                    //await _dynamoDBContext.SaveAsync(objProvider);
                    return Ok("Record inserted successfully.");
                }
                else
                {
                    //update
                    objProvider.UpdateDate = DateTime.Now;
                   // await _dynamoDBContext.SaveAsync(objProvider);
                    return Ok("Record updated successfully.");
                }
            }
            catch (AmazonDynamoDBException ex)
            {
                return BadRequest($"DynamoDB Exception: {ex.Message}");
            }         
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProvider()
        {
            //var providers = await _providerRepository.GetAllProvidersAsync();
            return Ok();
        }
    }
}
