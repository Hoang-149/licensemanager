namespace LicenseManagerCloud
{
    public class License
    {
        public int Id { get; set; }
        public string LicenseId { get; set; }
        public string MachineId { get; set; }
        public string Status { get; set; } = "Enable";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
    }
}
