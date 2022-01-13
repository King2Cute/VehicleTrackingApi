using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VehicleTracking.Core;
using VehicleTracking.Core.Persistence;
using VehicleTracking.Helpers;
using VehicleTracking.Models.Users;

namespace VehicleTracking.Controllers
{
    public class UserController : Controller
    {
        public UserController(ILogger<User> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _cryptoHelper = new CryptoHelper();
            _userHelper = new UserHelper(_logger, _mongoDbService, _cryptoHelper);
        }

        [HttpGet]
        [Route("api/user")]
        public virtual IActionResult GetUsers()
        {
            return Ok(_userHelper.GetUsers());
        }

        [HttpPost]
        [Route("api/user")]
        public async virtual Task<IActionResult> CreateUser(User user)
        {
            var userId = await _userHelper.CreateUser(user);

            if (userId == null)
                return BadRequest("error creating user");

            return Ok(userId);
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public virtual IActionResult GetUser(Guid id)
        {
            try
            {
                var user = _userHelper.GetUser(id);

                if (user == null)
                    return StatusCode(204, "No user found");

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting user");
                return StatusCode(400, e.Message);
            }
        }

        private readonly ILogger<User> _logger;
        private readonly UserHelper _userHelper;
        private readonly CryptoHelper _cryptoHelper;
        private readonly MongoDbService _mongoDbService;
    }
}
