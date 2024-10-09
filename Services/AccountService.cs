using API_1.DTO;
using API_1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using OpenQA.Selenium;
using System.Xml.Serialization;

namespace API_1.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IJwtTokenService jwt;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , IJwtTokenService jwt)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwt = jwt;
        }


        public async Task<IdentityResult> CreateUserAsync(CreateAccountDTO model)
        {
            var excistingUser = await userManager.FindByEmailAsync(model.Email);
            if (excistingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "EmailAlreadyInUse", Description = "Email Already In Use ." });
            }
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

            return await userManager.CreateAsync(user, model.Password);


        }

        public async Task<AuthResponse> LoginAsync(LoginDTO model)
        {
            var userExist = await userManager.FindByNameAsync(model.Username);

            if (userExist != null)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

              if (result.Succeeded)
              {
                    var token = await jwt.GenerateToken(userExist);

                    return new AuthResponse
                    {
                        Token = token,
                        Expiration = DateTime.UtcNow.AddHours(2)
                    };

              }
                else
                {
                    // Handle case where sign in failed due to invalid credentials
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

            }
            else
            {
                // Handle case where user does not exist
                throw new NotFoundException("User not found.");
            }



        }
    }
}
