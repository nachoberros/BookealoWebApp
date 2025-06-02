using Bookealo.CommonModel.Users;
using Bookealo.Services.Implementations;
using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize(Policy = "CanManageUser")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BookealoBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email claim not found.");
            }

            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var results = _userRepository.GetUsers(AccountId.Value);
            return Ok(results);
        }

        [HttpGet("{userId}")]
        public IActionResult GetById(int userId)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var result = _userRepository.GetUserById(AccountId.Value, userId);
            if (result == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Save([FromBody] User user)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            _userRepository.AddUser(AccountId.Value, user);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            _userRepository.UpdateUser(AccountId.Value, user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var user = _userRepository.GetUserById(AccountId.Value, id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.RemoveUser(AccountId.Value, user);

            return Ok();
        }
    }
}
