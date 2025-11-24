using Contacts.Interfaces;
using Contacts.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Contacts.Services
{
    public class AuthService : IAuthService
    {
        private readonly ContactContext _dbContext;
        private readonly IConfiguration _config;

        public AuthService(ContactContext contactContext, IConfiguration config)
        {
            _dbContext = contactContext;
            _config = config;
        }

        public Task<string> Login(LoginModel model)
        {
            var user = _dbContext.Users.Where(u => u.Username == model.Username).SingleOrDefault();

            //temporary
            if (model.Password.Equals(user.PwdHash))
            {
                return GenerateToken(model.Username);
            }

            return Task.FromResult(string.Empty);
        }

        public Task<bool> Logout(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(RegisterModel contact)
        {
            Contact newContact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = contact.FirstName,
                Surname = contact.Surname,
                Email = contact.Email,
                Category = _dbContext.Categories.Where(c => c.Id == contact.CategoryId).SingleOrDefault()
            };

            User newUser = new User
            {
                Id = Guid.NewGuid(),
                ContactId = newContact.Id,
                Username = contact.Email,
                PwdHash = contact.Password
            };

            _dbContext.Contacts.Add(newContact);
            _dbContext.Users.Add(newUser);
            if (await _dbContext.SaveChangesAsync() > 0)
                return true;
            else
                return false;
        }

        private async Task<string> GenerateToken(string email)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
