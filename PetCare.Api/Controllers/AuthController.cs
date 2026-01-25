using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.PetOwners.Commands;
using PetCare.Application.Features.PetOwners.Dtos;
using PetCare.Core.Models;
using PetCare.Shared.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, IMediator mediator)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest(new AuthResponse { Success = false, ErrorMessage = "Invalid login details" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? string.Empty;

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new AuthResponse { Success = true, Token = token, Role = userRole });
            }

            return BadRequest(new AuthResponse { Success = false, ErrorMessage = "Invalid login details" });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var command = new CreatePetOwnerCommand
            {
                PetOwner = new PetOwnerCreateModel
                {
                    Email = request.Email,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber
                }
            };

            try
            {
                var petOwnerId = await _mediator.Send(command);

                return Ok(new AuthResponse
                {
                    Success = true,
                    Token = ""
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    ErrorMessage = "Validation failed",
                    ValidationErrors = ex.Errors
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message + " " + string.Join(", ", ex.ValidationErrors ?? new List<string>())
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    ErrorMessage = $"A server error occurred during registration : {ex}."
                });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = _configuration["JwtSettings:Secret"];

            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("No JWT key found in appsettings.json!");
            }

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
