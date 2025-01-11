using LicenseManagerCloud.Models;
using LicenseManagerCloud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LicenseManagerCloud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService license)
        {
            _licenseService = license;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLicense([FromBody] Lincense request)
        {
            if (string.IsNullOrEmpty(request.LicenseKey) || string.IsNullOrEmpty(request.MachineId))
            {
                return BadRequest("LicenseKey and MachineId are required.");
            }

            var existLicenseKey = await _licenseService.GetLicenseByMachineIdAsync(request.MachineId);
            if (existLicenseKey != null)
            {
                return Conflict("A license for this machine already exists.");
            }

            var license = await _licenseService.CreateLicenseAsync(
            request.LicenseKey,
            request.MachineId,
            request.Status ?? "Enable",
            request.ExpiryDate);

            return CreatedAtAction(nameof(GetLicense), new { id = license.Id }, license);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicense(int id)
        {
            var license = await _licenseService.GetLicenseByMachineIdAsync(id.ToString());
            if (license == null)
            {
                return NotFound();
            }

            return Ok(license);
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] Lincense request)
        {
            var token = await _licenseService.GetTokenByLicenseAsync(request.LicenseKey);

            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        //[HttpPost("Activate")]
        //public IActionResult ActivateLicense([FromBody] Lincense request)
        //{
        //    var existingLicense = _licenseService.GetLicense(request.MachineId);
        //    if (existingLicense != null && existingLicense.Status == "Enable")
        //    {
        //        return Ok(new { Token = GenerateToken(request.MachineId, request.ExpiryDate) });
        //    }

        //    var newLicense = _licenseService.CreateLicense(request.MachineId);
        //    return Ok(new { Token = GenerateToken(newLicense.MachineId, newLicense.ExpiryDate) });
        //}

        [HttpGet("Validate")]
        public IActionResult ValidateLicense()
        {
            return Ok(new { Message = "Token is valid" });
        }
    }

}