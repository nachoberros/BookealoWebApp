using Bookealo.CommonModel.Assets;

namespace Bookealo.Services.Interfaces
{
    public interface IAssetRepository
    {
        List<Asset> GetAssets(int accountId);
        void UpdateAsset(int accountId, Asset asset);
        Asset? GetAssetById(int accountId, int assetID);
        void RemoveAsset(int accountId, Asset asset);
        void AddAsset(int accountId, Asset asset);
    }
}
