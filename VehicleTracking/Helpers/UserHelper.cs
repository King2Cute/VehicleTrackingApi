using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.Core;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Models.Users;

namespace VehicleTracking.Helpers
{
    public class UserHelper
    {
        public UserHelper(ILogger<User> logger, MongoDbService mongoDbService, CryptoHelper cryptoHelper)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _cryptoHelper = cryptoHelper;
        }

        public string Authenticate(string email, string password)
        {
            string savedHashPassword = _mongoDbService.Users.AsQueryable().Where(x => x.Email == email).First().Password;

            if (savedHashPassword == null)
                return null;

            bool checkPassword = _cryptoHelper.CheckPassword(savedHashPassword, password);

            if (!checkPassword)
                return null;
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

        private readonly ILogger _logger;
        private readonly MongoDbService _mongoDbService;
        private readonly CryptoHelper _cryptoHelper;
    }
}
