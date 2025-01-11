using LicenseManagerCloud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LicenseManagerCloud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly LicenseService _licenseService;

        private readonly string privatePem = System.IO.File.ReadAllText("private_key.pem");
        private readonly string _issuer = "licensemanagerapiapp";
        private readonly string _audience = "licensemanagerapi";

        public LicenseController(LicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost("Activate")]
        public IActionResult ActivateLicense([FromBody] License request)
        {
            var existingLicense = _licenseService.GetLicense(request.MachineId);
            if (existingLicense != null && existingLicense.IsActive)
            {
                return Ok(new { Token = GenerateToken(request.MachineId, request.ExpiryDate) });
            }

            var newLicense = _licenseService.CreateLicense(request.MachineId);
            return Ok(new { Token = GenerateToken(newLicense.MachineId, newLicense.ExpiryDate) });
        }

        [HttpGet("Validate")]
        public IActionResult ValidateLicense()
        {
            return Ok(new { Message = "Token is valid" });
        }

        private string GenerateToken(string machineId, DateTime expiryDate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var rsa = GetRsaPrivateKey();
            var key = new RsaSecurityKey(rsa);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("MachineId", machineId),
                    new Claim("ExpiryTime", expiryDate.ToString("yyyy-MM-ddTHH:mm:ss"))
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RSA GetRsaPrivateKey()
        {
            RSA rSA = RSA.Create();
            rSA.ImportFromEncryptedPem(privatePem.ToString(), "thang");

            return rSA;
        }
    }

}