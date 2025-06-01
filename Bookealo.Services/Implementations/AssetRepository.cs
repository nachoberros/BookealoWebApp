using Bookealo.CommonModel.Assets;
using Bookealo.Services.Interfaces;

namespace Bookealo.Services.Implementations
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IMockingRepository _mockingRepository;

        public AssetRepository(IMockingRepository mockingRepository) 
        {
            _mockingRepository = mockingRepository;
        }

        public List<Asset> GetAssets(int accountId)
        {
            return _mockingRepository.GetAssets(accountId);
        }

        public void UpdateAsset(int accountId, Asset asset)
        {
            _mockingRepository.UpdateAsset(accountId, asset);
        }

        public Asset? GetAssetById(int accountId, int assetID)
        {
            return _mockingRepository.GetAssets(accountId).FirstOrDefault(c => c.Id == assetID);
        }

        public void RemoveAsset(int accountId, Asset asset)
        {
            _mockingRepository.RemoveAsset(accountId, asset);
        }

        public void AddAsset(int accountId, Asset asset)
        {
            _mockingRepository.AddAsset(accountId, asset);
        }
    }
}
