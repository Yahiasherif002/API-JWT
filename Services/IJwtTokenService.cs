using API_1.Models;

namespace API_1.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken (ApplicationUser applicationUser);
    }
}
