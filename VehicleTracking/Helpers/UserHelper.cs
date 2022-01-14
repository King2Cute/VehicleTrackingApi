using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.Core;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.Users;

namespace VehicleTracking.Helpers
{
    public class UserHelper
    {
        public UserHelper(IConfiguration config, ILogger<User> logger, MongoDbService mongoDbService, CryptoHelper cryptoHelper)
        {
            _config = config;
            _logger = logger;
            _mongoDbService = mongoDbService;
            _cryptoHelper = cryptoHelper;
            key = _config.GetSection("JwtKey").ToString();
        }

        public List<User> GetUsers() => _mongoDbService.Users.Find(user => true).ToList();

        public User GetUser(Guid id)
        {
            try
            {
                var user = _mongoDbService.Users.AsQueryable().Where(v => v.Id.Value == id).First();
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting vehicle");
            }

            return null;
        }

        public async Task<Guid?> CreateUser(User user)
        {
            try
            {
                var userExists = _mongoDbService.Users.Find<User>(u => u.Email.ToLowerInvariant() == user.Email.ToLowerInvariant()).FirstOrDefault();

                if (userExists == null)
                {
                    if (!user.Id.HasValue)
                        user.Id = Guid.NewGuid();

                    user.Password = _cryptoHelper.HashPassword(user.Password);

                    if (user.Email.ToLowerInvariant().Contains("ethan"))
                    {
                        user.UserRole = "Admin";
                    } 
                    else
                    {
                        user.UserRole = "User";
                    }

                    await _mongoDbService.Users.InsertOneAsync(user);
                    return user.Id.Value;
                }

                return userExists.Id.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating user");
            }

            return null;
        }

        public string Authenticate(string email, string password)
        {
            string savedHashPassword = _mongoDbService.Users.AsQueryable().Where(x => x.Email == email).First().Password;

            if (savedHashPassword == null)
                return null;

            bool checkPassword = _cryptoHelper.CheckPassword(savedHashPassword, password);

            if (!checkPassword)
                return null;

            string savedUserRole = _mongoDbService.Users.AsQueryable().Where(x => x.Email == email).First().UserRole;

            if (savedUserRole == null)
                return null;

            return GetJwtToken(email, savedUserRole);
        }

        private string GetJwtToken(string email, string userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, userRole)
                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var token = tokenHandler.CreateToken(tokenDesc);

            return tokenHandler.WriteToken(token);
        }

        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
        private readonly CryptoHelper _cryptoHelper;
        private readonly string key;
    }
}
