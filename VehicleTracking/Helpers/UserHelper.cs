using Microsoft.AspNetCore.Http;
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

                    var updatedUser = SetRoles(user);

                    await _mongoDbService.Users.InsertOneAsync(updatedUser);
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
            try
            {
                User user = _mongoDbService.Users.AsQueryable().Where(x => x.Email == email).First();

                if (user == null)
                    return null;

                bool checkPassword = _cryptoHelper.CheckPassword(user.Password, password);

                if (!checkPassword)
                    return null;

                List<string> savedUserRoles = _mongoDbService.Users.AsQueryable().Where(x => x.Email == email).First().UserRoles;

                if (savedUserRoles == null)
                    return null;

                return GetJwtToken((Guid)user.Id, savedUserRoles);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        private User SetRoles(User user)
        {
            var userRoles = new List<string>();

            if (user.Email.ToLowerInvariant().Contains("admin"))
            {
                userRoles.Add("Admin");
                userRoles.Add("User");
            } else
            {
                userRoles.Add("User");
            }

            user.UserRoles = userRoles;

            return user;
        }

        private string GetJwtToken(Guid userId, List<string> userRoles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userId)));

            foreach(var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddHours(1),

                Issuer = "https://localhost:5001/",

                IssuedAt = DateTime.Now,

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
