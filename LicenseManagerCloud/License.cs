namespace LicenseManagerCloud
{
    public class License
    {
        public Guid LicenseId { get; set; }
        public string MachineId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddDays(30);
    }
}
