using API_1.DTO;
using Microsoft.AspNetCore.Identity;

namespace API_1.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(CreateAccountDTO createAccount);
        Task<AuthResponse> LoginAsync(LoginDTO login);
    }
}
