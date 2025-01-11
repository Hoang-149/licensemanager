using LicenseManagerCloud.Data;
using LicenseManagerCloud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LicenseManagerCloud.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ApplicationDbContext _context;

        private readonly string privatePem = System.IO.File.ReadAllText("private_key.pem");
        private readonly string _issuer = "licensemanagerapiapp";
        private readonly string _audience = "licensemanagerapi";

        public LicenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Lincense> CreateLicenseAsync(string licensekey, string machineId, string status, DateTime expicyDate)
        {
            DateTime ExpicydateTime = DateTime.SpecifyKind(expicyDate, DateTimeKind.Utc);

            var license = new Lincense
            {
                Id = 0,
                LicenseKey = licensekey,
                MachineId = machineId,
                Status = status,
                //CreatedAt = CreateAtDateTime,
                ExpiryDate = ExpicydateTime
            };
            _context.Add(license);
            await _context.SaveChangesAsync();

            return license;
        }

        public async Task<Lincense> GetLicenseByMachineIdAsync(string machineId)
        {
            return await _context.Set<Lincense>().FirstOrDefaultAsync(l => l.MachineId == machineId);
        }

        public async Task<string> GetTokenByLicenseAsync(string licensekey)
        {
            var license = await _context.Set<Lincense>().FirstOrDefaultAsync(l => l.LicenseKey == licensekey);
            if (license == null || license.Status != "Enable")
            {
                return null;
            }
            var token = GenerateToken(license.MachineId, license.ExpiryDate);
            if (token != null)
            {
                return token;
            }
            return null;
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
