using Bookealo.CommonModel.Users;
using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize(Policy = "CanManageUser")]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AssetController> _logger;

        public AssetController(IAssetRepository assetRepository, IUserRepository userRepository, ILogger<AssetController> logger)
        {
            _assetRepository = assetRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var results = _userRepository.GetUsers(accountId);
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Save([FromBody] User user)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            _userRepository.AddUser(accountId, user);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            _userRepository.UpdateUser(accountId, user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var user = _userRepository.GetUserById(accountId, id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.RemoveUser(accountId, user);

            return Ok();
        }
    }
}
