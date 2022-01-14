using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
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
        public UserController(IConfiguration config, ILogger<User> logger, MongoDbService mongoDbService)
        {
            _config = config;
            _logger = logger;
            _mongoDbService = mongoDbService;
            _cryptoHelper = new CryptoHelper();
            _userHelper = new UserHelper(_config, _logger, _mongoDbService, _cryptoHelper);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user")]
        [SwaggerOperation("GetUsers", Tags = new[] { "Users" })]
        public virtual IActionResult GetUsers()
        {
            return Ok(_userHelper.GetUsers());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/create")]
        [SwaggerOperation("CreateUser", Tags = new[] { "Users" })]
        public async virtual Task<IActionResult> CreateUser([FromBody] User user)
        {
            var userId = await _userHelper.CreateUser(user);

            if (userId == null)
                return BadRequest("error creating user");

            return Ok(userId);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("api/user/{id}")]
        [SwaggerOperation("GetUser", Tags = new[] { "Users" })]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/auth")]
        [SwaggerOperation("Login", Tags = new[] { "Users" })]
        public virtual IActionResult Login([FromBody] User user)
        {
            var token = _userHelper.Authenticate(user.Email, user.Password);

            if (token == null)
                return Unauthorized();

            return Ok(new { token, user });
        }

        private readonly IConfiguration _config;
        private readonly ILogger<User> _logger;
        private readonly UserHelper _userHelper;
        private readonly CryptoHelper _cryptoHelper;
        private readonly MongoDbService _mongoDbService;
    }
}
