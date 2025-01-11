using LicenseManagerCloud.Models;

namespace LicenseManagerCloud.Services
{
    public interface ILicenseService
    {
        Task<Lincense?> GetLicenseByMachineIdAsync(string machineId);
        Task<Lincense> CreateLicenseAsync(string licenseKey, string machineId, string status, DateTime expiryDate);
        Task<String> GetTokenByLicenseAsync(string licenseKey);
    }
}
