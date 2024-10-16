using Mobibox.ProviderSettings.API.Model;

namespace Mobibox.ProviderSettings.API.Repository
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAllProvidersAsync();
        Task<Provider> GetProviderByIdAsync(string providertId);
        Task AddProviderAsync(Provider provider);
        //Task UpdateProviderAsync(Provider provider);
        //Task DeleteProviderAsync(string providerId);
    }
}
