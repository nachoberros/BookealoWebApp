using Bookealo.CommonModel.Assets;
using Bookealo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookealoWebApp.Server.Controllers
{
    [Authorize(Policy = "CanManageUser")]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : BookealoBaseController
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
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var results = _assetRepository.GetAssets(AccountId.Value);
            return Ok(results);
        }

        [HttpGet("{assetId}")]
        public IActionResult GetById(int assetId)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var result = _assetRepository.GetAssetById(AccountId.Value, assetId);
            if (result == null)
            {
                return NotFound($"Asset with ID {assetId} not found.");
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Save([FromBody] Asset asset)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            _assetRepository.AddAsset(AccountId.Value, asset);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Asset asset)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            _assetRepository.UpdateAsset(AccountId.Value, asset);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            if (AccountId == null) return Unauthorized("Account Id claim not found.");

            var asset = _assetRepository.GetAssetById(AccountId.Value, id);
            if (asset == null)
            {
                return NotFound();
            }

            _assetRepository.RemoveAsset(AccountId.Value, asset);

            return Ok();
        }
    }
}
