using Amazon.DynamoDBv2.DataModel;
using Mobibox.ProviderSettings.API.Model;

namespace Mobibox.ProviderSettings.API.Repository
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly IDynamoDBContext _context;

        public async Task<Provider> GetProviderByIdAsync(string providertId)
        {
            return await _context.LoadAsync<Provider>(providertId);

        }

        public async Task<IEnumerable<Provider>> GetAllProvidersAsync()
        {
            var conditions = new List<ScanCondition>();  // No filter conditions for now
            return await _context.ScanAsync<Provider>(conditions).GetRemainingAsync();
        }
        public ProviderRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task AddProviderAsync(Provider provider)
        {
            await _context.SaveAsync(provider);
        }

        //public async Task UpdateProviderAsync(Provider product)
        //{
        //    await _context.SaveAsync(product);  // SaveAsync performs both insert and update
        //}

        //public async Task DeleteProviderAsync(string productId)
        //{
        //    await _context.DeleteAsync<Provider>(productId);
        //}

      
    }
}
