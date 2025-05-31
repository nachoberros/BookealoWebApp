using Bookealo.CommonModel.Calendars.Enum;

namespace Bookealo.CommonModel.Calendars
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AssetType Type { get; set; }
    }
}
