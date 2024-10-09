using API_1.DTO;
using API_1.Models;
using API_1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;

namespace API_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService account;

        public AccountController(IAccountService account)
        {
            this.account = account;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateAccountDTO model)
        {
            var result = await account.CreateUserAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                // Attempt to login the user using the account service
                var authResponse = await account.LoginAsync(model);

                // If the response is not null, return it with a 200 OK status
                if (authResponse != null)
                {
                    return Ok(authResponse);
                }

                // If the login fails for some reason, return unauthorized
                return Unauthorized(new { message = "Invalid username or password." });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle case where the login was unsuccessful due to invalid credentials
                return Unauthorized(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                // Handle case where the user is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) for debugging
                // You might use a logging framework like Serilog, NLog, etc.
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
