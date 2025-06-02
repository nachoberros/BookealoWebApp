using Bookealo.CommonModel.Assets;
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

            var results = _assetRepository.GetAssets(accountId);
            return Ok(results);
        }

        [HttpGet("{assetId}")]
        public IActionResult GetById(int assetId)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            var result = _assetRepository.GetAssetById(accountId, assetId);
            if (result == null)
            {
                return NotFound($"Asset with ID {assetId} not found.");
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Save([FromBody] Asset asset)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            _assetRepository.AddAsset(accountId, asset);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Asset asset)
        {
            var stringAccountId = User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
            if (string.IsNullOrEmpty(stringAccountId) || !int.TryParse(stringAccountId, out int accountId))
            {
                return Unauthorized("Account Id claim not found.");
            }

            _assetRepository.UpdateAsset(accountId, asset);
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

            var asset = _assetRepository.GetAssetById(accountId, id);
            if (asset == null)
            {
                return NotFound();
            }

            _assetRepository.RemoveAsset(accountId, asset);

            return Ok();
        }
    }
}
