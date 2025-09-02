using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InnovaGraphics.Models;
using Microsoft.IdentityModel.Tokens;

namespace InnovaGraphics.Utils.Facade
{
    public class TokenGenerationSubsystem
    {
        private readonly IConfiguration _configuration;

        public TokenGenerationSubsystem(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string accessToken, string refreshToken, DateTime accessExpiry, DateTime refreshExpiry) GenerateTokens(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var key = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryMinutes = jwtSettings.GetValue<int>("ExpiryMinutes");
            var expiryDays = jwtSettings.GetValue<int>("RefreshExpiryDays");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var accessExpiry = accessTokenDescriptor.Expires.Value;

            var refreshToken = GenerateRefreshToken();
            var refreshExpiry = DateTime.UtcNow.AddDays(expiryDays);

            return (tokenHandler.WriteToken(accessToken), refreshToken, accessExpiry, refreshExpiry);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}