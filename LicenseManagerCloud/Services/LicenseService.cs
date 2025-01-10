namespace LicenseManagerCloud.Services
{
    public class LicenseService
    {
        private readonly List<License> _licenses = new();

        public License? GetLicense(string machineId)
        {
            return _licenses.FirstOrDefault(l => l.MachineId == machineId);
        }

        public License CreateLicense(string machineId)
        {
            var license = new License
            {
                MachineId = machineId,
                IsActive = true
            };
            _licenses.Add(license);
            return license;
        }
    }
}
