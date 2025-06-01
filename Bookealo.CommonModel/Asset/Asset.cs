using Bookealo.CommonModel.Assets.Enum;

namespace Bookealo.CommonModel.Assets
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AssetBooking> Bookings { get; set; }
        public List<AssetBooking> Blockings { get; set; }
        public AssetType Type { get; set; }
    }
}
