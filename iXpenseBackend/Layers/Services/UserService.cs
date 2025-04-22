using iXpenseBackend.Layers.Repositories;
using iXpenseBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iXpenseBackend.Layers.Services
{
    public class UserService
    {
        private readonly UserRepo _userRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;


        public UserService(UserRepo userRepo, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //Register a new user
        public async Task<(bool IsSuccess, string Message, string? UserId)> RegisterUserAsync(RegisterDto registerDto)
        {
            var userExists = await _userRepo.FindUserByUsernameAsync(registerDto.Username);
            if(userExists != null)
            {
                return (false, "User already exists", null);
            }

            var user = new IdentityUser
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username,
            };

            var result = await _userRepo.CreateUserAsync(user, registerDto.Password);
            if(result.Succeeded)
            {
                return (true, "User created successfully", user.Id);
            }
            else
            {
                return (false, "Something went wrong, make sure you put in the correct details", null);
            }

        }

        //Login a user
        public async Task <(bool IsSuccess, string Message, string? Token)> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if(user == null)
            {
                return (false, "User not found", null);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return (false, "Invalid login credentials", null);
            }

            var token = GenerateJwtToken(user);
            return (true, "Login successful", token);
        }

        //JWT
        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
