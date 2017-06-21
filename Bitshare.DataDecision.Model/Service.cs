using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    public class Service
    {
        public ObjectId _id { get; set; }
        public int Rid { get; set; }
        public int? Type { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public int? Dependency { get; set; }
        public int? DeployLocale { get; set; }
        public int? DeployDeviceType { get; set; }
        public string Remark { get; set; }
    }
}
