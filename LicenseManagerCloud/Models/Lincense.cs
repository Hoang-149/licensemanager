namespace LicenseManagerCloud.Models
{
    public class Lincense
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; }
        public string MachineId { get; set; }
        public string Status { get; set; } = "Enale";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
    }
}
