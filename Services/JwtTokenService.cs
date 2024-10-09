using API_1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_1.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public JwtTokenService(IConfiguration configuration,UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser applicationUser)
        {


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,applicationUser.Id),
                new Claim(ClaimTypes.Name,applicationUser.UserName),
                new Claim(ClaimTypes.Email,applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var userRoles = await userManager.GetRolesAsync(applicationUser);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var TokenExpiration = DateTime.UtcNow.AddHours(2);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecritKey"]));

            var Credentials = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);



            var token = new JwtSecurityToken
            (
                issuer: configuration["JWT:IssuerIP"],
                audience: configuration["JWT:AudienceIP"],
                claims: claims,
                expires: TokenExpiration,
                signingCredentials: Credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            






        }
    }
}
