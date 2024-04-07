using Infrastructure;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Services.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserLogin : IUserLogin
    {
        private readonly AppDbContext _context;
        public IConfiguration Configuration { get; }

        public UserLogin(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetToken(LoginModel loginModel)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == loginModel.Username && u.Password == loginModel.Password);

            if (user == null)
            {
                return string.Empty;
            }

            // Generate JWT token
            var token = GenerateToken(user);
            return token;
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.Email), // Include user email as a claim
                                new Claim("userId", user.UserID.ToString()) // Include userId as a claim
                            }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetLoggedUserDetails(Guid userId)
        {
            var userDetails = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            return userDetails;
        }
    }
}
